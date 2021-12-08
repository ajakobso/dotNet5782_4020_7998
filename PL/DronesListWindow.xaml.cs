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
        public DronesListWindow(Ibl bl)
        {
            InitializeComponent();
            this.bl = bl;
            DronesListView.ItemsSource = bl.DisplayDronesList(x => x.DroneId == x.DroneId);//predicate that always true to show all drones
            WeightSelector.ItemsSource = Enum.GetValues(typeof(Enums.WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(Enums.DroneStatuses));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
            if (WeightSelector.SelectedIndex > -1)//check if the second combo box selected
            {
                Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
                DronesListView.ItemsSource = status == Enums.DroneStatuses.All && weight == Enums.WeightCategories.All
                 ? bl.DisplayDronesList(x => x.DroneId == x.DroneId)
                 : status == Enums.DroneStatuses.All
                 ? bl.DisplayDronesList(x => x.DroneId == x.DroneId && x.MaxWeight == weight)
                 : weight == Enums.WeightCategories.All
                 ? bl.DisplayDronesList(x => x.DroneState == status && x.DroneId == x.DroneId)
                 : bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight);
            }
            else
            {
                DronesListView.ItemsSource = status == Enums.DroneStatuses.All
                 ? bl.DisplayDronesList(x => x.DroneId == x.DroneId)
                 : bl.DisplayDronesList(x => x.DroneState == status);
            }
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
            if (StatusSelector.SelectedIndex > -1)//check if the second combo box selected

            {
                Enums.DroneStatuses status = (Enums.DroneStatuses)StatusSelector.SelectedItem;
                DronesListView.ItemsSource = status == Enums.DroneStatuses.All && weight == Enums.WeightCategories.All
                 ? bl.DisplayDronesList(x => x.DroneId == x.DroneId)
                 : status == Enums.DroneStatuses.All
                 ? bl.DisplayDronesList(x => x.DroneId == x.DroneId && x.MaxWeight == weight)
                 : weight == Enums.WeightCategories.All
                 ? bl.DisplayDronesList(x => x.DroneState == status && x.DroneId == x.DroneId)
                 : bl.DisplayDronesList(x => x.DroneState == status && x.MaxWeight == weight);
            }
            else
            {
                DronesListView.ItemsSource = weight == Enums.WeightCategories.All
                    ? bl.DisplayDronesList(x => x.DroneId == x.DroneId)
                    : bl.DisplayDronesList(x => x.MaxWeight == weight);
            }
        }

        private void AddDroneWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }
    }
}
