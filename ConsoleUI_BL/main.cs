using System;
using IBL.BO;
namespace ConsoleUI_BL
{
    class main
    {
        static void Main(string[] args)
        {
            IBL.BL bl = new IBL.BL();
            IDAL.DO.Coordinate coordinate;
            ConsoleUI.ConsoleUI.Inputs option;
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
                ConsoleUI.ConsoleUI.Inputs.TryParse(input, out option);
                switch (option)
                {
                    case ConsoleUI.ConsoleUI.Inputs.a:
                        Console.WriteLine("What do you want to add?\n" +
                        "nBaseStation: Add new base station\n" +
                        "nDrone: Add new drone\n" +
                        "nCustomer: Add new customer\n" +
                        "nParcel: Add new parcel\n");
                        ConsoleUI.ConsoleUI.Adding a;
                        string input1;
                        input1 = Console.ReadLine();
                        ConsoleUI.ConsoleUI.Adding.TryParse(input1, out a);
                        switch (a)
                        {
                            case ConsoleUI.ConsoleUI.Adding.nBaseStation:
                                Console.WriteLine("please enter:\n" + "base station's id:\n");
                                //string inp;
                                int BSId;
                                inp = Console.ReadLine();
                                int.TryParse(inp, out BSId);
                                Console.WriteLine("base station's name:\n");
                                string BSName;
                                BSName = Console.ReadLine();
                                Console.WriteLine("base station's longitude:\n");
                                inp = Console.ReadLine();
                                double BSLongitude;
                                double.TryParse(inp, out BSLongitude);
                                Console.WriteLine("base station's lattitude:\n");
                                inp = Console.ReadLine();
                                double BSLattitude;
                                double.TryParse(inp, out BSLattitude);
                                Console.WriteLine("base station's charge slots:\n");
                                inp = Console.ReadLine();
                                int BSChargeSlots;
                                int.TryParse(inp, out BSChargeSlots);
                                bl.AddBaseStation(BSId, BSName, BSLongitude, BSLattitude, BSChargeSlots);//create a new base station with new values
                                break;
                            case ConsoleUI.ConsoleUI.Adding.nDrone:
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
                                Enums.DroneStatuses bgGTDS;
                                Console.WriteLine("base station's number for first charging:");
                                inp = Console.ReadLine();
                                int StationForCharging;
                                int.TryParse(inp, out StationForCharging);
                                bl.AddDrone(DId, DWC, DModel, StationForCharging);//create a new drone with new values
                                break;
                            case ConsoleUI.ConsoleUI.Adding.nCustomer:
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
                                Location Clocation = new Location(CLongitude, CLattitude);
                                bl.AddCustomer(CId, CName, CPhone, Clocation);//adding new customer with new values
                                break;
                            case ConsoleUI.ConsoleUI.Adding.nParcel:
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
                    case ConsoleUI.ConsoleUI.Inputs.p:
                        Console.WriteLine("What do you want to update?\n" +
                            "DroneModel: Update drone's name\n" +
                            "BaseStation: Update a base station\n" +
                            "Customer: Update a customer\n" +
                            "DroneToAscriptionPToDCharge: Sending a drone to charge at a base station \n" +
                            "DroneRealese: Release drone from charging at a base station\n" +
                            ": Ascription of a parcel to a drone\n" +
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
                                Console.WriteLine("drone model:\n");
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
                                int CSNumber;
                                int.TryParse(inp, out CSNumber);
                                bl.UpdateBaseStation(BSId, BSName, CSNumber);
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
                                int CPNum;
                                int.TryParse(inp, out CPNum);
                                bl.UpdateCustomer(CId, CName, CPNum);
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
                                Console.WriteLine("how long the drone has been charging:\n");
                                inp = Console.ReadLine();
                                DateTime TICharging;//time in charging
                                DateTime.TryParse(inp, out TICharging);
                                bl.ReleaseDroneFromCharge(DId2, TICharging);
                                break;
                            case Enums.NewUpdating.AscriptionPToD:
                                Console.WriteLine("please enter drone's id:\n");
                                inp = Console.ReadLine();
                                int DId;
                                int.TryParse(inp, out DId);
                                bl.AscriptionParcelToDrone(DId);
                                break;
                            case Enums.NewUpdating.PickUpParcel:
                                Console.WriteLine("please enter drone's id:\n");
                                inp = Console.ReadLine();
                                int DId3;
                                int.TryParse(inp, out DId3);
                                bl.PickUpParcel(DId3);
                                break;
                            case Enums.NewUpdating.DeliveringPByD:
                                Console.WriteLine("please enter drone's id:\n");
                                inp = Console.ReadLine();
                                int DId4;
                                int.TryParse(inp, out DId4);
                                bl.DeliveringParcelByDrone(DId4);
                                break;
                            default:
                                break;
                        }
                        break;
                    case ConsoleUI.ConsoleUI.Inputs.d:
                        ConsoleUI.ConsoleUI.Displaying d;
                        Console.WriteLine("What do you want to add?\n" +
                            "DBaseStation: display a base station\n" +
                            "Ddrone: display a drone\n" +
                            "DCustomer: display a customer\n" +
                            "DParcel: display a parcel\n");
                        inp = Console.ReadLine();
                        Enums.Displaying.TryParse(inp, out d);
                        switch (d)
                        {
                            case ConsoleUI.ConsoleUI.Displaying.DBaseStation:
                                Console.WriteLine("please enter base station id:\n");
                                inp = Console.ReadLine();
                                int DBSId;//display base station id
                                int.TryParse(inp, out DBSId);
                                bl.DisplayBaseStation(DBSId);
                                break;
                            case ConsoleUI.ConsoleUI.Displaying.DDrone:
                                Console.WriteLine("please enter drone id:\n");
                                inp = Console.ReadLine();
                                int DDId;//display drone id
                                int.TryParse(inp, out DDId);
                                bl.DisplayDrone(DDId);
                                break;
                            case ConsoleUI.ConsoleUI.Displaying.DCustomer:
                                Console.WriteLine("please enter customer id:\n");
                                inp = Console.ReadLine();
                                int DCId;
                                int.TryParse(inp, out DCId);
                                bl.DisplayCustomer(DCId);
                                break;
                            case ConsoleUI.ConsoleUI.Displaying.DParcel:
                                Console.WriteLine("please enter parcel id:\n");
                                inp = Console.ReadLine();
                                int DPId;
                                int.TryParse(inp, out DPId);//display parcel id
                                bl.DisplayParcel(DPId);
                                break;
                            default:
                                break;
                        }
                    case ConsoleUI.ConsoleUI.Inputs.l:
                        ConsoleUI.ConsoleUI.ListsDisplaying l;
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
                            case ConsoleUI.ConsoleUI.ListsDisplaying.BaseStationsList:
                                bl.DisplayBaseStationsList();
                                break;
                            case ConsoleUI.ConsoleUI.ListsDisplaying.DronesList:
                                bl.DisplayDronesList();
                                break;
                            case ConsoleUI.ConsoleUI.ListsDisplaying.CustomersList:
                                bl.DisplayCustomersList();
                                break;
                            case ConsoleUI.ConsoleUI.ListsDisplaying.ParcelsList:
                                bl.DisplayParcelsList();
                                break;
                            case ConsoleUI.ConsoleUI.ListsDisplaying.UnAscriptedParcelsList:
                                bl.DisplayUnAscriptedParcelsList();
                                break;
                            case ConsoleUI.ConsoleUI.ListsDisplaying.AvailableChargingStationsList:
                                bl.DisplayAvailableChargingStation();
                                break;
                            default:
                                break;
                        }
                    case ConsoleUI.ConsoleUI.Inputs.e:
                        Console.WriteLine("Bye!\n");
                        break;
                    default:
                        break;

                }


            } while (!(option == ConsoleUI.ConsoleUI.Inputs.e));

        }
    }
}

