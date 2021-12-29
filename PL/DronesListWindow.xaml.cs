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
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private readonly IBL bl;
        private Drone drone;
        //static readonly DependencyProperty DronesListProperty = DependencyProperty.Register("Drones List", typeof(ObservableCollection<DroneForList>), typeof(DronesListWindow));
        public ObservableCollection<PO.DroneForList> dronesList; /*{ get => (ObservableCollection<PO.DroneForList>)GetValue(DronesListProperty); set => SetValue(DronesListProperty, value); }*/
        public DronesListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            drone = new();
            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneId == x.DroneId));
            DroneForListDataGrid.DataContext = dronesList;
            DroneForListDataGrid.ItemsSource = dronesList;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(Enums.WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(Enums.DroneStatuses));
        }
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            PO.ParcelToList s = cell.DataContext as PO.ParcelToList;
            if ((string)cell.DataContext != "0")
            { new ParcelWindow(bl, s.ParcelId).ShowDialog(); }
            else
                MessageBox.Show("there is no parcel ascripted to the drone,\n cant open parcel's window.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
        public void RefreshDronesListWindow()
        {
            new DronesListWindow(bl);
            //int weightSelectoeSelectedIndex = WeightSelector.SelectedIndex;
            //int statusSelectorSelectedIndex = StatusSelector.SelectedIndex;
            if (WeightSelector != null || StatusSelector != null)
            //if (weightSelectoeSelectedIndex > -1 || statusSelectorSelectedIndex > -1) 
            {
                if (WeightSelector != null && StatusSelector != null)
                //if (WeightSelector.SelectedIndex > -1 && StatusSelector.SelectedIndex > -1)
                {
                    Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                    Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight));
                    DroneForListDataGrid.DataContext = dronesList;
                    DroneForListDataGrid.ItemsSource = dronesList;
                }
                else
                {
                    if (WeightSelector != null)
                    //if (WeightSelector.SelectedIndex > -1)
                    {
                        Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                        dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.MaxWeight == weight));
                        DroneForListDataGrid.DataContext = dronesList;
                        DroneForListDataGrid.ItemsSource = dronesList;
                    }
                    else
                    {
                        if (StatusSelector.SelectedIndex > -1)
                        {
                            Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status));
                            DroneForListDataGrid.DataContext = dronesList;
                            DroneForListDataGrid.ItemsSource = dronesList;
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("enjoy in your next action!\n", "Goodluck", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            }

        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                if (WeightSelector.SelectedIndex > -1)//check if the second combo box selected
                {
                    Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight));
                    DroneForListDataGrid.DataContext = dronesList;
                    DroneForListDataGrid.ItemsSource = dronesList;
                }
                else
                {
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status));
                    DroneForListDataGrid.DataContext = dronesList;
                    DroneForListDataGrid.ItemsSource = dronesList;
                }
            }
        }
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                if (StatusSelector.SelectedIndex > -1 && StatusSelector.SelectedItem != null)//check if the second combo box selected
                {
                    Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight));
                    DroneForListDataGrid.DataContext = dronesList;
                    DroneForListDataGrid.ItemsSource = dronesList;
                }
                else
                {
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.MaxWeight == weight));
                    DroneForListDataGrid.DataContext = dronesList;
                    DroneForListDataGrid.ItemsSource = dronesList;
                }
            }
        }
        private void AddDroneWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).ShowDialog();
            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneId == x.DroneId));
            DroneForListDataGrid.ItemsSource = dronesList;
            WeightSelector_SelectionChanged(WeightSelector, null);
            StatusSelector_SelectionChanged(StatusSelector, null);
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneId == x.DroneId));
            DroneForListDataGrid.ItemsSource = dronesList;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DroneForListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            drone.DroneId = ((PO.DroneForList)DroneForListDataGrid.SelectedItem).DroneId;//meanwhile until i figure out how to get the drone id in the row clicked
            new DroneWindow(bl, drone.DroneId).Show();
        }
    }
}
