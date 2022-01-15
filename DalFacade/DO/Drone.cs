using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        //public DroneStatuses Status { get; set; }
        //public double Battery { get; set; }
        public override string ToString()
        {
            return $"id = {Id}, model = {Model}, max weight = {MaxWeight} ";///*status = {Status} , battery = {Battery}
        }
    }

}
