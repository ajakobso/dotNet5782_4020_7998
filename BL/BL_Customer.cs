using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {

        void AddCustomer(int Id, string Name, int PhoneNum, string Location);
        void UpdateCustomer(int Id, string Name, int PhoneNum);
        void DisplayCustomer(int id);
        void DisplayCustomersList();

    }
}
