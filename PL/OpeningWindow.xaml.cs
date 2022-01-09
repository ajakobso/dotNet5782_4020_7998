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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for OpeningWindow.xaml
    /// </summary>
    public partial class OpeningWindow : Window
    {
        private readonly IBL bl;
        public OpeningWindow()
        {
            bl = BLFactory.GetBL("1");
            InitializeComponent();
        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
          MessageBoxResult result=  MessageBox.Show("Are you sure you want to join our great IFLY?\n","Add Customer", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                new NewCustomerWindow(bl);//////////
            }
               // new MainWindow(bl);
        }
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            new PasswordWindow(bl);
        }
    }
}
