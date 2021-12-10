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
        private readonly Ibl bl;
        private IBL.BO.DroneForList drone;//for action
        private int id;
        private int BsId;
        private bool IdTextBoxChanged, ModelTextBoxChanged;
        private DateTime In, Out;//in and out time of sending drone to charge

        public DroneWindow(Ibl bl)//add drone constractor
        {
            InitializeComponent();
            this.bl = bl;
            AddDroneGrid.Visibility = Visibility.Visible;//on this Grid i need to add this: Visibility="Hidden" after im done edit this
            _ = MessageBox.Show("in order to add a drone insert the\n required information in the designated places", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enums.WeightCategories));
            BsIdSelector.ItemsSource = bl.DisplayBaseStationsId();
        }
        public DroneWindow(Ibl bl, int droneId)//actions on existing drone constractor
        {
            this.bl = bl;
            InitializeComponent();
            ActionsOnDroneGrid.Visibility = Visibility.Visible;
            drone = bl.DisplayDrone(droneId);
            DroneView.ItemsSource = bl.DisplayDrone(droneId).ToString();
            
        }

        private void DroneModelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            drone.Model = DroneModelTextBox.Text;
            ModelTextBoxChanged = true;
        }
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            if (BsIdSelector.SelectedIndex > -1 && DroneWeightSelector.SelectedIndex > -1 && ModelTextBoxChanged && IdTextBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this drone?", "Add Drone", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)//the exe callapse here//////////////////////////
                {
                    try { bl.AddDrone(id, drone.Model, drone.MaxWeight, BsId); }//add try and catch with the proper exceptions from the bl.exceptions
                    catch (IBL.BO.LocationOutOfRangeException) { MessageBox.Show("the location of the base station tou choose is out of range,\n please choose different base station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                    ModelTextBoxChanged = false;
                    IdTextBoxChanged = false;
                    new DronesListWindow(bl, true);
                    Close();
                }
            }
        }

        private void BsIdSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BsId = (int)BsIdSelector.SelectedItem;
        }

        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drone.MaxWeight = (IBL.BO.Enums.WeightCategories)DroneWeightSelector.SelectedItem;
        }

        private void DroneIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = DroneIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || drone.DroneId < 0)
            {
                DroneIdTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    DroneIdTextBox.Foreground = Brushes.Black;
                    IdTextBoxChanged = true;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DroneModelTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            drone.Model = DroneModelTextBox.Text;
            ModelTextBoxChanged = true;
        }

        private void ModelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (drone.DroneState == IBL.BO.Enums.DroneStatuses.Available)
            {
                bl.UpdateDrone(drone.DroneId, drone.Model);//try catch
                SuccessOperation();
            }//we to do this in try and catch and catch any exception that might be thrown from bl.
            
        }

        private void ChargeIn_Click(object sender, RoutedEventArgs e)
        {
            if(drone.DroneState == IBL.BO.Enums.DroneStatuses.Available)
            {
                In = DateTime.Now;
                bl.DroneToCharge(drone.DroneId);//we to do this in try and catch and catch any exception that might be thrown from bl.
                
            }
        }

        
        private void PickUpParcel_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.Parcel parcel = bl.DisplayParcel(drone.InDeliveringParcelId);
            if(parcel.ParcelAscriptionTime != null && parcel.ParcelPickUpTime==null && drone.DroneState==IBL.BO.Enums.DroneStatuses.Shipping)
            {
                bl.PickUpParcel(drone.DroneId);//we to do this in try and catch and catch any exception that might be thrown from bl.
            }
        }

        private void DeliverParcel_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.Parcel parcel = bl.DisplayParcel(drone.InDeliveringParcelId);
            if (parcel.ParcelAscriptionTime != null && parcel.ParcelPickUpTime != null && parcel.ParcelDeliveringTime == null && drone.DroneState == IBL.BO.Enums.DroneStatuses.Shipping)
            {
                bl.DeliveringParcelByDrone(drone.DroneId);//we to do this in try and catch and catch any exception that might be thrown from bl.
            }
        }

        private void SendDrone_Click(object sender, RoutedEventArgs e)//should be a refernce i done know why there is not i think it could cause an error
        {
            bl.AscriptionParcelToDrone(drone.DroneId);//we to do this in try and catch and catch any exception that might be thrown from bl.
        }

        private void ChargeOut_Click(object sender, RoutedEventArgs e)
        {
            if (drone.DroneState == IBL.BO.Enums.DroneStatuses.Maintenance)
            {
                Out = DateTime.Now;
                TimeSpan time = In - Out;
                double timeInCharge = time.TotalHours;
                bl.ReleaseDroneFromCharge(drone.DroneId, timeInCharge);//we to do this in try and catch and catch any exception that might be thrown from bl.
            }
        }
        private void SuccessOperation()
        {
            MessageBoxResult result = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                DroneView.ItemsSource = bl.DisplayDrone(drone.DroneId).ToString();
            }
        }
        private void FailOperation()
        {
            _ = MessageBox.Show("operation faild", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
