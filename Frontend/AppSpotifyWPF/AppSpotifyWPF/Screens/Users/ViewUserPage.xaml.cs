using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class ViewUserPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private User selectedUser;

        public ViewUserPage(User user)
        {
            InitializeComponent();
            selectedUser = user;

            lblName.Text = selectedUser.Username;
            lblEmail.Text = selectedUser.Email;

            lblRole.Text = "User";
            lblPermissions.Text = "Cap permís";
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserManagementPage());
        }

        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UpdateUserPage(selectedUser));
        }

        private async void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No hi ha cap usuari seleccionat");
                return;
            }

            var result = MessageBox.Show(
                $"Estàs segur que vols eliminar \"{selectedUser.Username}\"?",
                "Confirmació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteAsync($"/users/{selectedUser.Id}");
                    MessageBox.Show("Usuari eliminat");

                    NavigationService.Navigate(new UserManagementPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminant l'usuari:\n" + ex.Message);
                }
            }
        }
    }
}
