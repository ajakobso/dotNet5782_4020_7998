﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
namespace DO
{
    public struct BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChargeSlots { get; set; }
        public int AvailableChargeSlots { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public override string ToString()
        {

            Coordinate sexaLongitude = new();
            sexaLongitude = sexaLongitude.Fromdouble(Longitude);///convert from decimal to sexagesimal
            string strLongitude = new string(sexaLongitude.ToString("NS"));///create the string form of the sexagesimal longitude 
            Coordinate sexaLattitude = new();
            sexaLattitude = sexaLattitude.Fromdouble(Lattitude);///convert from decimal to sexagesimal
            string strLattitude = new string(sexaLattitude.ToString("WE"));///create the string form of the sexagesimal lattitude
            return $"id = {Id}, name = {Name}, charge slot = {ChargeSlots}, longitude = {strLongitude}, lattitude = {strLattitude}";
        }
    }
}