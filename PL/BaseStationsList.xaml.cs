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
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationsListWindow.xaml
    /// </summary>
    public partial class BaseStationsListWindow : Window
    {
        private readonly IBL bl;
        private PO.BaseStationForList BaseStation;
        ObservableCollection<PO.BaseStationForList> baseStations;
        bool filterButtonIsClicked = false;
        public BaseStationsListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            BaseStation = new();
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId));
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new BaseStationWindow(bl, ((PO.BaseStationForList)BaseStationForListDataGrid.SelectedItem).BaseStationId).Show();
            //    DataGridCell cell = sender as DataGridCell;
            //    PO.DroneInCharge s = cell.DataContext as PO.DroneInCharge;
            //    if (cell.DataContext.ToString() != ""&&s!=null)
            //    { new DroneWindow(bl, s.DroneId).Show(); }
            //    else
            //        MessageBox.Show("there is no drone in charge at this station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            filterButtonIsClicked = false;
            refreshWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.AvailableChargingS > 0));
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;
            filterButtonIsClicked = true;
        }
        private void AddBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl).ShowDialog();
            BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }
        
        //private void RemoveBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _ = new BaseStationWindow(bl, true).Show();
        //    BaseStationForListDataGrid.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
        //    if (filterButtonIsClicked)
        //    { FilterButton_Click(FilterButton, null); }
        //}
        private void UpdateBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl, 0).Show();
            refreshWindow();
        }
        private void refreshWindow()
        {
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId));
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }

        private void group_Click(object sender, RoutedEventArgs e)
        {
            List<IGrouping<int, PO.BaseStationForList>> GroupingData = baseStations.GroupBy(x => x.AvailableChargingS).ToList();
            baseStationGroupingDataGrid.DataContext = GroupingData;
            baseStationGroupingDataGrid.ItemsSource = GroupingData;
            BaseStationForListDataGrid.Visibility = Visibility.Hidden;
            baseStationGroupingDataGrid.Visibility = Visibility.Visible;
            group.Visibility = Visibility.Hidden;
            ungroup.Visibility = Visibility.Visible;
        }

        private void ungroup_Click(object sender, RoutedEventArgs e)
        {
            baseStationGroupingDataGrid.Visibility = Visibility.Hidden;
            BaseStationForListDataGrid.Visibility = Visibility.Visible;
            group.Visibility = Visibility.Visible;
            ungroup.Visibility = Visibility.Hidden;
        }
    }
}
