using AppSpotifyWPF.Screens.Users;
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
using AppSpotifyWPF.Screens.Songs;

namespace AppSpotifyWPF.Screens
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void changePage(Page newPage)
        {
            NavigationService.Navigate(newPage);
        }

        private void userManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new UserManagementPage());
        }

        private void songManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagReadSong());
        }

        private void playlistManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagCreateSong());
        }
    }
}
