using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class PagDeleteUser : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<User> _users = new List<User>();
        private User _selectedUser;

        public PagDeleteUser()
        {
            InitializeComponent();
        }
        private async void LoadUsers_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                _users = await _apiService.GetAsync<List<User>>("/users");
                RenderUsers(_users);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error loading users:\n" + ex.Message);
            }
        }

        private void UserCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is User user)
            {
                // Quitar resaltado de los demás
                foreach (var child in UsersWrap.Children)
                {
                    if (child is Border b)
                        b.BorderBrush = Brushes.Transparent;
                }

                // Resaltar el seleccionado
                border.BorderBrush = Brushes.DeepSkyBlue;

                // Guardar el usuario seleccionado
                _selectedUser = user;
            }
        }


        private void RenderUsers(IEnumerable<User> users)
        {
            UsersWrap.Children.Clear();

            foreach (var user in users)
            {
                // Círculo gris pa imagen
                Ellipse avatar = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.LightGray,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                // Nombre del usuario
                TextBlock name = new TextBlock
                {
                    Text = user.Username,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                // StackPanel que contiene el avatar + nombre
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

                // Evento de click para seleccionar
                border.MouseLeftButtonDown += UserCard_Click;

                UsersWrap.Children.Add(border);
            }
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow is HomeScreen home)
            {
                home.MainFrame.Visibility = Visibility.Collapsed;
                home.HomeContent.Visibility = Visibility.Visible;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = txtSearch.Text.Trim().ToLower();

            var filtered = _users.FindAll(u =>
                u.Username.ToLower().Contains(query) ||
                u.Email.ToLower().Contains(query));

            RenderUsers(filtered);
        }
        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Select an user.");
                return;
            }

            var result = MessageBox.Show(
                $"¿Are you sure you wnat to delete {_selectedUser.Username}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteAsync($"/users/{_selectedUser.Id}");
                    MessageBox.Show("✅ User eliminated.");

                    _selectedUser = null;
                    await LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error deleting user:\n" + ex.Message);
                }
            }
        }
    }
}
