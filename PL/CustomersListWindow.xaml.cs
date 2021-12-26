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
using System.Collections.ObjectModel;
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
        static readonly DependencyProperty CustomerListProperty = DependencyProperty.Register("Customer List", typeof(ObservableCollection<CustomerForList>), typeof(CustomersListWindow));
        public ObservableCollection<CustomerForList> CustomersList { get => (ObservableCollection<CustomerForList>)GetValue(CustomerListProperty); set => SetValue(CustomerListProperty, value); }
        public CustomersListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            customer = new();
            CustomerForListDataGrid.DataContext = CustomersList;
            CustomersList = (bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId) as ObservableCollection<CustomerForList>);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource customerForListViewSource = (CollectionViewSource)FindResource("customerForListViewSource");
        }
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)//work
        {
                new CustomerWindow(bl).ShowDialog();
                CustomerForListDataGrid.ItemsSource = bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId);            
        }
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
          //  new CustomerWindow(bl).ShowDialog();
          new CustomerWindow.CustomerWindow()
            CustomerForListDataGrid.ItemsSource = bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId);
        }
        public void RefreshCustomerButton_Click()
        {
            new CustomersListWindow(bl);
            MessageBox.Show("enjoy in your next action!\n", "Goodluck", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)//work
        {
            Close();
        }
        private void Reset_Click(object sender, RoutedEventArgs e)//work
        {
            CustomerForListDataGrid.ItemsSource = bl.DisplayCustomersList(x => x.CustomerId == x.CustomerId);
        }
    }
}
