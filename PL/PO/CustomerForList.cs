using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class CustomerForList: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(CustomerForList));
        static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(CustomerForList));
        static readonly DependencyProperty PhoneProperty = DependencyProperty.Register("Phone", typeof(string), typeof(Customer));
        static readonly DependencyProperty NumOfDeliveredParcelsProperty = DependencyProperty.Register("Delivered Parcels", typeof(int), typeof(CustomerForList));
        static readonly DependencyProperty NumOfUnDeliveredParcelsProperty = DependencyProperty.Register("Un Delivered Parcels", typeof(int), typeof(CustomerForList));
        static readonly DependencyProperty NumOfReceivedParcelsProperty = DependencyProperty.Register("Received Parcels", typeof(int), typeof(CustomerForList));
        static readonly DependencyProperty NumOfParcelsOnTheWayProperty = DependencyProperty.Register("On the way Parcels", typeof(int), typeof(CustomerForList));
        public int CustomerId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string CustomerName { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
        public string CustomerPhone { get => (string)GetValue(PhoneProperty); set => SetValue(PhoneProperty, value); }
        public int NumOfDeliveredParcels { get => (int)GetValue(NumOfDeliveredParcelsProperty); set => SetValue(NumOfDeliveredParcelsProperty, value); }
        public int NumOfUnDeliveredParcels { get => (int)GetValue(NumOfUnDeliveredParcelsProperty); set => SetValue(NumOfUnDeliveredParcelsProperty, value); }
        public int NumOfReceivedParcels { get => (int)GetValue(NumOfReceivedParcelsProperty); set => SetValue(NumOfReceivedParcelsProperty, value); }
        public int NumOfParcelsOnTheWay { get => (int)GetValue(NumOfParcelsOnTheWayProperty); set => SetValue(NumOfParcelsOnTheWayProperty, value); }
    }
}
