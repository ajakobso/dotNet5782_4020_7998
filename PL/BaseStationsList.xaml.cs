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
    /// Interaction logic for BaseStationsListWindow.xaml
    /// </summary>
    public partial class BaseStationsListWindow : Window
    {
        private readonly IBL bl;
        private BaseStationForList BaseStation;
        bool filterButtonIsClicked = false;
        public BaseStationsListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            BaseStation = new();
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
        }
        
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.AvailableChargingS > 0);
            filterButtonIsClicked = true;
        }
        private void AddBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl).ShowDialog();
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }
        
        private void RemoveBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _ = new BaseStationWindow(bl, true).ShowDialog();
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }
        private void UpdateBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _ = new BaseStationWindow(bl, 0).ShowDialog();
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }
    }
}
