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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private readonly IBL bl;
        private DroneForList drone;//for action
        private int id;
        private string model;
        Enums.WeightCategories maxWeight;
        private int BsId;
        private bool IdTextBoxChanged, ModelTextBoxChanged;
        private DateTime In, Out;//in and out time of sending drone to charge
        #region add drone
        public DroneWindow(IBL bl)//add drone constractor
        {
            InitializeComponent();
            this.bl = bl;
            AddDroneGrid.Visibility = Visibility.Visible;//on this Grid i need to add this: Visibility="Hidden" after im done edit this
            _ = MessageBox.Show("in order to add a drone insert the\n required information in the designated places", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(Enums.WeightCategories));
            BsIdSelector.ItemsSource = bl.DisplayBaseStationsId();
        }
        private void DroneModelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = DroneModelTextBox.Text;
            model = input;
            ModelTextBoxChanged = true;
        }
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            if (BsIdSelector.SelectedIndex > -1 && DroneWeightSelector.SelectedIndex > -1 && ModelTextBoxChanged && IdTextBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this drone?", "Add Drone", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)//the exe callapse here//////////////////////////
                {
                    try { bl.AddDrone(id, model, maxWeight, BsId); }//add try and catch with the proper exceptions from the bl.exceptions
                    catch (LocationOutOfRangeException) { MessageBox.Show("the location of the base station tou choose is out of range,\n please choose different base station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                    MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
                    ModelTextBoxChanged = false;
                    IdTextBoxChanged = false;
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("please insert all the requested information,\nwithout it you cant add the drone!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void BsIdSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BsId = (int)BsIdSelector.SelectedItem;
        }
        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            maxWeight = (Enums.WeightCategories)DroneWeightSelector.SelectedItem;
        }
        private void DroneIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = DroneIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || id < 0)
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
            AddDroneGrid.Visibility = Visibility.Hidden;
            ActionsOnDroneGrid.Visibility = Visibility.Hidden;
            Close();
        }
        #endregion
        #region actions on drone
        public DroneWindow(IBL bl, int droneId)//actions on existing drone constractor
        {
            this.bl = bl;
            InitializeComponent();
            ActionsOnDroneGrid.Visibility = Visibility.Visible;
            try
            { drone = bl.DisplayDrone(droneId); }
            catch (DroneIdNotFoundException) { MessageBox.Show("sorry, this drone is not exist in our company yet!\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            DroneDataGrid.DataContext = drone;

        }
        private void DroneModelTBox_TextChanged(object sender, TextChangedEventArgs e)//working
        {
            drone.Model = DroneModelTBox.Text;
            ModelTextBoxChanged = true;
        }
        private void ModelUpdate_Click(object sender, RoutedEventArgs e)//working
        {
            try
            { bl.UpdateDrone(drone.DroneId, DroneModelTBox.Text); }//try catch
            catch(DroneIdNotFoundException) { MessageBox.Show( "sorry, this drone is not exist in our company yet!\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            SuccessOperation();
        }
        private void ChargeIn_Click(object sender, RoutedEventArgs e)
        {
            if (drone.DroneState == Enums.DroneStatuses.Available)
            {
                In = DateTime.Now;
                try
                { bl.DroneToCharge(drone.DroneId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (DroneOutOfBatteryException) { MessageBox.Show("this drone does not have enough battery to go to the closest charging station\n please call someone to take it", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                catch (BaseStationNotFoundException) { MessageBox.Show("look like there is no charging station around the drone\n please call someone to take it to charge", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                catch (NoChargingSlotIsAvailableException) { MessageBox.Show("there is no available charging stations anymore\n please call someone to take it to charge", "ERROE", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this drone can't be charged", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void ChargeOut_Click(object sender, RoutedEventArgs e)//overall its working except in BL_Drone line 166 at the base stations list in BL - not all BS in there - therefor it may not delete the drone in charge from the dic list in the bs in the list of bs
        {
            if (drone.DroneState == Enums.DroneStatuses.Maintenance)
            {
                Out = DateTime.Now;
                if (In == DateTime.MinValue)
                {
                    try { In = bl.GetInsertionTime(drone.DroneId); }
                    catch (BaseStationNotFoundException)
                    { MessageBox.Show("can't stop charging of drone that didnt inserted to charge", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); return; }//enter this error bc probably he didnt add the drone to the charge list
                    catch (DroneIdNotFoundException)
                    { MessageBox.Show("can't stop charging non-existing drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                }
                TimeSpan time = Out - In;
                double timeInCharge = time.TotalHours;
                try
                { bl.ReleaseDroneFromCharge(drone.DroneId, timeInCharge); }
                catch (DroneIdNotFoundException) { MessageBox.Show("this drone is not exist\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (BaseStationNotFoundException) { MessageBox.Show("this base station is not exist\n please choose enother base station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (AddExistingBaseStationException) { MessageBox.Show("something went wrong\n please try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();

            }
            else
            {
                MessageBox.Show("this drone is not in maintenance\n and can't exit charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void PickUpParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel parcel = new();
            try
            { parcel = bl.DisplayParcel(drone.InDeliveringParcelId); }
            catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            if (parcel.ParcelAscriptionTime != null && parcel.ParcelPickUpTime == null && drone.DroneState == Enums.DroneStatuses.Shipping)
            {
                try
                { bl.PickUpParcel(drone.DroneId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch(DroneIdNotFoundException) { MessageBox.Show("this drone is not exist\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch(ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch(ParcelCantBePickedUPException) { MessageBox.Show("this parcel can not be picked up\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this drone can't pick up this parcl", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void DeliverParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel parcel = new();
            try
            { parcel = bl.DisplayParcel(drone.InDeliveringParcelId); }
            catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            if (parcel.ParcelAscriptionTime != null && parcel.ParcelPickUpTime != null && parcel.ParcelDeliveringTime == null && drone.DroneState == Enums.DroneStatuses.Shipping)
            {
                try
                { bl.DeliveringParcelByDrone(drone.DroneId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (DroneIdNotFoundException) { MessageBox.Show("this drone is not exist\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch(ParcelCantBeDeliveredException) { MessageBox.Show("this parcel can not be delivered\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this drone can't deliver this parcl", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void SendDrone_Click(object sender, RoutedEventArgs e)
        {
            if (drone.DroneState == Enums.DroneStatuses.Available)
            {
                try
                { bl.AscriptionParcelToDrone(drone.DroneId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (DroneIdNotFoundException) { MessageBox.Show("this drone is not exist\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (NoParcelAscriptedToDroneException) { MessageBox.Show("no parcel could be ascripted to the drone\n please choose another drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this drone can't be sent to delivery", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        #endregion
        private void SuccessOperation()
        {
            MessageBoxResult result = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                { DroneDataGrid.ItemsSource = bl.DisplayDrone(drone.DroneId).ToString(); }
                catch (DroneIdNotFoundException) { MessageBox.Show("this drone is not exist\n please choose enother drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            }
        }
    }
}