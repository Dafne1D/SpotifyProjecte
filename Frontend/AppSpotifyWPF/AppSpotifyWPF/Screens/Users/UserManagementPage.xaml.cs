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
        private User selectedUser = null;

        public UserManagementPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        /* BUTTON METHODS */
        private void newUserButton_Click(object sender, RoutedEventArgs e)
        {
            // changePage(new CreateUserPage());
        }
        private void viewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            // changePage(new ReadUserPage(selectedUser));
        }
        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            changePage(new UpdateUserPage(selectedUser));
        }
        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            // Delete User somehow
        }

        private async void LoadUsers_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {

        }

        /* PAGE METHODS */
        private void changePage(Page newPage)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(newPage);
        }

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

                Border border = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(10),
                    Width = 100,
                    Child = new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Children = { avatar, name }
                    },
                    Tag = user
                };

                border.MouseLeftButtonDown += UserCard_MouseLeftButtonDown;

                UsersWrap.Children.Add(border);
            }
        }

        private void UserCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is User user)
            {
                foreach (var child in UsersWrap.Children)
                {
                    if (child is Border b)
                        b.BorderBrush = Brushes.Transparent;
                }

                border.BorderBrush = Brushes.DeepSkyBlue;

                selectedUser = user;
            }
        }
    }
}
