﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class Customer: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Customer));
        static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Customer));
        static readonly DependencyProperty PhoneProperty = DependencyProperty.Register("Phone", typeof(string), typeof(Customer));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(Customer));
        static readonly DependencyProperty ParcelsFromCustomerProperty = DependencyProperty.Register("Parcels frome customer", typeof(IEnumerable<ParcelInCustomer>), typeof(Customer));
        static readonly DependencyProperty ParcelsToCustomerProperty = DependencyProperty.Register("Parcels to customer", typeof(IEnumerable<ParcelInCustomer>), typeof(Customer));
        public int CustomerId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string CustomerName { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
        public string CustomerPhone { get => (string)GetValue(PhoneProperty); set => SetValue(PhoneProperty, value); }
        public Location Place { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value); }
        public IEnumerable<ParcelInCustomer> ParcelsFromCustomer { get => (IEnumerable<ParcelInCustomer>)GetValue(ParcelsFromCustomerProperty); set => SetValue(ParcelsFromCustomerProperty, value); }
        public IEnumerable<ParcelInCustomer> ParcelsToCustomer { get => (IEnumerable<ParcelInCustomer>)GetValue(ParcelsToCustomerProperty); set => SetValue(ParcelsToCustomerProperty, value); }
    }
}
