﻿using System;
using System.Collections.Generic;
using IDAL.DO;
using DalObject;
namespace ConsoleUI
{
    public class ConsoleUI
    {
        public static IDAL.IDal myDalObject;
        public enum Inputs { a = 1, p, d, l, e };
        public enum Adding { nBaseStation = 1, nDrone, nCustomer, nParcel };//for the main
        public enum Updating { AscPtoD = 1, PUParcel, PDelivering, DCharging, DRelease };//for the main
        public enum Displaying { DBaseStation = 1, DDrone, DCustomer, DParcel };//for the main
        public enum ListsDisplaying { BaseStationsList = 1, DronesList, CustomersList, ParcelsList, UnAscriptedParcelsList, AvailableChargingStationsList };//for the main
        static void Main(string[] args)
        {
            myDalObject = new DalObject.DalObject();
            DataSource.Initialize();
            Inputs options;
            do
            {
                Console.WriteLine("Choose one of the following options:\n" +
                "a: Add new element\n" +
                "p: Updating existing element\n" +
                "d: Display an element\n" +
                "l: display a list\n" +
                "e: exit\n");
                string input;
                string inp;
                input = Console.ReadLine();
                Inputs.TryParse(input, out options);
                switch (options)
                {
                    case Inputs.a:
                        Console.WriteLine("What do you want to add?\n" +
                "nBaseStation: Add new base station\n" +
                "nDrone: Add new drone\n" +
                "nCustomer: Add new customer\n" +
                "nParcel: Add new parcel\n");
                        Adding a;
                        string input1;
                        input1 = Console.ReadLine();
                        Adding.TryParse(input1, out a);
                        switch (a)
                        {
                            case Adding.nBaseStation:
                                Console.WriteLine("please enter:\n" + "base station's id:\n");
                                //string inp;
                                int BSId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out BSId);
                                Console.WriteLine("base station's name:\n");
                                string BSName;
                                BSName = Console.ReadLine();
                                Console.WriteLine("base station's charge slots:\n");
                                inp = Console.ReadLine();
                                int BSChargeSlots;
                                int.TryParse(inp, out BSChargeSlots);
                                Console.WriteLine("base station's longitude:\n");
                                inp = Console.ReadLine();
                                double BSLongitude;
                                double.TryParse(inp, out BSLongitude);
                                Console.WriteLine("base station's lattitude:\n");
                                inp = Console.ReadLine();
                                double BSLattitude;
                                double.TryParse(inp, out BSLattitude);
                                try
                                { myDalObject.AddBaseStation(BSId, BSName, BSChargeSlots, BSLongitude, BSLattitude); }//create a new base station with new values
                                catch (AddExistingBaseStationException)
                                { Console.WriteLine("ERROR - attempt to add an existing base station!\n"); }
                                break;
                            case Adding.nDrone:
                                Console.WriteLine("please enter:\n" + "drone's id\n");
                                int DId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out DId);
                                Console.WriteLine("drone's battery:\n");
                                inp = Console.ReadLine();
                                double DBattery;
                                double.TryParse(inp, out DBattery);
                                WeightCategories DWC;
                                Console.WriteLine("drone's weight categories (Heavy, Light, Middle):\n");
                                inp = Console.ReadLine();
                                WeightCategories.TryParse(inp, out DWC);
                                Console.WriteLine("drone's model\n");
                                string DModel;
                                DModel = Console.ReadLine();
                                try
                                { myDalObject.AddDrone(DId, DBattery, DWC, DModel);}//create a new drone with new values
                                catch (AddExistingDroneException)
                                { Console.WriteLine("ERROR - attempt to add an existing drone!\n"); }
                                break;
                            case Adding.nCustomer:
                                Console.WriteLine("please enter:\n+ customer's id:\n");
                                int CId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out CId);
                                Console.WriteLine("customer's name:\n");
                                string CName;
                                CName = Console.ReadLine();
                                Console.WriteLine("customer's phone:\n");
                                string CPhone;
                                CPhone = Console.ReadLine();
                                Console.WriteLine("customer's longitude:\n");
                                inp = Console.ReadLine();
                                double CLongitude;
                                double.TryParse(inp, out CLongitude);
                                Console.WriteLine("customer's lattitude:\n");
                                inp = Console.ReadLine();
                                double CLattitude;
                                double.TryParse(inp, out CLattitude);
                                try
                                { myDalObject.AddCustomer(CId, CName, CPhone, CLongitude, CLattitude); }//adding new customer with new values
                                catch (AddExistingCustomerException)
                                { Console.WriteLine("ERROR - attempt to add an existing customer!\n"); }
                                break;
                            case Adding.nParcel:
                                Console.WriteLine("please enter:\n" + "drone id:\n");
                                inp = Console.ReadLine();
                                int PDId;
                                int.TryParse(inp, out PDId);
                                Console.WriteLine("sender id:\n");
                                inp = Console.ReadLine();
                                int PSId;
                                int.TryParse(inp, out PSId);
                                Console.WriteLine("target id:\n");
                                inp = Console.ReadLine();
                                int PTId;
                                int.TryParse(inp, out PTId);
                                Console.WriteLine("parcel's priority (Fast, Standart, Urgent:\n)");
                                inp = Console.ReadLine();
                                Priorities PPriority;
                                Priorities.TryParse(inp, out PPriority);
                                Console.WriteLine("parcel's weight category(Heavy, Light, Middle)\n");
                                inp = Console.ReadLine();
                                WeightCategories PWC;
                                WeightCategories.TryParse(inp, out PWC);
                                DateTime? PRT = DateTime.Now;//parcel requested time
                                DateTime? PST = null;//parcel scheduled time
                                DateTime? PPUT = null;//parcel pick up time
                                DateTime? PDT = null;//parcel delivery time
                                try
                                { myDalObject.AddParcel(PDId, PSId, PTId, PPriority, PWC, PRT, PST, PPUT, PDT); }//create a new parcel with new values
                                catch (IDAL.DO.AddParcelToAnAsscriptedDroneException)
                                { Console.WriteLine("ERROR - attempt to ascript a parcel to an ascripted drone!\n"); }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Inputs.p:
                        Console.WriteLine("What do you want to update?\n" +
                            "AscPtoD: ascription a parcel to a drone\n" +
                            "PUParcel: pick up a parcel by a drone\n" +
                            "PDelivering: The parcel was delivered to the customer\n" +
                            "DCharging: Sending a drone for charging at a base station \n" +
                            "DRelease: Release drone from charging at base station\n");
                        Updating u;
                        inp = Console.ReadLine();
                        Updating.TryParse(inp, out u);
                        switch (u)
                        {
                            case Updating.AscPtoD:
                                Console.WriteLine("please enter:\n" + "parcel id:\n");
                                inp = Console.ReadLine();
                                int UPId;//update parcel id
                                int.TryParse(inp, out UPId);
                                Console.WriteLine("drone id:\n");
                                inp = Console.ReadLine();
                                int UDId;//update drone id
                                int.TryParse(inp, out UDId);
                                try
                                { myDalObject.AscriptionPtoD(UPId, UDId); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to ascript a parcel to a non-exists drone!\n"); }
                                catch (ParcelIdNotFoundException)
                                { Console.WriteLine("ERROR - attemp to update a non-existing parcel!\n"); }
                                break;
                            case Updating.PUParcel:
                                Console.WriteLine("please enter a parcel:\n");
                                inp = Console.ReadLine();
                                int PUP;//pick up parcel
                                int.TryParse(inp, out PUP);
                                try
                                { myDalObject.PickUpParcel(PUP); }
                                catch (ParcelIdNotFoundException)
                                { Console.WriteLine("ERROR - attemp to pick up a non-existing parcel!\n"); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to pick up a parcel by a non-exists drone!\n"); }
                                break;
                            case Updating.PDelivering:
                                Console.WriteLine("please enter parcel id:\n");
                                inp = Console.ReadLine();
                                int PDeId;
                                int.TryParse(inp, out PDeId);
                                try
                                { myDalObject.ParcelDelivering(PDeId); }
                                catch (ParcelIdNotFoundException)
                                { Console.WriteLine("ERROR - attemp to deliver a non-existing parcel!\n"); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to deliver a parcel by a non-exists drone!\n"); }
                                break;
                            case Updating.DCharging:
                                Console.WriteLine("please enter:\n" + "drone id:\n");
                                inp = Console.ReadLine();
                                int DCDId; //drone charging drone id
                                int.TryParse(inp, out DCDId);
                                Console.WriteLine("base station id:\n");
                                inp = Console.ReadLine();
                                int DCBSId;//drone charging base station id
                                int.TryParse(inp, out DCBSId);
                                try
                                { myDalObject.DroneCharging(DCDId, DCBSId); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to charge a non-exists drone!\n"); }
                                break;
                            case Updating.DRelease:
                                Console.WriteLine("please enter:\n" + "drone id:\n");
                                inp = Console.ReadLine();
                                int DRDId; //drone release drone id
                                int.TryParse(inp, out DRDId);
                                Console.WriteLine("base station id:\n");
                                inp = Console.ReadLine();
                                int DRBSId;//drone release base station id
                                int.TryParse(inp, out DRBSId);
                                try
                                { myDalObject.DroneRelease(DRDId, DRBSId); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to release a non-exists drone!\n"); }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Inputs.d:
                        Console.WriteLine("What are you want to display?\n" +
                            "DBaseStation: display a base station\n" +
                            "DDrone: display a drone\n" +
                            "DCustomer: display a customer\n" +
                            "DParcel: display a parcel\n");
                        inp = Console.ReadLine();
                        Displaying d = new Displaying();
                        Displaying.TryParse(inp, out d);
                        switch (d)
                        {
                            case Displaying.DBaseStation:
                                Console.WriteLine("please enter base station's id:\n");
                                inp = Console.ReadLine();
                                int DBSId;//display base station id
                                int.TryParse(inp, out DBSId);
                                try
                                { Console.WriteLine(myDalObject.CopyBaseStation(DBSId)); }
                                catch (BaseStationNotFoundException)
                                { Console.WriteLine("ERROR - attempt to display a non-existsing base station!\n"); }
                                break;
                            case Displaying.DDrone:
                                Console.WriteLine("please enter drone's id:\n");
                                inp = Console.ReadLine();
                                int DDId;//display drone id
                                int.TryParse(inp, out DDId);
                                try
                                { Console.WriteLine(myDalObject.CopyDrone(DDId)); }
                                catch (DroneIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to display a non-existing drone!\n"); }
                                break;
                            case Displaying.DCustomer:
                                Console.WriteLine("please enter customer's id:\n");
                                inp = Console.ReadLine();
                                int DCId;//display customer id
                                int.TryParse(inp, out DCId);
                                try
                                { Console.WriteLine(myDalObject.CopyCustomer(DCId)); }
                                catch (CustomerNotFoundException)
                                { Console.WriteLine("ERROR - attempt to display a non-existing cutomer!\n"); }
                                break;
                            case Displaying.DParcel:
                                Console.WriteLine("please enter parcel's id:\n");
                                inp = Console.ReadLine();
                                int DPId;//display parcel id
                                int.TryParse(inp, out DPId);
                                try
                                { Console.WriteLine(myDalObject.CopyParcel(DPId)); }
                                catch (ParcelIdNotFoundException)
                                { Console.WriteLine("ERROR - attempt to display a non - existing parcel!\n"); }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Inputs.l:
                        Console.WriteLine("Which list do you want to display?\n" +
                            "BaseStationsList: the list of base stations\n" +
                            "DronesList: the list of drones\n" +
                            "CustomersList: the list of customers\n" +
                            "ParcelsList: the list of parcels\n" +
                            "UnAscriptedParcelsList: the list of un-ascriptes parcels\n" +
                            "AvailableChargingStationsList: the list of base stations with available charging slots\n");
                        inp = Console.ReadLine();
                        ListsDisplaying ld = new ListsDisplaying();
                        ListsDisplaying.TryParse(inp, out ld);
                        switch (ld)
                        {
                            case ListsDisplaying.BaseStationsList:
                                IEnumerable<BaseStation> BaseStations;
                                BaseStations = myDalObject.CopyBaseStations();
                                { Console.WriteLine("ERROR - attempt to copy a non-existing base station"); }
                                foreach (BaseStation basestation in BaseStations)
                                {
                                    Console.WriteLine(basestation);
                                }
                                break;
                            case ListsDisplaying.DronesList:
                                IEnumerable<Drone> Drones = myDalObject.CopyDronesList();
                                foreach (Drone drones in Drones)
                                {
                                    Console.WriteLine(drones);
                                }
                                break;
                            case ListsDisplaying.CustomersList:
                                IEnumerable<Customer> Customers;
                                Customers = myDalObject.CopyCustomersList();
                                foreach (Customer customer in Customers)
                                {
                                    Console.WriteLine(customer);
                                }
                                break;
                            case ListsDisplaying.ParcelsList:
                                IEnumerable<Parcel> Parcels;
                                Parcels = myDalObject.CopyParcelsList();
                                foreach (Parcel parcel in Parcels)
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case ListsDisplaying.UnAscriptedParcelsList:
                                IEnumerable<Parcel> UAParcels = new List<Parcel>();
                                UAParcels = myDalObject.UnAscriptedParcels();
                                foreach (Parcel parcel in UAParcels)
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case ListsDisplaying.AvailableChargingStationsList:
                                IEnumerable<BaseStation> ACSList = new List<BaseStation>();
                                ACSList = myDalObject.AvailableBaseStation();
                                foreach (BaseStation basestation in ACSList)
                                {
                                    Console.WriteLine(basestation);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Inputs.e:
                        Console.WriteLine("Bye!");
                        break;
                    default:
                        break;
                }

            } while (!(options == Inputs.e));

        }
    }
}
