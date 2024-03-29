﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneForList
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories MaxWeight { get; set; }
        public Enums.DroneStatuses DroneState { get; set; }
        public double Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public int InDeliveringParcelId { get; set; }//of course just in case there is a parcel in delivering
        public override string ToString()
        {
            return $"id = {DroneId}, model = {Model}, max weight = {MaxWeight}, status = {DroneState}, battery = {Battery}, location = {CurrentLocation},parcel id = {InDeliveringParcelId}";
        }

    }
}
