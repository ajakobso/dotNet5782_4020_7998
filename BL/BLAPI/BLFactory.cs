using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
namespace BlApi
{
    public static class BLFactory
    {
        public static IBL GetBL(string type)
        {
            switch (type)
            {
                case "1":
                    return BL.BL.GetInstance();
                case "2":
                //return new BLImp2();//dalxml
                default:
                    return BL.BL.GetInstance();
            }
        }
    }
}

