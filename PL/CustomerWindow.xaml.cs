﻿using System;
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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly IBL bl;
        private Customer customer;
        private int id;
        private string name;
        private string phone;
        private int NumOfDeliveredParcels;
        private int NumOfUnDeliveredParcels;
        private int NumOfRecivedParcels;
        private int NumOfParcelsOnTheWay;
        double longitude, lattitude;
        private bool IdTextBoxChanged, PhoneTextBoxChanged, NameTextBoxChanged, LongitudeTextBoxChanged, lattitudeTextBoxChanged;
        #region add customer
        public CustomerWindow(IBL bl)//add customer (constructor)
        {
            InitializeComponent();
            this.bl = bl;
            AddCustomerGrid.Visibility = Visibility.Visible;
            _ = MessageBox.Show("in order to add a customer insert the\n required information in the designated places", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }
        private void CustomerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input;
            input = CustomerNameTextBox.Text;
            name = input;
            NameTextBoxChanged = true;
        }
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to add this customer?", "Add Customer", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)//the exe callapse here//////////////////////////
            {
                try { bl.AddCustomer(id, name, phone, new Location(longitude, lattitude)); }//add try and catch with the proper exceptions from the bl.exceptions
                catch (LocationOutOfRangeException) { MessageBox.Show("the location you choose is out of range,\n please choose different location", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                catch (AddExistingCustomerException) { MessageBox.Show("you are trying to add an existing customer,\n please try enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
                NameTextBoxChanged = false;
                IdTextBoxChanged = false;
                LongitudeTextBoxChanged = false;
                lattitudeTextBoxChanged = false;
                Close();
                return;
            }
            MessageBox.Show("please insert all the requested information,\nwithout it you cant add the customer!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void CustomerIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string input;
            //input = CustomerIdTextBox.Text;
            //bool isInt = int.TryParse(input, out id);
            //if (isInt == false || id < 0)
            //{
            //    CustomerIdTextBox.Foreground = Brushes.Red;
            //    _ = MessageBox.Show("Invalid input, please enter a valid non-negative integer", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            //}
            //else
            //{
            //    CustomerIdTextBox.Foreground = Brushes.Black;
            //    IdTextBoxChanged = true;
            //}
            IdTextBoxChanged = true;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerGrid.Visibility = Visibility.Hidden;
            ActionsOnCustomerGrid.Visibility = Visibility.Hidden;
            Close();
        }
        private void CustomerLongitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string input;
            //input = CustomerLongitudeTextBox.Text;
            //double.TryParse(input, out longitude);
            //CustomerLongitudeTextBox.Foreground = Brushes.Black;
            LongitudeTextBoxChanged = true;
        }
        private void CustomerlattitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string input;
            //input = CustomerlattitudeTextBox.Text;
            //double.TryParse(input, out lattitude);
            //CustomerlattitudeTextBox.Foreground = Brushes.Black;
            lattitudeTextBoxChanged = true;
        }

        private void CustomerPhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //phone = CustomerPhoneTextBox.Text;
            //CustomerPhoneTextBox.Foreground = Brushes.Black;
            PhoneTextBoxChanged = true;
        }
        #endregion
        #region actions on customer
        public CustomerWindow(IBL bl, int CustomerId)//actions on existing customer constractor
        {
            this.bl = bl;
            InitializeComponent();
            ActionsOnCustomerGrid.Visibility = Visibility.Visible;
            try
            { customer = bl.DisplayCustomer(CustomerId); }
            catch (CustomerNotFoundException) { MessageBox.Show("sorry, this customer is not exist in our company yet!\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            CustomerDataGrid.DataContext = customer;
        }
        private void CustomerNameTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            customer.CustomerName = CustomerNameTBox.Text;
            NameTextBoxChanged = true;
        }
        private void CustomerPhoneTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            customer.CustomerPhone = CustomerPhoneTextBox.Text;
            PhoneTextBoxChanged = true;
        }
        private void NameUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBoxChanged)
            {
                try
                { bl.UpdateCustomer(customer.CustomerId, CustomerNameTextBox.Text, customer.CustomerPhone); }//try catch
                catch (CustomerNotFoundException) { MessageBox.Show("sorry, this customer is not exist in our company yet!\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                NameTextBoxChanged = false;
            }
            SuccessOperation();
        }
        private void PhoneUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (PhoneTextBoxChanged)
            {
                try
                { bl.UpdateCustomer(customer.CustomerId, customer.CustomerName, CustomerPhoneTextBox.Text); }//try catch
                catch (CustomerNotFoundException) { MessageBox.Show("sorry, this customer is not exist in our company yet!\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
                PhoneTextBoxChanged = false;
            }
        }
        #endregion
        private void SuccessOperation()
        {
            MessageBoxResult result = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                { CustomerDataGrid.ItemsSource = bl.DisplayDrone(customer.CustomerId).ToString(); }
                catch (DroneIdNotFoundException) { MessageBox.Show("this customer is not exist\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            }
        }
    }
}