using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class PagCreateUser : Page
    {
        private readonly ApiService _apiService = new ApiService();

        public PagCreateUser()
        {
            InitializeComponent();
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
                Password = password
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
            txtPassword.Clear();
            txtRepeatPassword.Clear();
        }
        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is HomeScreen home)
            {
                home.MainFrame.Visibility = Visibility.Collapsed;
                home.HomeContent.Visibility = Visibility.Visible;
            }
        }
    }
}
