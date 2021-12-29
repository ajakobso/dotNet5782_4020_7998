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
        public MainWindow()
        {
            bl = BLFactory.GetBL("1");
            InitializeComponent();
        }
        private void DronesListButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(bl).Show();
        }
        private void BSListButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationsListWindow(bl).Show();
        }
        private void ParcelsListButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(bl).Show();
        }
        private void CustomersListButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(bl).Show();
        }
    }
}