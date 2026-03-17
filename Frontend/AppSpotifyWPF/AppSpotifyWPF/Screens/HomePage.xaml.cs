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
using AppSpotifyWPF.Screens.Playlists;

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

        private void ForceAdmin_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUserId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            MessageBox.Show("Admin mode enabled");
        }

        private void userManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new UserManagementPage());
        }

        private void songManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new SongManagementPage());
        }

        private void playlistManagementButton_Click(object sender, RoutedEventArgs e)
        {
            changePage(new PlaylistManagementPage());
        }

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