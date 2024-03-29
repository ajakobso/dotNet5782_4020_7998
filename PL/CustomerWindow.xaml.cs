﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly IBL bl;
        private Customer customer;
        IEnumerable<ParcelInCustomer> parcelsFromCustomer;
        IEnumerable<ParcelInCustomer> parcelsToCustomer;
        private int id;
        private string name;
        private string phone;
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
        public CustomerWindow(IBL bl, int customerId)//actions on existing customer constractor
        {
            this.bl = bl;
            InitializeComponent();
            ActionsOnCustomerGrid.Visibility = Visibility.Visible;
            try
            { customer = /*PO.BoPoAdapter.CustomerBoPo(*/bl.DisplayCustomer(customerId)/*)*/; }
            catch (CustomerNotFoundException) { MessageBox.Show("sorry, this customer is not exist in our company yet!\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            CustomerId.Text = customer.CustomerId.ToString();
            CustomerName.Text = customer.CustomerName;
            CustomerPhone.Text = customer.CustomerPhone;
            CustomerLocation.Text = customer.Place.ToString();
            parcelsFromCustomer = customer.ParcelsFromCustomer;
            parcelsToCustomer = customer.ParcelsToCustomer;
            ParcelsFromDataGrid.DataContext = parcelsFromCustomer;
            ParcelsToDataGrid.DataContext = parcelsToCustomer;
            //if the name is changed then update the name - same about the phone, i just dont know how to check if the text changed.
        }
        private void ParcelsFromDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(bl, ((CustomerInParcel)ParcelsFromDataGrid.SelectedItem).CustomerId).Show();
        }

        private void ParcelsToDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(bl, ((CustomerInParcel)ParcelsToDataGrid.SelectedItem).CustomerId).Show();
        }
        #endregion
        private void SuccessOperation()
        {
            MessageBoxResult result = MessageBox.Show("operation successfully completed", "SUCCESS!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                { customer = /*PO.BoPoAdapter.CustomerBoPo(*/bl.DisplayCustomer(customer.CustomerId)/*)*/; }
                catch (CustomerNotFoundException) { MessageBox.Show("this customer is not exist\n please choose enother customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
            }
        }
        /*      <Grid x:Name="NameUpdateGrid" HorizontalAlignment="Stretch" Height="36" Margin="2,8,0,0" Grid.Row="2" VerticalAlignment="Stretch" Width="auto">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="0.5*"/>
                    <ColumnDefinition Width="0.5*"/>
                    <ColumnDefinition Width="0.5*"/>
                </Grid.ColumnDefinitions>
                <TextBox x:Name="CustomerNameTBox" Height="20" Width="120" Grid.Column="1" HorizontalAlignment="Stretch" Grid.Row="2" TextWrapping="Wrap" VerticalAlignment="Stretch" TextChanged="CustomerNameTBox_TextChanged"/>
                <Label x:Name="CustomerName" Background="FloralWhite" Content="Customer's name" Grid.Column="0" Grid.Row="2" HorizontalAlignment="Stretch" Height="38" VerticalAlignment="Stretch" Width="130" FontFamily="Berlin Sans FB Demi" FontSize="22" />
                <Button x:Name="NameUpdate" Grid.Column="2" HorizontalAlignment="Stretch" Height="32" VerticalAlignment="Stretch" Width="130" FontFamily="Berlin Sans FB Demi" FontSize="15" BorderBrush="{x:Null}" Click="NameUpdate_Click">
                    <Button.Background>
                        <ImageBrush ImageSource="\Images\update-button.jpg"/>
                    </Button.Background>
                </Button>
            </Grid>
            <Grid x:Name="PhoneUpdateGrid" HorizontalAlignment="Stretch" Height="36" Margin="2,8,0,0" Grid.Row="2" VerticalAlignment="Stretch" Width="auto">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="0.5*"/>
                    <ColumnDefinition Width="0.5*"/>
                    <ColumnDefinition Width="0.5*"/>
                </Grid.ColumnDefinitions>
                <TextBox x:Name="CustomerPhoneTBox" Height="20" Width="74" Grid.Column="1" HorizontalAlignment="Stretch" TextWrapping="Wrap" VerticalAlignment="Stretch" TextChanged="CustomerNameTBox_TextChanged"/>
                <Label x:Name="CustomerPhone" Background="FloralWhite" Content="new phone" Grid.Column="0" HorizontalAlignment="Stretch" Height="38" VerticalAlignment="Stretch" Width="172" FontFamily="Berlin Sans FB Demi" FontSize="22" />
                <Button x:Name="PhoneUpdate" Grid.Column="2" HorizontalAlignment="Stretch" Height="32" VerticalAlignment="Stretch" Width="130" FontFamily="Berlin Sans FB Demi" FontSize="15" BorderBrush="{x:Null}" Click="PhoneUpdate_Click">
                    <Button.Background>
                        <ImageBrush ImageSource="\Images\update-button.jpg"/>
                    </Button.Background>
                </Button>
            </Grid>
   */
    }
}
