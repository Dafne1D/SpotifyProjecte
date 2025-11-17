using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF.Screens.Songs
{
    public partial class PagReadSong : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Song> _songs = new List<Song>();
        public PagReadSong()
        {
            InitializeComponent();
        }
        private async void LoadSongs_Click(object sender, RoutedEventArgs e)
        {
            await LoadSongs();
        }

        private async Task LoadSongs()
        {
            try
            {
                _songs = await _apiService.GetAsync<List<Song>>("/songs");
                RenderSongs(_songs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error loading users:\n" + ex.Message);
            }
        }

        private void RenderSongs(IEnumerable<Song> songs)
        {
            SongsWrap.Children.Clear();

            var sortedSongs = songs.OrderBy(s => s.Title); 

            foreach (var song in sortedSongs)
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
                    Text = song.Title,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                // StackPanel que contiene el avatar + nombre
                StackPanel card = new StackPanel
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                card.Children.Add(avatar);
                card.Children.Add(name);

                SongsWrap.Children.Add(card);
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

            var filtered = _songs.FindAll(u =>
                u.Title.ToLower().Contains(query));
            RenderSongs(filtered);
        }
    }
}
