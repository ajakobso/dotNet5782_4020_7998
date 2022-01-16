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
        private Parcel parcel;//for action
        int SID, DID;//variables for binding - add parcel
        PO.Enums.WeightCategories weight;//variable for binding - add parcel
        PO.Enums.Priorities priority;//variable for binding - add parcel
        private bool SIDTextBoxChanged, DIDTextBoxChanged;
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
            { parcel = bl.DisplayParcel(ParcelId); }
            catch (ParcelIdNotFoundException) { MessageBox.Show("sorry, this Parcel is not exist in our company yet!\n please choose enother Parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            DataContext = parcel;
            List<Parcel> l = new List<Parcel>();
            l.Add(parcel);//it didnt work without to list, i didnt found another solution here
            ParcelDataGrid.ItemsSource = l;
            //if the drone id then make the grid of the drone in charge to be invisible
        }

        private void OpenSenderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (parcel.SCIParcel.CustomerId > 0)//check if the customer exists
            { new CustomerWindow(bl, parcel.SCIParcel.CustomerId).Show(); }
            else
            { MessageBox.Show("sorry, we couldnt open the data of this customer!\n please choose enother one", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        }

        private void OpenReceiverWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (parcel.DCIParcel.CustomerId > 0)//check if the customer exists
            { new CustomerWindow(bl, parcel.DCIParcel.CustomerId).Show(); }
            else
            { MessageBox.Show("sorry, we couldnt open the data of this customer!\n please choose enother one", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        }

        private void OpenDroneWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (parcel.DInParcel.DroneId > 0)//there is a drone that deliver the parcel
            { new DroneWindow(bl, parcel.DInParcel.DroneId, null).Show(); }
            else
            { MessageBox.Show("sorry, we couldnt open the data of this drone!\n please choose enother one", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
        }        
        #endregion
    }
}

