using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
namespace DAL.DalApi
{
    public static class DalFactory
    {
        public static IDAL GetDal(string type)
        {
            switch (type)
            {
                case "1":
                    return DalObject.DalObject.Instance;
                case "2":
                //return new DalXml();
                default:
                    throw new DalTypeCantBeProducedException();
            }
        }
        }
    }


