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
        private int DelayTimer = 1000;
        private int Speed = 360; //360 kilometer per hour = 1 kilometer per second
        Simulator(BL bl, int droneId, Action updateDisplay, Func<bool> stopCheck)
        {
            while (!stopCheck())
            {
                //handle the drone acoording to its state
            }
        }
    }
}
