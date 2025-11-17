using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class PagReadUser : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<User> _users = new List<User>();
        public PagReadUser()
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
    }
}
