using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using System.Threading;
using static BL.BL;
namespace BL
{
    internal class Simulator
    {
        private const double VELOCITY = 1.0;//1 kilometer per second
        private const int DELAY = 1000; //every 1000 milisec - 1 second of delay 
        private const double TIME_STEP = DELAY / 1000.0;//time in seconds of each pause step
        private const double STEP = VELOCITY * TIME_STEP;//the distance the drone is moving in 1 delay
        private double[] batteryConsuming = new double[5];
        private BL myBl;
        private DalApi.IDAL myDal;
        private Action updateDisplay;
        public Simulator(BL bl, int droneId, Action updateDisplay, Func<bool> stopCheck)
        {
            myBl = bl;
            myDal = myBl.myDal;
            Drone drone;
            double timeInCharge;
            TimeSpan time;
            double battery, originalBattery;
            Location originalLoction;
            lock (myDal) { batteryConsuming = myDal.DronePowerConsumingPerKM(); }
            this.updateDisplay = updateDisplay;
            while (!stopCheck())//in send drone to charge the drone dousnt in the drones in charge list
            {
                lock (bl) { drone = bl.GetDrone(droneId); }
                if (drone.DroneStatus == Enums.DroneStatuses.Available)
                {
                    try//maybe need to add catch of exception
                    {
                        lock (myBl) { myBl.AscriptionParcelToDrone(droneId); }
                        updateDisplay();
                        Thread.Sleep(DELAY);
                    }
                    catch (NoParcelAscriptedToDroneException)
                    {
                        lock (myBl) { try { bl.DroneToCharge(droneId); } catch (NoChargingSlotIsAvailableException) { } }
                        updateDisplay();
                        Thread.Sleep(DELAY);
                    }//the drone is available (but cant perform new delivary) which meen he is at the target location - so we send it to charge   
                }
                if (drone.DroneStatus == Enums.DroneStatuses.Maintenance)
                {
                    battery = drone.Battery;
                    while (battery < 100.0)
                    {

                        lock (myBl) { time = DateTime.Now - myBl.GetInsertionTime(droneId); }
                        timeInCharge = time.TotalSeconds;
                        lock (myBl) { myBl.ReleaseDroneFromCharge(droneId, timeInCharge); myBl.DroneToCharge(droneId); }
                        battery += batteryConsuming[4] * timeInCharge;
                        updateDisplay();
                        Thread.Sleep(DELAY);
                    }
                }
                //in the display update, its update the batter and location in the bl and data source, so in order of the pickup/delivery to work as excpected 
                //we need to save the battery and location before the udpate, then udpate the data source with the original battery and location
                if (drone.DroneStatus == Enums.DroneStatuses.Shipping)
                {
                    originalBattery = drone.Battery;
                    originalLoction = drone.CurrentLocation;
                    if (drone.DeliveryParcel.ParcelState)//true if the parcel picked up, false if its waiting for pick up 
                    {
                        //the parcel picked up - the drone is at the sender's location and need to get to the target
                        displayUpdate(drone, 'd');
                        drone.Battery = originalBattery;
                        drone.CurrentLocation = originalLoction;
                        lock (myBl) { bl.DeliveringParcelByDrone(droneId); updateDisplay(); }
                        Thread.Sleep(DELAY);
                    }
                    else
                    {
                        //the parcel need to be picked up
                        displayUpdate(drone, 'p'); //here implement an update of the location and battery each DELAY - didnt yet
                        drone.Battery = originalBattery;
                        drone.CurrentLocation = originalLoction;
                        lock (myBl) { bl.PickUpParcel(droneId); updateDisplay(); }
                        Thread.Sleep(DELAY);
                    }
                }
            }
        }
        /// <summary>
        /// function that update the display every DELAY milisec, given a drone
        /// update the location and battery of a drone
        /// </summary>
        /// <param name="drone">drone</param>
        ///  <param name="c">pickup/delivery</param>
        private void displayUpdate(Drone drone, char c)
        {
            Location start, end;
            if (c == 'p')//update for a drone that pick up a parcel
            {
                start = drone.CurrentLocation;
                end = drone.DeliveryParcel.SenderLocation;
            }
            else//update for a drone that deliver a parcel
            {
                start = drone.DeliveryParcel.SenderLocation;
                end = drone.DeliveryParcel.TargetLocation;
            }
            int countSteps = 1;
            //לולאה שנמשכת עד שהמיקום של הרחפן מגיע למיקום היעד
            while (drone.CurrentLocation != end)
            {
                drone.CurrentLocation = StepLocation(start, end, STEP * countSteps);
                countSteps++;
                lock (myBl) { drone.Battery -= batteryConsuming[(int)drone.DeliveryParcel.ParcelWC] * STEP * countSteps; }//battery consuming acording to the weight of the parcel multiply the distance
                lock (myBl) { updateDrone(drone); }//update the bl and dal with the new values of the battery and the location of the drone
                updateDisplay();//display the changes in the PL layer
                Thread.Sleep(DELAY);
            }
        }
        /// <summary>
        /// return the location of the drone
        /// the new location is the location that made from progress in a DELAY time
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distance">the distance from the start location</param>
        /// <returns></returns>
        private Location StepLocation(Location start, Location end, double distance)
        {
            double distanceBetweenStartEnd = BL.Distance(start.Long, start.Lat, end.Long, end.Lat);
            double d = distance / distanceBetweenStartEnd;
            double newLongitude = start.Long + (d * (end.Long - start.Long));//clculating the new longitude
            double newLattitude = start.Lat + (d * (end.Lat - start.Lat));//calculating the new lattitude
            if (BL.Distance(end.Long, end.Lat, newLongitude, newLattitude) >= 0)//if the new location is bigger then the end location, we would like to return the end location
                return end;
            else
                return new Location(newLongitude, newLattitude);//if the new location hadnt past the end location
        }
        /// <summary>
        /// given a new drone - name drone, the function update the data source to the data in the given drone
        /// </summary>
        /// <param name="drone"></param>
        private void updateDrone(Drone drone)
        {
            lock (myDal) { myDal.RemoveDrone(drone.DroneId); }
            lock (myDal)
            {
                myDal.AddDrone(drone.DroneId, drone.Battery, (DO.WeightCategories)drone.MaxWeight, drone.Model);
            }
            lock (myBl)
            {
                myBl.updateDroneForList(new DroneForList { DroneId = drone.DroneId, Model = drone.Model, MaxWeight = drone.MaxWeight, Battery = drone.Battery, CurrentLocation = drone.CurrentLocation, DroneState = drone.DroneStatus, InDeliveringParcelId = drone.DeliveryParcel.ParcelId });
            }
        }
    }
}
