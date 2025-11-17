using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;


namespace AppSpotifyWPF.Screens.Songs {
     public partial class PagDeleteSong : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Song> _songs = new List<Song>();
        private Song _selectedSong;

        public PagDeleteSong()
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

        private void SongCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Song song)
            {
                // Quitar resaltado de los demás
                foreach (var child in SongsWrap.Children)
                {
                    if (child is Border b)
                        b.BorderBrush = Brushes.Transparent;
                }

                // Resaltar el seleccionado
                border.BorderBrush = Brushes.DeepSkyBlue;

                // Guardar el usuario seleccionado
                _selectedSong = song;
            }
        }


        private void RenderSongs(IEnumerable<Song> songs)
        {
            SongsWrap.Children.Clear();

            foreach (var song in songs)
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
                    Tag = song
                };

                // Evento de click para seleccionar
                border.MouseLeftButtonDown += SongCard_Click;

                SongsWrap.Children.Add(border);
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
                u.Title.ToLower().Contains(query) ||
                u.Artist.ToLower().Contains(query));

            RenderSongs(filtered);
        }
        private async void DeleteSong_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSong == null)
            {
                MessageBox.Show("Select an user.");
                return;
            }

            var result = MessageBox.Show(
                $"¿Are you sure you wnat to delete {_selectedSong.Title}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteAsync($"/songs/{_selectedSong.Id}");
                    MessageBox.Show("✅ Song eliminated.");

                    _selectedSong = null;
                    await LoadSongs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error deleting user:\n" + ex.Message);
                }
            }
        }
    }
}

