using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class Parcel: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Parcel));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(Parcel));
        static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(Enums.Priorities), typeof(Parcel));
        static readonly DependencyProperty SCIParcelProperty = DependencyProperty.Register("Sender", typeof(CustomerInParcel), typeof(Parcel));
        static readonly DependencyProperty DCIParcelProperty = DependencyProperty.Register("Receiver", typeof(CustomerInParcel), typeof(Parcel));
        static readonly DependencyProperty DInParcelProperty = DependencyProperty.Register("Drone", typeof(DroneInParcel), typeof(Parcel));
        static readonly DependencyProperty CreationTimeProperty = DependencyProperty.Register("Creation Time", typeof(DateTime?), typeof(Parcel));
        static readonly DependencyProperty AscriptionTimeProperty = DependencyProperty.Register("Ascription Time", typeof(DateTime?), typeof(Parcel));
        static readonly DependencyProperty PickUpTimeProperty = DependencyProperty.Register("Pick Up Time", typeof(DateTime?), typeof(Parcel));
        static readonly DependencyProperty DeliveringTimeProperty = DependencyProperty.Register("Delivering Time", typeof(DateTime?), typeof(Parcel));
        public int ParcelId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public CustomerInParcel SCIParcel { get => (CustomerInParcel)GetValue(SCIParcelProperty); set => SetValue(SCIParcelProperty, value); }//Source Customer in parcel - the sender
        public CustomerInParcel DCIParcel { get => (CustomerInParcel)GetValue(DCIParcelProperty); set => SetValue(DCIParcelProperty, value); }//Destination Customer in parcel - the receiver
        public Enums.WeightCategories ParcelWC { get => (Enums.WeightCategories)GetValue(MaxWeightProperty); set => SetValue(MaxWeightProperty, value); }
        public Enums.Priorities ParcelPriority { get => (Enums.Priorities)GetValue(PriorityProperty); set => SetValue(PriorityProperty, value); }
        public DroneInParcel DInParcel { get => (DroneInParcel)GetValue(DInParcelProperty); set => SetValue(DInParcelProperty, value); }
        public DateTime? ParcelCreationTime { get => (DateTime?)GetValue(CreationTimeProperty); set => SetValue(CreationTimeProperty, value); }
        public DateTime? ParcelAscriptionTime { get => (DateTime?)GetValue(AscriptionTimeProperty); set => SetValue(AscriptionTimeProperty, value); }
        public DateTime? ParcelPickUpTime { get => (DateTime?)GetValue(PickUpTimeProperty); set => SetValue(PickUpTimeProperty, value); }
        public DateTime? ParcelDeliveringTime { get => (DateTime?)GetValue(DeliveringTimeProperty); set => SetValue(DeliveringTimeProperty, value); }
    }
}
