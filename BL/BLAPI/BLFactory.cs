using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BL
{ 
    namespace BlApi
    {
        public static class BLFactory
        {
            public static IBL GetBL(string type)
            {
                switch (type)
                {
                    case "1":
                        return BL.Instance;
                    case "2":
                    //return new BLImp2();
                    default:
                        return BL.Instance;
                }
            }
        }
    }
}
