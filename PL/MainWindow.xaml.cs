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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBL bl;
        public MainWindow()//starting
        {
            bl = BLFactory.GetBL("1");
            InitializeComponent();
        }
        private void DronesListButton_Click(object sender, RoutedEventArgs e)//when we click on the "drones" button, open drones list window
        {
            new DronesListWindow(bl).Show();
        }
        private void BSListButton_Click(object sender, RoutedEventArgs e)//when we click on the "base stations" button, open base stations list window
        {
            new BaseStationsListWindow(bl).Show();
        }
        private void ParcelsListButton_Click(object sender, RoutedEventArgs e)//when we click on the "parcels" button, open parcels list window
        {
            new ParcelsListWindow(bl).Show();
        }
        private void CustomersListButton_Click(object sender, RoutedEventArgs e)//when we click on the "customers" button, open customers list window
        {
            new CustomersListWindow(bl).Show();
        }
    }
}