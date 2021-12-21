using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class ParcelInCustomer: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(ParcelInCustomer));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(ParcelInCustomer));
        static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(Enums.Priorities), typeof(ParcelInCustomer));
        static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(Enums.ParcelState), typeof(ParcelInCustomer));
        static readonly DependencyProperty SCIParcelProperty = DependencyProperty.Register("Sender", typeof(CustomerInParcel), typeof(ParcelInCustomer));
        static readonly DependencyProperty DCIParcelProperty = DependencyProperty.Register("Receiver", typeof(CustomerInParcel), typeof(ParcelInCustomer));

        public int ParcelId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public Enums.WeightCategories ParcelWC { get => (Enums.WeightCategories)GetValue(MaxWeightProperty); set => SetValue(MaxWeightProperty, value); }
        public Enums.Priorities ParcelPriority { get => (Enums.Priorities)GetValue(PriorityProperty); set => SetValue(PriorityProperty, value); }
        public Enums.ParcelState ParcelState { get => (Enums.ParcelState)GetValue(StateProperty); set => SetValue(StateProperty, value); }
        public CustomerInParcel SourceCustomer { get => (CustomerInParcel)GetValue(SCIParcelProperty); set => SetValue(SCIParcelProperty, value); }//Source Customer in parcel - the sender
        public CustomerInParcel DestinationCustomer { get => (CustomerInParcel)GetValue(DCIParcelProperty); set => SetValue(DCIParcelProperty, value); }//Destination Customer in parcel - the receiver

    }
}
