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
        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            //Window parentWindow = Window.GetWindow(this);
            //if (parentWindow is HomeScreen home)
            //{
            //    home.MainFrame.Visibility = Visibility.Collapsed;
            //    home.HomeContent.Visibility = Visibility.Visible;
            //}
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            //var parentWindow = Window.GetWindow(this);
            //if (parentWindow is HomeScreen home)
            //{
            //    home.MainFrame.Visibility = Visibility.Collapsed;
            //    home.HomeContent.Visibility = Visibility.Visible;
            //}
        }


        private async void saveUserButton_Click(object sender, RoutedEventArgs e)
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Name and email are mandatory");
                return;
            }

            if (!email.EndsWith("@gmail.com"))
            {
                MessageBox.Show("Email must be Gmail");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                MessageBox.Show("Password must have at least 8 characters.");
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


        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
