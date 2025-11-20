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
        private User? selectedUser = null;
        private Border? selectedBorder = null;

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
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            changePage(new CreateUserPage(selectedUser));
        }

        private void viewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            changePage(new ViewUserPage(selectedUser));
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
        private async void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("No User selected!");
                return;
            }

            var result = MessageBox.Show(
            $"Are you sure you want to delete {selectedUser.Username}?",
            "Confirm",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteAsync($"/users/{selectedUser.Id}");
                    MessageBox.Show("✅ User deleted.");

                    unselectAllUsers();
                    await LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error deleting user:\n" + ex.Message);
                }
            }
        }

        private async void LoadUsers_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is User user)
            {
                if (selectedBorder == border)
                {
                    unselectAllUsers();
                    return;
                }

                unselectAllUsers();
                selectUser(border, user);
            }
        }

        /* PAGE METHODS */
        private void changePage(Page newPage)
        {
            NavigationService.Navigate(newPage);
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
                Ellipse avatarBackground = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.LightGray
                };

                TextBlock avatarIcon = new TextBlock
                {
                    Text = char.ToUpper(user.Username[0]).ToString(),
                    FontSize = 36,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid avatar = new Grid
                {
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                avatar.Children.Add(avatarBackground);
                avatar.Children.Add(avatarIcon);

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

        private void unselectAllUsers()
        {
            foreach (var child in UsersWrap.Children)
            {
                if (child is Border b)
                    b.BorderBrush = Brushes.Transparent;
            }
            selectedUser = null;
            selectedBorder = null;
        }

        private void selectUser(Border border, User user)
        {
            border.BorderBrush = Brushes.DeepSkyBlue;

            selectedUser = user;
            selectedBorder = border;
        }
    }
}
