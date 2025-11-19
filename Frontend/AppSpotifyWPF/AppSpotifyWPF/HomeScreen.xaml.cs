
using System.Windows;
using AppSpotifyWPF.Screens.Users;
using AppSpotifyWPF.Screens.Songs;
using System.Windows.Controls;

namespace AppSpotifyWPF
{
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void changePage(Page newPage)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(newPage);
        }

        private void userManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new UserManagementPage());
        }

        private void songManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagReadUser());
        }

        private void playlistManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagCreateUser());
        }
    }
}