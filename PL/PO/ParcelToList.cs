using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class ParcelToList: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(ParcelToList));
        static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(Enums.ParcelState), typeof(ParcelToList));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(ParcelToList));
        static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(Enums.Priorities), typeof(ParcelToList));
        static readonly DependencyProperty SenderNameProperty = DependencyProperty.Register("Sender Name", typeof(string), typeof(ParcelToList));
        static readonly DependencyProperty ReceiverNameProperty = DependencyProperty.Register("Receiver Name", typeof(string), typeof(ParcelToList));
        public int ParcelId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string SenderName { get => (string)GetValue(SenderNameProperty); set => SetValue(SenderNameProperty, value); }
        public string ReceiverName { get => (string)GetValue(ReceiverNameProperty); set => SetValue(ReceiverNameProperty, value); }
        public Enums.WeightCategories ParcelWC { get => (Enums.WeightCategories)GetValue(MaxWeightProperty); set => SetValue(MaxWeightProperty, value); }
        public Enums.Priorities ParcelPriority { get => (Enums.Priorities)GetValue(PriorityProperty); set => SetValue(PriorityProperty, value); }
        public Enums.ParcelState ParcelState { get => (Enums.ParcelState)GetValue(StateProperty); set => SetValue(StateProperty, value); }
    }
}
