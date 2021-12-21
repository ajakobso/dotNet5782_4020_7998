using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class ParcelInDelivering: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(ParcelInDelivering));
        static readonly DependencyProperty StateProperty = DependencyProperty.Register("Picked Up?", typeof(bool), typeof(ParcelInDelivering));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(ParcelInDelivering));
        static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(Enums.Priorities), typeof(ParcelInDelivering));
        static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Sender", typeof(CustomerInParcel), typeof(ParcelInDelivering));
        static readonly DependencyProperty DestinationProperty = DependencyProperty.Register("Receiver", typeof(CustomerInParcel), typeof(ParcelInDelivering));
        static readonly DependencyProperty SenderLocationProperty = DependencyProperty.Register("Sender Location", typeof(Location), typeof(ParcelInDelivering));
        static readonly DependencyProperty TargetLocationProperty = DependencyProperty.Register("Receiver Location", typeof(Location), typeof(ParcelInDelivering));
        static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(double), typeof(ParcelInDelivering));
        public int ParcelId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public bool ParcelState { get => (bool)GetValue(StateProperty); set => SetValue(StateProperty, value); }//true if the parcel picked up, false if its waiting for pick up    
        public Enums.WeightCategories ParcelWC { get => (Enums.WeightCategories)GetValue(MaxWeightProperty); set => SetValue(MaxWeightProperty, value); }
        public Enums.Priorities ParcelPriority { get => (Enums.Priorities)GetValue(PriorityProperty); set => SetValue(PriorityProperty, value); }
        public CustomerInParcel Source{ get => (CustomerInParcel)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }//Source Customer in parcel - the sender
        public CustomerInParcel Destination { get => (CustomerInParcel)GetValue(DestinationProperty); set => SetValue(DestinationProperty, value); }//Destination Customer in parcel - the receiver
        public Location SenderLocation { get => (Location)GetValue(SenderLocationProperty); set => SetValue(SenderLocationProperty, value); }
        public Location TargetLocation { get => (Location)GetValue(TargetLocationProperty); set => SetValue(TargetLocationProperty, value); }
        public double Distance { get => (double)GetValue(DistanceProperty); set => SetValue(DistanceProperty, value); }
    }
}
