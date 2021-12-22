using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL.BO;
using BL.BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        private readonly IBL bl;
        private CustomerForList customer;
        public CustomersListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            customer = new();
            CustomerForListDataGrid.ItemsSource = bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId);
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveCustomerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            CustomerForListDataGrid.ItemsSource = bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId);
        }
    }
}
