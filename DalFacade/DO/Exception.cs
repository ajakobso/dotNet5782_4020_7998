using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class AddExistingBaseStationException : Exception
    {
        public AddExistingBaseStationException() { }

    }
    [Serializable]
    public class AddExistingCustomerException : Exception
    {
        public AddExistingCustomerException() { }

    }
    [Serializable]
    public class AddExistingDroneException : Exception
    {
        public AddExistingDroneException() { }

    }
    [Serializable]
    public class AddExistingUserException : Exception
    {
        public AddExistingUserException() { }

    }
    [Serializable]
    public class AddParcelToAnAsscriptedDroneException : Exception
    {
        public AddParcelToAnAsscriptedDroneException() { }

    }
    [Serializable]
    public class BaseStationNotFoundException : Exception
    {
        public BaseStationNotFoundException() { }
    }
    [Serializable]
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() { }
    }
    [Serializable]
    public class DroneIdNotFoundException : Exception
    {
        public DroneIdNotFoundException() { }
    }
    [Serializable]
    public class DroneOutOfBatteryException : Exception
    {
        public DroneOutOfBatteryException() { }

    }
    [Serializable]
    public class LocationOutOfRangeException : Exception
    {
        public LocationOutOfRangeException() { }
    }
    [Serializable]
    public class ParcelIdNotFoundException : Exception
    {
        public ParcelIdNotFoundException() { }
    }
    [Serializable]
    public class DalTypeCantBeProducedException : Exception
    {
        public DalTypeCantBeProducedException() { }
    }
}
