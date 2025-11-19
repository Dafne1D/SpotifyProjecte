using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
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

namespace AppSpotifyWPF.Screens.Users
{
    /// <summary>
    /// Interaction logic for UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        private readonly ApiService _apiService = new ApiService();

        public UserManagementPage()
        {
            InitializeComponent();
        }

        /* BUTTON METHODS */
        private void newUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void viewUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadUsers_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {

        }

        /* PAGE METHODS */
        private async Task LoadUsers()
        {
            try
            {
                List<User> _users = await _apiService.GetAsync<List<User>>("/users");

                RenderUsers(_users);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error loading users:\n" + ex.Message);
            }
        }

        private void RenderUsers(IEnumerable<User> users)
        {
            UsersWrap.Children.Clear();

            var sortedUsers = users.OrderBy(u => u.Username);

            foreach (var user in sortedUsers)
            {
                Ellipse avatar = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.LightGray,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                TextBlock name = new TextBlock
                {
                    Text = user.Username,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                StackPanel card = new StackPanel
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                card.Children.Add(avatar);
                card.Children.Add(name);

                UsersWrap.Children.Add(card);
            }
        }
    }
}
