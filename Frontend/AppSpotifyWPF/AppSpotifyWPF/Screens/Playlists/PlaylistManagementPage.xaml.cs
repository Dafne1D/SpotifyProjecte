using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Screens;
using AppSpotifyWPF.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AppSpotifyWPF.Screens.Playlists
{
    public partial class PlaylistManagementPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private Playlist? selectedPlaylist = null;
        private Border? selectedBorder = null;

        public PlaylistManagementPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPlaylists();
        }

        private async Task LoadPlaylists()
        {
            var playlists = await _apiService.GetAsync<List<Playlist>>("/playlists");
            Render(playlists);
        }

        private void Render(IEnumerable<Playlist> playlists)
        {
            PlaylistsWrap.Children.Clear();

            foreach (var p in playlists.OrderBy(x => x.Name))
            {
                Ellipse avatar = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.LightGray
                };

                TextBlock name = new TextBlock
                {
                    Text = p.Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                Border border = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(10),
                    Width = 100,
                    Child = new StackPanel
                    {
                        Children = { avatar, name }
                    },
                    Tag = p
                };

                border.MouseLeftButtonDown += Click;
                PlaylistsWrap.Children.Add(border);
            }
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.Tag is Playlist p)
            {
                if (selectedBorder == b)
                {
                    Unselect();
                    return;
                }

                Unselect();
                b.BorderBrush = Brushes.DeepSkyBlue;

                selectedPlaylist = p;
                selectedBorder = b;
            }
        }

        private void Unselect()
        {
            foreach (var child in PlaylistsWrap.Children)
            {
                if (child is Border b)
                    b.BorderBrush = Brushes.Transparent;
            }

            selectedPlaylist = null;
            selectedBorder = null;
        }

        private void newPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagCreatePlaylist());
        }

        private void viewPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaylist == null)
            {
                MessageBox.Show("No playlist selected");
                return;
            }

            // FIXED HERE
            NavigationService.Navigate(new PagReadPlaylist(selectedPlaylist));
        }

        private void editPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaylist == null)
            {
                MessageBox.Show("No playlist selected");
                return;
            }

            NavigationService.Navigate(new PagUpdatePlaylist(selectedPlaylist));
        }

        private async void deletePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaylist == null)
            {
                MessageBox.Show("No playlist selected");
                return;
            }

            await _apiService.DeleteAsync($"/playlists/{selectedPlaylist.Id}");
            await LoadPlaylists();
        }

        private async void LoadPlaylists_Click(object sender, RoutedEventArgs e)
        {
            await LoadPlaylists();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}