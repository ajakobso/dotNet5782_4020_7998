﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInCharge
    {
        public int DroneId { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return $"id = {DroneId}, battery = {Battery}";
        }
    }
}
