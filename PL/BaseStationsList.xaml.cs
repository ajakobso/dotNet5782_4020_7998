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
        private PO.BaseStationForList BaseStation;//We use it to do the actions
        ObservableCollection<PO.BaseStationForList> baseStations;//list of base stations
        bool filterButtonIsClicked = false;//are the base stations filtered according to available charging stations?
        public BaseStationsListWindow(IBL bl)//opening base stations list window
        {
            InitializeComponent();
            this.bl = bl;
            BaseStation = new();
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId));//initializing baseStations to be the list of base stations
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;//show the list
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)//when we clicks twice on the list
        {
            new BaseStationWindow(bl, ((PO.BaseStationForList)BaseStationForListDataGrid.SelectedItem).BaseStationId).Show();//open the window according to the base station we click on
            //    DataGridCell cell = sender as DataGridCell;
            //    PO.DroneInCharge s = cell.DataContext as PO.DroneInCharge;
            //    if (cell.DataContext.ToString() != ""&&s!=null)
            //    { new DroneWindow(bl, s.DroneId).Show(); }
            //    else
            //        MessageBox.Show("there is no drone in charge at this station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
        private void Reset_Click(object sender, RoutedEventArgs e)//return to the normal state
        {
            filterButtonIsClicked = false;
            refreshWindow();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)//close
        {
            Close();
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)//filter by available stations
        {
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.AvailableChargingS > 0));//choosing only stations with available charging slots
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;//show the list of stations with available charging slots
            filterButtonIsClicked = true;
        }
        private void AddBaseStationWindowButton_Click(object sender, RoutedEventArgs e)//add base station
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
        private void UpdateBaseStationWindowButton_Click(object sender, RoutedEventArgs e)//update base station
        {
            new BaseStationWindow(bl, 0).Show();
            refreshWindow();
        }
        private void refreshWindow()//refresh the window 
        {
            baseStations = PO.BoPoAdapter.BaseStationForListAdapter(bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId));
            BaseStationForListDataGrid.ItemsSource = baseStations;
            BaseStationForListDataGrid.DataContext = baseStations;
            if (filterButtonIsClicked)
            { FilterButton_Click(FilterButton, null); }
        }
        private void group_Click(object sender, RoutedEventArgs e)//grup of stations with available charging slots?
        {
            List<IGrouping<int, PO.BaseStationForList>> GroupingData = baseStations.GroupBy(x => x.AvailableChargingS).ToList();
            baseStationGroupingDataGrid.DataContext = GroupingData;
            baseStationGroupingDataGrid.ItemsSource = GroupingData;
            BaseStationForListDataGrid.Visibility = Visibility.Hidden;
            baseStationGroupingDataGrid.Visibility = Visibility.Visible;
            group.Visibility = Visibility.Hidden;
            ungroup.Visibility = Visibility.Visible;
        }
        private void ungroup_Click(object sender, RoutedEventArgs e)//no group anymore
        {
            baseStationGroupingDataGrid.Visibility = Visibility.Hidden;
            BaseStationForListDataGrid.Visibility = Visibility.Visible;
            group.Visibility = Visibility.Visible;
            ungroup.Visibility = Visibility.Hidden;
        }
    }
}
