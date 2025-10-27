using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppSpotifyWPF
{
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void butCrearUser_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new Pantalles.PagCreateUser());
        }

        private void butReadUser_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new Pantalles.PagReadUser());
        }

        private void butUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new Pantalles.PagUpdateUser());
        }

        private void butDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new Pantalles.PagDeleteUser());
        }

    }
}