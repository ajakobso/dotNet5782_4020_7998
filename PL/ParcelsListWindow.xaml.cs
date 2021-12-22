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
    /// Interaction logic for ParcelsList.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private readonly IBL bl;
        private Parcel parcel;
        public ParcelsListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            parcel = new();
            ParcelForListDataGrid.ItemsSource = bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId);
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ParcelForListDataGrid.ItemsSource = bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
