﻿using System;
using IBL.BO;
namespace ConsoleUI_BL
{
    class main
    {
        static void Main(string[] args)
        {
            BL bl = new BL();
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
                                Enums.DroneStatuses DS;
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
                                bl.AddCustomer(CId, CName, CPhone, CLongitude, CLattitude);//adding new customer with new values
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
                                bl.AddParcel( PSId, PDeId, PWC, PPriority );//create a new parcel with new values
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
                            "DroneToCharge: Sending a drone to charge at a base station \n" +
                            "DroneRealese: Release drone from charging at a base station\n"+
                            "AscriptionPToD: Ascription of a parcel to a drone\n"+
                            "PickUpParcel: picking up parcel by a drone\n"+
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
                                inp=Console.ReadLine();
                                int BSId;
                                int.TryParse(inp, out BSId);
                                Console.WriteLine("new name for the base station(optional, else enter' '):\n");
                                string BSName = Console.ReadLine();
                                Console.WriteLine("new number of charge slots(optional, else enter ' '):\n");
                                string CSNumber = Console.ReadLine();
                                bl.UpdateBaseStation(BSId, BSName, CSNumber)
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


                }
            }
        }
    }
}