using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DalApi
{
    public static class DalFactory
    {
        public static IDAL GetDal(string type)
        {
            switch (type)
            {
                case "1":
                    return new DalObject.DalObject();
                case "2":
                //return new DalXml();
                default:
                    throw new DalTypeCantBeProducedException();
            }
        }
        }
    }


