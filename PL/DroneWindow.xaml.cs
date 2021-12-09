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
using IBL;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private readonly Ibl bl;
        private int DId;
        private string DModel;
        private IBL.BO.Enums.WeightCategories DWeight;
        private int BsId;
        private bool IdTextBoxChanged, ModelTextBoxChanged;
        
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
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            if (BsIdSelector.SelectedIndex > -1 && DroneWeightSelector.SelectedIndex > -1 && ModelTextBoxChanged && IdTextBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this drone?", "Add Drone", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes /*&& input is valid*/)
                {
                    bl.AddDrone(DId, DModel, DWeight, BsId);//add try and catch with the proper exceptions from the bl.exceptions
                }
            }
        }

        private void BsIdSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BsId = (int)BsIdSelector.SelectedItem;
        }

        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DWeight = (IBL.BO.Enums.WeightCategories)DroneWeightSelector.SelectedItem;
        }

        private void DroneIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DId = Convert.ToInt32(DroneIdTextBox.Text);
            IdTextBoxChanged = true;
        }
        private void DroneModelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DModel = DroneModelTextBox.Text;
            ModelTextBoxChanged = true;
        }
    }
}
