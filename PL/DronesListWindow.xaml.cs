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
using IBL.BO;
using IBL;
namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private readonly Ibl bl;
        private Drone drone;
        public DronesListWindow(Ibl bl)
        {
            InitializeComponent();
            this.bl = bl;
            drone = new();
            DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneId == x.DroneId);//predicate that always true to show all drones
            WeightSelector.ItemsSource = Enum.GetValues(typeof(Enums.WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(Enums.DroneStatuses));
        }
        public DronesListWindow(Ibl bl, bool check)
        { 
            if (check)
            {
                new DronesListWindow(bl);
                if(WeightSelector.SelectedIndex > -1 || StatusSelector.SelectedIndex > -1)
                {
                    if (WeightSelector.SelectedIndex > -1 && StatusSelector.SelectedIndex > -1)
                    {
                        Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                        Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                        DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight);
                    }
                    else
                    {
                        if (WeightSelector.SelectedIndex > -1)
                        {
                            Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                            DronesListView.ItemsSource = bl.DisplayDronesList(x => x.MaxWeight == weight);
                        }
                        else
                        {
                            if (StatusSelector.SelectedIndex > -1)
                            {
                                Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                                DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneState == status);
                            }
                        }
                    }
                    
                }
            }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
            if (WeightSelector.SelectedIndex > -1)//check if the second combo box selected
            {
                Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight);
            }
            else
            {
                DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneState == status);
            }
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
            if (StatusSelector.SelectedIndex > -1)//check if the second combo box selected

            {
                Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight);
            }
            else
            {
                DronesListView.ItemsSource = bl.DisplayDronesList(x => x.MaxWeight == weight);
            }
        }

        private void AddDroneWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneId == x.DroneId);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
       
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            drone.DroneId = 0;//meanwhile until i figure out how to get the drone id in the row clicked
            new DroneWindow(bl, drone.DroneId).Show();
        }
    }
}
