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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public DroneWindow(Ibl bl)//add drone constractor
        {
            InitializeComponent();
        }
        public DroneWindow(Ibl bl, int droneId)//actions on existing drone constractor
        {
            InitializeComponent();
        }
    }
}
