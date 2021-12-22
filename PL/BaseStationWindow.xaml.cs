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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        private readonly IBL bl;
        private BaseStationForList BaseStation;//for action
        private int id, numChargeS;
        private string name;
        private double longitude, lattitude;
        private bool IdTextBoxChanged, CSTextBoxChanged, NameTextBoxChanged, LongChanged, LattChanged;
        #region add BaseStation
        public BaseStationWindow(IBL bl)//add BaseStation constractor
        {
            InitializeComponent();
            this.bl = bl;
            AddBaseStationGrid.Visibility = Visibility.Visible;//on this Grid i need to add this: Visibility="Hidden" after im done edit this
            _ = MessageBox.Show("in order to add a BaseStation insert the\n required information in the designated places", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            //BaseStationWeightSelector.ItemsSource = Enum.GetValues(typeof(Enums.WeightCategories));
            //BsIdSelector.ItemsSource = bl.DisplayBaseStationsId();
        }
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (LongChanged && LattChanged && CSTextBoxChanged && NameTextBoxChanged && IdTextBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this BaseStation?", "Add BaseStation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    Location location = new(0,0);
                    try
                    {
                        location = bl.AddLocation(longitude, lattitude);
                        bl.AddBaseStation(id, name, location, numChargeS); 
                    }//add try and catch with the proper exceptions from the bl.exceptions
                    catch (LocationOutOfRangeException) { _ = MessageBox.Show("the location of the base station you choose is out of range,\n please choose different base station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                    _ = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
                    NameTextBoxChanged = false;
                    IdTextBoxChanged = false;
                    CSTextBoxChanged = false;
                    LongChanged = false;
                    LattChanged = false;
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("please insert all the requested information,\nwithout it you cant add the BaseStation!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void BaseStationNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = BaseStationNameTextBox.Text;
            name = input;
            NameTextBoxChanged = true;
        }
        private void BaseStationCSTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = BaseStationIdTextBox.Text;
            bool isInt = int.TryParse(input, out numChargeS);
            if (isInt == false || id < 0)
            {
                BaseStationCSTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    BaseStationCSTextBox.Foreground = Brushes.Black;
                    CSTextBoxChanged = true;
                }
            }
        }
        private void BaseStationIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = BaseStationIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || id < 0)
            {
                BaseStationIdTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    BaseStationIdTextBox.Foreground = Brushes.Black;
                    IdTextBoxChanged = true;
                }
            }
        }
        private void doubleLongitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = doubleLongitudeTextBox.Text;
            bool isDouble = double.TryParse(input, out longitude);
            if (isDouble == false)
            {
                doubleLongitudeTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a double", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                doubleLongitudeTextBox.Foreground = Brushes.Black;
                LongChanged = true;
            }
        }
        private void doubleLattitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = doubleLattitudeTextBox.Text;
            bool isDouble = double.TryParse(input, out lattitude);
            if (isDouble == false)
            {
                doubleLattitudeTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a double", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                doubleLattitudeTextBox.Foreground = Brushes.Black;
                LattChanged = true;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddBaseStationGrid.Visibility = Visibility.Hidden;
            RemoveBaseStationGrid.Visibility = Visibility.Hidden;
            UpdateBaseStationGrid.Visibility = Visibility.Hidden;
            Close();
        }

        #endregion
        #region remove BaseStation
        public BaseStationWindow(IBL bl, bool nothing)//remove an existing BaseStation constractor
        {
            this.bl = bl;
            InitializeComponent();
            RemoveBaseStationGrid.Visibility = Visibility.Visible;
        }
        private void RemoveBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            try { bl.removeBaseStation(id); }
            catch (BaseStationNotFoundException) { MessageBox.Show("sorry, this BaseStation is not exist in our company yet!\n please choose enother BaseStation to remove", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            _ = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BaseStationRemoveIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = BaseStationRemoveIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || id < 0)
            {
                BaseStationRemoveIdTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    BaseStationRemoveIdTextBox.Foreground = Brushes.Black;
                    IdTextBoxChanged = true;
                }
            }
        }
        #endregion
        #region update BaseStation
        public BaseStationWindow(IBL bl, int nothing)//update an existing BaseStation constractor
        {
            this.bl = bl;
            InitializeComponent();
            UpdateBaseStationGrid.Visibility = Visibility.Visible;
        }
        private void UpdateBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (CSTextBoxChanged && NameTextBoxChanged && IdTextBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to update this BaseStation?", "Update BaseStation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    { bl.UpdateBaseStation(id, name, numChargeS); }//try catch
                    catch (BaseStationNotFoundException) { MessageBox.Show("sorry, this BaseStation is not exist in our company yet!\n please choose enother BaseStation", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                    _ = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
                    NameTextBoxChanged = false;
                    IdTextBoxChanged = false;
                    CSTextBoxChanged = false;
                    Close();
                    return;
                }
            }
        }
        private void UpdateBaseStationIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = UpdateBaseStationIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || id < 0)
            {
                UpdateBaseStationIdTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    UpdateBaseStationIdTextBox.Foreground = Brushes.Black;
                    IdTextBoxChanged = true;
                }
            }
        }
        private void UpdateBaseStationNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = UpdateBaseStationNameTextBox.Text;
            Name = input;
            NameTextBoxChanged = true;
        }
        private void UpdateBaseStationCSTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = UpdateBaseStationIdTextBox.Text;
            bool isInt = int.TryParse(input, out id);
            if (isInt == false || id < 0)
            {
                UpdateBaseStationCSTextBox.Foreground = Brushes.Red;
                _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                if (isInt && id >= 0)
                {
                    UpdateBaseStationCSTextBox.Foreground = Brushes.Black;
                    CSTextBoxChanged = true;
                }
            }
        }
        #endregion
    }
}