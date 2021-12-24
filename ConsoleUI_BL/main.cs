using System;
using BL.BO;
namespace ConsoleUI_BL
{
    class ConsoleUI_BL
    {
        static void Main(string[] args)
        {
            BL.BlApi.IBL bl = BL.BlApi.BLFactory.GetBL("1");
            Enums.Inputs option;
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
                Enums.Inputs.TryParse(input, out option);
                switch (option)
                {
                    case Enums.Inputs.a:
                        Console.WriteLine("What do you want to add?\n" +
                        "nBaseStation: Add new base station\n" +
                        "nDrone: Add new drone\n" +
                        "nCustomer: Add new customer\n" +
                        "nParcel: Add new parcel\n");
                        Enums.Adding a;
                        string input1;
                        input1 = Console.ReadLine();
                        Enums.Adding.TryParse(input1, out a);
                        switch (a)
                        {
                            case Enums.Adding.nBaseStation:
                                Console.WriteLine("please enter:\n" + "base station's id:\n");
                                //string inp;
                                int BSId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out BSId);
                                Console.WriteLine("base station's name:");
                                string BSName;
                                BSName = Console.ReadLine();
                                Console.WriteLine("base station's longitude:");
                                inp = Console.ReadLine();
                                double BSLongitude;
                                double.TryParse(inp, out BSLongitude);
                                Console.WriteLine("base station's lattitude:");
                                inp = Console.ReadLine();
                                double BSLattitude;
                                double.TryParse(inp, out BSLattitude);
                                Console.WriteLine("base station's charge slots:");
                                inp = Console.ReadLine();
                                int BSChargeSlots;
                                int.TryParse(inp, out BSChargeSlots);
                                Location BSLocation = new Location(BSLongitude, BSLattitude);
                                try { bl.AddBaseStation(BSId, BSName, BSLocation, BSChargeSlots); }//create a new base station with new values
                                catch (AddExistingBaseStationException) { Console.WriteLine("ERROR - attemp to add an existing base station\n"); }
                                catch (LocationOutOfRangeException) { Console.WriteLine("ERROR- attemp to add base station out of Jerusalem\n"); }
                                break;
                            case Enums.Adding.nDrone:
                                Console.WriteLine("please enter:\n" + "drone's id\n");
                                int DId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out DId);
                                Console.WriteLine("drone's model\n");
                                string DModel;
                                DModel = Console.ReadLine();
                                Enums.WeightCategories DWC;
                                Console.WriteLine("drone's Max weight categories (Heavy, Light, Middle):\n");
                                inp = Console.ReadLine();
                                Enums.WeightCategories.TryParse(inp, out DWC);
                                Console.WriteLine("base station's number for first charging:");
                                inp = Console.ReadLine();
                                int StationForCharging;
                                int.TryParse(inp, out StationForCharging);
                                try { bl.AddDrone(DId, DModel, DWC, StationForCharging); }//create a new drone with new values
                                catch (BaseStationNotFoundException) { Console.WriteLine("ERROR - the base station for first charging not found\n"); }
                                catch (AddExistingDroneException) { Console.WriteLine("ERROR - attemp to add an existing drone\n"); }
                                catch (LocationOutOfRangeException) { Console.WriteLine("ERROR- attemp to add a drone out of Jerusalem\n"); }
                                break;
                            case Enums.Adding.nCustomer:
                                Console.WriteLine("please enter:\n" + "customer's id:\n");
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
                                Location Clocation = new Location(CLongitude, CLattitude);
                                try { bl.AddCustomer(CId, CName, CPhone, Clocation); }//adding new customer with new values
                                catch (AddExistingCustomerException) { Console.WriteLine("ERROR - attemp to add an existing customer\n"); }
                                catch (LocationOutOfRangeException) { Console.WriteLine("ERROR- attemp to add a customer out of Jerusalem\n"); }
                                break;
                            case Enums.Adding.nParcel:
                                Console.WriteLine("please enter:\n");
                                Console.WriteLine("sender id:\n");
                                inp = Console.ReadLine();
                                int PSId;
                                int.TryParse(inp, out PSId);
                                Console.WriteLine("destination id:\n");
                                inp = Console.ReadLine();
                                int PDeId;
                                int.TryParse(inp, out PDeId);
                                Console.WriteLine("parcel's weight category(Heavy, Light, Middle)\n");
                                inp = Console.ReadLine();
                                Enums.WeightCategories PWC;
                                Enums.WeightCategories.TryParse(inp, out PWC);
                                Console.WriteLine("parcel's priority (Fast, Standart, Urgent:\n)");
                                inp = Console.ReadLine();
                                Enums.Priorities PPriority;
                                Enums.Priorities.TryParse(inp, out PPriority);
                                bl.AddParcelToDeliver(PSId, PDeId, PWC, PPriority);//create a new parcel with new values
                                break;
                            default:
                                break;
                        }
                        break;
                    case Enums.Inputs.p:
                        {
                            Console.WriteLine("What do you want to update?\n" +
                              "DroneModel: Update drone's name\n" +
                              "BaseStation: Update a base station\n" +
                              "RBaseStation: remove a base station\n"+
                              "Customer: Update a customer\n" +
                              "DroneToAscriptionPToDCharge: Sending a drone to charge at a base station \n" +
                              "DroneRealese: Release drone from charging at a base station\n" +
                              "Asc: Ascription of a parcel to a drone\n" +
                              "PickUpParcel: picking up parcel by a drone\n" +
                              "DeliveringPByD: delivere of a parcel by a drone\n");
                            Enums.NewUpdating u;
                            inp = Console.ReadLine();
                            Enums.Updating.TryParse(inp, out u);
                            switch (u)
                            {
                                case Enums.NewUpdating.DroneModel:
                                    Console.WriteLine("please enter:\n" + "drone id:\n");
                                    inp = Console.ReadLine();
                                    int NewDID;
                                    int.TryParse(inp, out NewDID);
                                    Console.WriteLine("drone's new model:\n");
                                    inp = Console.ReadLine();
                                    bl.UpdateDrone(NewDID, inp);
                                    break;
                                case Enums.NewUpdating.BaseStation:
                                    Console.WriteLine("please enter:\n" + "base station's id:\n");
                                    inp = Console.ReadLine();
                                    int BSId;
                                    int.TryParse(inp, out BSId);
                                    Console.WriteLine("new name for the base station(optional, else enter' '):\n");
                                    string BSName = Console.ReadLine();
                                    Console.WriteLine("new number of charge slots(optional, else enter -1):\n");
                                    inp = Console.ReadLine();
                                    int CSNumber;//charge slots number
                                    int.TryParse(inp, out CSNumber);
                                    try { bl.UpdateBaseStation(BSId, BSName, CSNumber); }
                                    catch (BaseStationNotFoundException) { Console.WriteLine("ERROR - attemp to update non-exists base station\n"); }
                                    break;
                                case Enums.NewUpdating.RBaseStation:
                                    Console.WriteLine("please enter base station's id:");
                                    inp = Console.ReadLine();
                                    int BsId;
                                    int.TryParse(inp, out BsId);
                                    try { bl.removeBaseStation(BsId); }
                                    catch (BaseStationNotFoundException) { Console.WriteLine("Sorry, this base station is not exist\n"); }
                                    break;
                                case Enums.NewUpdating.Customer:
                                    Console.WriteLine("please enter:\n" + "customer id:\n");
                                    inp = Console.ReadLine();
                                    int CId;
                                    int.TryParse(inp, out CId);
                                    Console.WriteLine("new customer name (optional, else enter ' '):\n");
                                    string CName = Console.ReadLine();
                                    Console.WriteLine("new customer's phone number(optional, else enter ' '):\n");
                                    inp = Console.ReadLine();
                                    string CPNum;
                                    CPNum = inp;
                                    try { bl.UpdateCustomer(CId, CName, CPNum); }
                                    catch (AddExistingCustomerException) { Console.WriteLine("ERROR - attemp to update non-existing "); }
                                    break;
                                case Enums.NewUpdating.DroneToCharge:
                                    Console.WriteLine("plese enter drone's id:\n");
                                    inp = Console.ReadLine();
                                    int DroneId;
                                    int.TryParse(inp, out DroneId);
                                    bl.DroneToCharge(DroneId);
                                    break;
                                case Enums.NewUpdating.DroneRealese:
                                    Console.WriteLine("please enter:\n" + "drone's id:");
                                    inp = Console.ReadLine();
                                    int DId2;
                                    int.TryParse(inp, out DId2);
                                    Console.WriteLine("how long the drone has been charging? (in hours)\n");
                                    inp = Console.ReadLine();
                                    double TICharging;//time in charging
                                    double.TryParse(inp, out TICharging);
                                    try { bl.ReleaseDroneFromCharge(DId2, TICharging); }
                                    catch (BaseStationNotFoundException) { Console.WriteLine("ERROR - attemp to relese drone from a non-existing charge station\n"); }
                                    catch (AddExistingBaseStationException) { Console.WriteLine("ERROR - sorry, we were unable to relese the drone from the charge station\n"); }
                                    break;
                                case Enums.NewUpdating.AscriptionPToD:
                                    Console.WriteLine("please enter drone's id:\n");
                                    inp = Console.ReadLine();
                                    int DId;
                                    int.TryParse(inp, out DId);
                                    try { bl.AscriptionParcelToDrone(DId); }
                                    catch (NoParcelAscriptedToDroneException) { Console.WriteLine("ERROR - sorry, we could not found a parcel to ascript with the drone\n"); }
                                    catch (DroneIdNotFoundException) { Console.WriteLine("ERROR - attemp to ascript a non-exists drone\n"); }
                                    catch (ParcelIdNotFoundException) { Console.WriteLine("ERROR - attemp to ascript a non-exists parcel\n"); }
                                    break;
                                case Enums.NewUpdating.PickUpParcel:
                                    Console.WriteLine("please enter drone's id:\n");
                                    inp = Console.ReadLine();
                                    int DId3;
                                    int.TryParse(inp, out DId3);
                                    try { bl.PickUpParcel(DId3); }
                                    catch (ParcelCantBePickedUPException) { Console.WriteLine("ERROR - sorry, we were unable to pick up the parcel\n"); }
                                    catch (DroneIdNotFoundException) { Console.WriteLine("ERROR - attemp to pick up a parcel by a non-exists drone\n"); }
                                    catch (ParcelIdNotFoundException) { Console.WriteLine("ERROR - attemp to pick up a non-exists parcel\n"); }
                                    break;
                                case Enums.NewUpdating.DeliveringPByD:
                                    Console.WriteLine("please enter drone's id:\n");
                                    inp = Console.ReadLine();
                                    int DId4;
                                    int.TryParse(inp, out DId4);
                                    try { bl.DeliveringParcelByDrone(DId4); }
                                    catch (ParcelCantBeDeliveredException) { Console.WriteLine("ERROR - soory, we were unable to deliver this parcel\n"); }
                                    catch (DroneIdNotFoundException) { Console.WriteLine("ERROR - attemp to deliver a parcel by a non-exists drone\n"); }
                                    catch (ParcelIdNotFoundException) { Console.WriteLine("ERROR - attemp to deliver a non-exists parcel\n"); }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Enums.Inputs.d:
                        Enums.Displaying d;
                        Console.WriteLine("What do you want to add?\n" +
                            "DBaseStation: display a base station\n" +
                            "DDrone: display a drone\n" +
                            "DCustomer: display a customer\n" +
                            "DParcel: display a parcel\n");
                        inp = Console.ReadLine();
                        Enums.Displaying.TryParse(inp, out d);
                        switch (d)
                        {
                            case Enums.Displaying.DBaseStation:
                                Console.WriteLine("please enter base station id:\n");
                                inp = Console.ReadLine();
                                int DBSId;//display base station id
                                int.TryParse(inp, out DBSId);
                                BaseStationForList bs;
                                try
                                { bs = bl.DisplayBaseStation(DBSId); }
                                catch (BaseStationNotFoundException) { Console.WriteLine("ERROR - there is no base station matching the id you entered"); break; }
                                Console.WriteLine($"id: {bs.BaseStationId}, name: {bs.StationName}, Available Charging Slots: {bs.AvailableChargingS}, Un Available Charging Slots: {bs.UnAvailableChargingS}")/*add more staff and add to string*/;
                                break;
                            case Enums.Displaying.DDrone:
                                Console.WriteLine("please enter drone id:\n");
                                inp = Console.ReadLine();
                                int DDId;//display drone id
                                int.TryParse(inp, out DDId);
                                DroneForList df;
                                try
                                { df = bl.DisplayDrone(DDId); }
                                catch (DroneIdNotFoundException) { Console.WriteLine("ERROR- there is no drone matching the id tou entered"); break; }
                                Console.WriteLine($"Id: {df.DroneId}, Model: {df.Model}, Battery: {df.Battery}, Location: {df.CurrentLocation}, Max weight: {df.MaxWeight}, Drone state: {df.DroneState}, Parcels in delivering: {df.InDeliveringParcelId}");
                                break;
                            case Enums.Displaying.DCustomer:
                                Console.WriteLine("please enter customer id:\n");
                                inp = Console.ReadLine();
                                int DCId;
                                int.TryParse(inp, out DCId);
                                Customer cf;
                                try
                                { cf = bl.DisplayCustomer(DCId); }
                                catch (CustomerNotFoundException) { Console.WriteLine("ERROE- there is no customer matching the id you entered"); break; }
                                Console.WriteLine($"id: {cf.CustomerId}, name: {cf.CustomerName}, phone: {cf.CustomerPhone}, parcels delivered from customer: {cf.ParcelsFromCustomer}, parcels delivered to customer: {cf.ParcelsToCustomer}, place: {cf.Place}");
                                break;
                            case Enums.Displaying.DParcel:
                                Console.WriteLine("please enter parcel id:\n");
                                inp = Console.ReadLine();
                                int DPId;
                                int.TryParse(inp, out DPId);//display parcel id
                                Parcel pc;
                                try
                                { pc = bl.DisplayParcel(DPId); }
                                catch (ParcelIdNotFoundException) { Console.WriteLine("ERROR- there is no parcel matching the id you rntered"); break; }
                                Console.WriteLine($"id: {pc.ParcelId}, weight category: {pc.ParcelWC}");///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                break;
                            default:
                                break;
                        }
                        break;
                    case Enums.Inputs.l:
                        Enums.ListsDisplaying l;
                        Console.WriteLine("Which list do you want to display?\n" +
                            "BaseStationsList: list of base stations\n" +
                            "DronesList: list of drones\n" +
                            "CustomersList: list of customers\n" +
                            "ParcelsList: list of parcels\n" +
                            "UnAscriptedParcelsList: list of unascripted parcels\n" +
                            "AvailableChargingStationsList: list of base stations with availables charge slots\n");
                        inp = Console.ReadLine();
                        Enums.ListsDisplaying.TryParse(inp, out l);
                        switch (l)
                        {
                            case Enums.ListsDisplaying.BaseStationsList:
                                foreach(var baseStation in bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId))//////////////not linq
                                {
                                    Console.WriteLine(baseStation);
                                }
                                break;
                            case Enums.ListsDisplaying.DronesList:
                                foreach(var drone in bl.DisplayDronesList(x => x.DroneId == x.DroneId))/////////not linq
                                {
                                    Console.WriteLine(drone);
                                }
                                break;
                            case Enums.ListsDisplaying.CustomersList:
                                foreach (var customer in bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId)) /////////not linq
                                {
                                    Console.WriteLine(customer);
                                }
                                break;
                            case Enums.ListsDisplaying.ParcelsList:
                                foreach (var parcel in bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId))//////////not linq
                                {
                                    Console.WriteLine(parcel);
                                }
                                break;
                            case Enums.ListsDisplaying.UnAscriptedParcelsLict:
                                foreach(var fParcel in bl.DisplayUnAscriptedParcelsList())//free parcel///////////not linq
                                {
                                    Console.WriteLine(fParcel);
                                }
                                break;
                            case Enums.ListsDisplaying.AvailableChargingStationsList:
                                foreach (var aBaseStation in bl.DisplayAvailableChargingStation())//available base stations/////////////not linq
                                {
                                    Console.WriteLine(aBaseStation);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Enums.Inputs.e:
                        Console.WriteLine("Bye!\n");
                        break;
                    default:
                        break;
                }
            } while (!(option == Enums.Inputs.e));
        }
    }
}

