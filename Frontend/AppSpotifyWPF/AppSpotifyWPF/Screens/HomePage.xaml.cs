using AppSpotifyWPF.Screens.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AppSpotifyWPF.Screens.Songs;

namespace AppSpotifyWPF.Screens
{
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
            // Note: Your original code navigated to PagCreateSong for Playlists
            changePage(new PagCreateSong());
        }

        // NEW NAVIGATION HANDLERS

        private void roleManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagRoleManagement());
        }

        private void permissionManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagPermissionManagement());
        }

        private void assignmentManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PagUserRoleAssignment());
        }
    }
}