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
    /// Interaction logic for BaseStationsListWindow.xaml
    /// </summary>
    public partial class BaseStationsListWindow : Window
    {
        private readonly IBL bl;
        private BaseStationForList BaseStation;
        public BaseStationsListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            BaseStation = new();
            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);//predicate that always true to show all BaseStations
            
        }
        //public void RefreshBaseStationsListWindow()
        //{
        //    new BaseStationsListWindow(bl);
        //    //int weightSelectoeSelectedIndex = WeightSelector.SelectedIndex;
        //    //int statusSelectorSelectedIndex = StatusSelector.SelectedIndex;
        //    if (WeightSelector != null || StatusSelector != null)
        //    //if (weightSelectoeSelectedIndex > -1 || statusSelectorSelectedIndex > -1) 
        //    {
        //        if (WeightSelector != null && StatusSelector != null)
        //        //if (WeightSelector.SelectedIndex > -1 && StatusSelector.SelectedIndex > -1)
        //        {
        //            Enums.BaseStationStatuses status = (Enums.BaseStationStatuses)StatusSelector.SelectedItem;
        //            Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
        //            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationState == status && x.MaxWeight == weight);
        //        }
        //        else
        //        {
        //            if (WeightSelector != null)
        //            //if (WeightSelector.SelectedIndex > -1)
        //            {
        //                Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
        //                BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.MaxWeight == weight);
        //            }
        //            else
        //            {
        //                if (StatusSelector.SelectedIndex > -1)
        //                {
        //                    Enums.BaseStationStatuses status = (Enums.BaseStationStatuses)StatusSelector.SelectedItem;
        //                    BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationState == status);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("enjoy in your next action!\n", "Goodluck", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        //    }

        //}

        //private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (StatusSelector.SelectedItem != null)
        //    {
        //        Enums.BaseStationStatuses status = (Enums.BaseStationStatuses)StatusSelector.SelectedItem;
        //        if (WeightSelector.SelectedIndex > -1)//check if the second combo box selected
        //        {
        //            Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
        //            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationState == status && x.MaxWeight == weight);
        //        }
        //        else
        //        {
        //            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationState == status);
        //        }
        //    }
        //}

        //private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (WeightSelector.SelectedItem != null)
        //    {
        //        Enums.WeightCategories weight = (Enums.WeightCategories)WeightSelector.SelectedItem;
        //        if (StatusSelector.SelectedIndex > -1 && StatusSelector.SelectedItem != null)//check if the second combo box selected
        //        {
        //            Enums.BaseStationStatuses status = (Enums.BaseStationStatuses)StatusSelector.SelectedItem;
        //            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationState == status && x.MaxWeight == weight);
        //        }
        //        else
        //        {
        //            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.MaxWeight == weight);
        //        }
        //    }
        //}

        private void AddBaseStationWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //new BaseStationWindow(bl).ShowDialog();
            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
            //WeightSelector_SelectionChanged(WeightSelector, null);
            //StatusSelector_SelectionChanged(StatusSelector, null);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            BaseStationsListView.ItemsSource = bl.DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BaseStationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStation.BaseStationId = ((BaseStationForList)BaseStationsListView.SelectedItem).BaseStationId;//meanwhile until i figure out how to get the BaseStation id in the row clicked
            //new BaseStationWindow(bl, BaseStation.BaseStationId).Show();
        }


    }
}
