using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class CustomerInParcel: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Customer));
        static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Customer));
        public int CustomerId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string CustomerName { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
    }
}
