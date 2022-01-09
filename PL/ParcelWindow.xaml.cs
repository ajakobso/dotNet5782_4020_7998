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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private readonly IBL bl;
        private PO.Parcel Parcel;//for action
        int SID, DID;//variables for binding - add parcel
        PO.Enums.WeightCategories weight;//variable for binding - add parcel
        PO.Enums.Priorities priority;//variable for binding - add parcel
        private bool SIDTextBoxChanged, DIDTextBoxChanged;
        DateTime In;
        #region add Parcel
        public ParcelWindow(IBL bl)//add Parcel constractor
        {
            InitializeComponent();
            this.bl = bl;
            AddParcelGrid.Visibility = Visibility.Visible;//on this Grid i need to add this: Visibility="Hidden" after im done edit this
            _ = MessageBox.Show("in order to add a Parcel insert the\n required information in the designated places", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(PO.Enums.WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(PO.Enums.Priorities));
        }
        private void IdParcelSTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SIDTextBoxChanged = true;
        }
        private void IdParcelDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DIDTextBoxChanged = true;
        }
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ParcelWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrioritySelector.SelectedIndex > -1 && ParcelWeightSelector.SelectedIndex > -1 && SIDTextBoxChanged && DIDTextBoxChanged)//sender id, destination id
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this Parcel?", "Add Parcel", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    bl.AddParcelToDeliver(SID, DID, (Enums.WeightCategories)weight, (Enums.Priorities)priority);
                    MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
                    SIDTextBoxChanged = false;
                    DIDTextBoxChanged = false;
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("please insert all the requested information,\nwithout it you cant add the Parcel!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddParcelGrid.Visibility = Visibility.Hidden;
            ActionsOnParcelGrid.Visibility = Visibility.Hidden;
            Close();
        }
        #endregion
        #region actions on Parcel
        public ParcelWindow(IBL bl, int ParcelId)//actions on existing Parcel constractor
        {
            this.bl = bl;
            InitializeComponent();
            ActionsOnParcelGrid.Visibility = Visibility.Visible;
            try
            { Parcel = PO.BoPoAdapter.ParcelBoPo(bl.DisplayParcel(ParcelId)); }
            catch (ParcelIdNotFoundException) { MessageBox.Show("sorry, this Parcel is not exist in our company yet!\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            ParcelDataGrid.DataContext = Parcel;
            IEnumerable<PO.Parcel> l = new List<PO.Parcel>();
            l.Append(Parcel);
            ParcelDataGrid.ItemsSource = l;
            //if the drone id then make the grid of the drone in charge to be invisible
        }
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            PO.CustomerInParcel s = Parcel.SCIParcel;
            new CustomerWindow(bl, s.CustomerId).ShowDialog();
            PO.CustomerInParcel d = Parcel.DCIParcel;
            new CustomerWindow(bl, d.CustomerId).ShowDialog();
            PO.DroneInParcel drone = Parcel.DInParcel;
            new CustomerWindow(bl, drone.DroneId).ShowDialog();
            // else
            //   MessageBox.Show("there is no available sender", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
        //private void ParcelModelTBox_TextChanged(object sender, TextChangedEventArgs e)//what is this?
        //{
        //    Parcel.Model = ParcelModelTBox.Text;
        //    ModelTextBoxChanged = true;
        //}
        //private void ModelUpdate_Click(object sender, RoutedEventArgs e)//working
        //{
        //    try
        //    { bl.UpdateParcel(Parcel.ParcelId, ParcelModelTBox.Text); }//try catch
        //    catch (ParcelIdNotFoundException) { MessageBox.Show("sorry, this Parcel is not exist in our company yet!\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        //    SuccessOperation();
        //}
        //private void ChargeIn_Click(object sender, RoutedEventArgs e)//what is this?
        //{
        //    if (Parcel.ParcelState == PO.Enums.ParcelState.Ascripted)/////////gues so?
        //    {
        //        In = DateTime.Now;
        //        try
        //        { bl.ParcelToCharge(Parcel.ParcelId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
        //        catch (ParcelOutOfBatteryException) { MessageBox.Show("this Parcel does not have enough battery to go to the closest charging station\n please call someone to take it", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        //        catch (BaseStationNotFoundException) { MessageBox.Show("look like there is no charging station around the Parcel\n please call someone to take it to charge", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        //        catch (NoChargingSlotIsAvailableException) { MessageBox.Show("there is no available charging stations anymore\n please call someone to take it to charge", "ERROE", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        //        SuccessOperation();
        //    }
        //    else
        //    {
        //        MessageBox.Show("this Parcel can't be charged", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        //    }
        //}
        //private void ChargeOut_Click(object sender, RoutedEventArgs e)//what is this?
        // {

        // }
        private void PickUpParcel_Click(object sender, RoutedEventArgs e)//the drone is taking our parcel
        {
            if (Parcel.ParcelAscriptionTime != null && Parcel.ParcelPickUpTime == null)//deliver or pick up?
            {
                try
                { bl.PickUpParcel(Parcel.ParcelId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (ParcelIdNotFoundException) { MessageBox.Show("this Parcel is not exist\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
               // catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (ParcelCantBePickedUPException) { MessageBox.Show("this parcel can not be picked up\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this Parcel can't pick up this parcl", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void DeliverParcel_Click(object sender, RoutedEventArgs e)//parcel has delivered
        {
            Parcel parcel = new();
            try
            { parcel = bl.DisplayParcel(Parcel.ParcelId); }//Parcel.InDeliveringParcelId
            catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            if (parcel.ParcelAscriptionTime != null && parcel.ParcelPickUpTime != null && parcel.ParcelDeliveringTime == null)
            {
                try
                { bl.DeliveringParcelByDrone(Parcel.ParcelId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (ParcelIdNotFoundException) { MessageBox.Show("this Parcel is not exist\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
               // catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (ParcelCantBeDeliveredException) { MessageBox.Show("this parcel can not be delivered\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this Parcel can't deliver this parcl", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void SendParcel_Click(object sender, RoutedEventArgs e)//Asctipted parcel to drone for delivery
        {
            if (Parcel.ParcelCreationTime != null)
            {
                try
                { bl.AscriptionParcelToDrone(Parcel.ParcelId); }//we to do this in try and catch and catch any exception that might be thrown from bl.
                catch (ParcelIdNotFoundException) { MessageBox.Show("this Parcel is not exist\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
               // catch (ParcelIdNotFoundException) { MessageBox.Show("this parcel is not exist\n please choose enother parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                catch (NoParcelAscriptedToDroneException) { MessageBox.Show("no parcel could be ascripted to the Parcel\n please choose another Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); return; }
                SuccessOperation();
            }
            else
            {
                MessageBox.Show("this Parcel can't be sent to delivery", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        #endregion
        private void SuccessOperation()
        {
            MessageBoxResult result = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                { ParcelDataGrid.ItemsSource = bl.DisplayParcel(Parcel.ParcelId).ToString(); }
                catch (BO.ParcelIdNotFoundException) { MessageBox.Show("this Parcel is not exist\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            }
        }
    }
}

