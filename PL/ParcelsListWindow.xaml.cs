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
using System.Collections.ObjectModel;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelsList.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private readonly IBL bl;
        private PO.ParcelToList parcel;
        public ObservableCollection<PO.ParcelToList> ParcelsList { get; set; }
        public ParcelsListWindow() { }//an empty consractor - never used, its just formal for the grouping
        public ParcelsListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            parcel = new();
            ParcelsList = PO.BoPoAdapter.ParcelToListAdapter(bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId));
            ParcelForListDataGrid.DataContext = ParcelsList;
            ParcelForListDataGrid.ItemsSource = ParcelsList;
        }
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl).ShowDialog();
            ParcelsList = PO.BoPoAdapter.ParcelToListAdapter(bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId));
            ParcelForListDataGrid.DataContext = ParcelsList;
            ParcelForListDataGrid.ItemsSource = ParcelsList;
        }
        private void RemoveParcelButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Parcel ParcelToRemove=new BO.Parcel();
            try {ParcelToRemove = bl.DisplayParcel(parcel.ParcelId);            }
            catch (BO.ParcelIdNotFoundException) { MessageBox.Show("This parcel does not exist in here,\n", "please choose another parcel\n", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK); }
           if (ParcelToRemove.DInParcel.DroneId!=0)
            {
                try { bl.RemoveParcel(ParcelToRemove.ParcelId); }
                catch (BO.ParcelIdNotFoundException ){ MessageBox.Show("This parcel does not exist in here,\n", "please choose another parcel\n", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }
        private void UpdateParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, parcel.ParcelId).Show();

        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ParcelsList = PO.BoPoAdapter.ParcelToListAdapter(bl.DisplayParcelsList(x => x.ParcelId == x.ParcelId));
            ParcelForListDataGrid.DataContext = ParcelsList;
            ParcelForListDataGrid.ItemsSource = ParcelsList;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ParcelForListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parcel.ParcelId = ((PO.ParcelToList)ParcelForListDataGrid.SelectedItem).ParcelId;
            new ParcelWindow(bl, parcel.ParcelId).Show();
            Reset_Click(sender, e);
        }
    }
}
