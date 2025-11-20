using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class UpdateUserPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private User selectedUser;

        public UpdateUserPage(User selectedUser)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;

            txtTitle.Text = $"Edit User \"{selectedUser.Username}\"";

            lblName.Text = selectedUser.Username;
            lblEmail.Text = selectedUser.Email;
            lblRole.Text = "User";
        }

        private async void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string repeatPassword = txtRepeatPassword.Password;

            if (password != repeatPassword)
            {
                MessageBox.Show("The passwords do not match!");
                return;
            }

            var newUser = new User
            {
                Username = name,
                Email = email,
                //Rol
            };

            try
            {
                var createdUser = await _apiService.PostAsync<User>("/users", newUser);
                MessageBox.Show($"User created! ID: {createdUser.Id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}");
            }
            ClearBoxes();
        }

        private void ClearBoxes()
        {
            txtName.Clear();
            txtEmail.Clear();
            //txtPassword.Clear();
            //txtRepeatPassword.Clear();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserManagementPage());
        }


        private async void saveUserButton_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string repeatPassword = txtRepeatPassword.Password;

            if (password != repeatPassword)
            {
                MessageBox.Show("Les contrasenyes no coincideixen");
                return;
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("El nom d'usuari i el correu són obligatoris");
                return;
            }

            if (!email.EndsWith("@gmail.com"))
            {
                MessageBox.Show("Només es permeten comptes de Gmail");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                MessageBox.Show("La contrasenya ha de tenir almenys 8 caràcters");
                return;
            }

            var updatedUser = new User
            {
                Id = selectedUser.Id,
                Username = name,
                Email = email,
                Password = password,
                Salt = ""
            };

            try
            {
                await _apiService.PutAsync($"/users/{selectedUser.Id}", updatedUser);
                MessageBox.Show("User updated!");

                selectedUser.Username = name;
                selectedUser.Email = email;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user:\n{ex.Message}");
            }
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
