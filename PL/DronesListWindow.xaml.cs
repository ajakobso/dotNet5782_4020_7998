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
        private List<int> IdOfOpenedDroneWindow = new();
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
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)//filter the drones by their statuses
        {
            if (StatusSelector.SelectedItem != null)
            {
                Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                if (WeightSelector.SelectedIndex > -1)//check if the second combo box selected
                {
                    Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight));
                }
                else
                {
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status));
                }
            }
        }
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)//ilter the drones by their wheight
        {
            if (WeightSelector.SelectedItem != null)
            {
                Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                if (StatusSelector.SelectedIndex > -1 && StatusSelector.SelectedItem != null)//check if the second combo box selected
                {
                    Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight));
                }
                else
                {
                    dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.MaxWeight == weight));
                }
            }
        }
        private void AddDroneWindowButton_Click(object sender, RoutedEventArgs e)//add 
        {
            new DroneWindow(bl).ShowDialog();
            refreshWindow();
        }
        private void refreshWindow()//refresh
        {
            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneId == x.DroneId));
            DroneForListDataGrid.DataContext = dronesList;
            DroneForListDataGrid.ItemsSource = dronesList;
            WeightSelector_SelectionChanged(WeightSelector, null);
            StatusSelector_SelectionChanged(StatusSelector, null);
        }
        private void Reset_Click(object sender, RoutedEventArgs e)//reset
        {
            dronesList = PO.BoPoAdapter.DroneForListAdapter(bl.DisplayDronesList(x => x.DroneId == x.DroneId));
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)//close
        {
            Close();
        }
        private void DroneForListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            drone.DroneId = ((PO.DroneForList)DroneForListDataGrid.SelectedItem).DroneId;
            new DroneWindow(bl, drone.DroneId, refreshWindow).Show();
        }
    }
}
