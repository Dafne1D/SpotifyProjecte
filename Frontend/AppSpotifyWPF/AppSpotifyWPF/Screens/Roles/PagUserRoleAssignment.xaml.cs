using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens
{
    public partial class PagUserRoleAssignment : Page
    {
        public PagUserRoleAssignment() { InitializeComponent(); }

        private void OnAssignClicked(object sender, RoutedEventArgs e)
        {
            // Logic to call app.MapPost("/userRoles", ...) 
            MessageBox.Show("Role Assigned Successfully!");
        }

        private void OnBackClicked(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}