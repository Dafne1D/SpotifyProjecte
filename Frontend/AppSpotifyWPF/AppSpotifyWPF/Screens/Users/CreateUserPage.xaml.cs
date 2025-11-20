using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class CreateUserPage : Page
    {
        private readonly ApiService _apiService = new ApiService();

        public CreateUserPage()
        {
            InitializeComponent();
        }

        private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("El nom i el correu són obligatoris");
                return;
            }

            if (!email.EndsWith("@gmail.com"))
            {
                MessageBox.Show("Només es permeten correus Gmail");
                return;
            }

            var newUser = new User
            {
                Username = name,
                Email = email,
                Password = password,
                Salt = ""
            };

            try
            {
                var createdUser = await _apiService.PostAsync<User>("/users", newUser);
                MessageBox.Show($"Usuari creat correctament! ID: {createdUser.Id}");
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creant l'usuari:\n" + ex.Message);
            }
        }

        private void ClearFields()
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
