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
using IBL;
namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private Ibl bl;
        public DronesListWindow(Ibl bl)
        {
            InitializeComponent();
            this.bl = bl;
            DronesListView.ItemsSource = bl.DisplayDronesList();
            //new DroneWindow(this.bl).Show();
            this.Close();

        }

    }
}
