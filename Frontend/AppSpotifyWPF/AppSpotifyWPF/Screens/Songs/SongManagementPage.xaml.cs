using AppSpotifyWPF.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppSpotifyWPF;

namespace AppSpotifyWPF.Screens.Songs
{
    public class SongResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }
    }

    public partial class SongManagementPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private SongResponse? selectedSong = null;
        private Border? selectedBorder = null;

        public SongManagementPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSongs();
        }

        private async Task LoadSongs()
        {
            try
            {
                if (Session.CurrentUserId == Guid.Empty)
                {
                    MessageBox.Show("Click Force Admin first");
                    return;
                }

                var songs = await _apiService.GetAsync<List<SongResponse>>(
                    $"/songs?requesterId={Session.CurrentUserId}"
                );

                RenderSongs(songs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading songs:\n" + ex.Message);
            }
        }

        private async void LoadSongs_Click(object sender, RoutedEventArgs e)
        {
            await LoadSongs();
        }

        private void RenderSongs(IEnumerable<SongResponse> songs)
        {
            SongsWrap.Children.Clear();

            foreach (var song in songs.OrderBy(s => s.Title))
            {
                Ellipse avatarBackground = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.LightGray
                };

                TextBlock avatarIcon = new TextBlock
                {
                    Text = !string.IsNullOrEmpty(song.Title)
                        ? char.ToUpper(song.Title[0]).ToString()
                        : "?",
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

                TextBlock title = new TextBlock
                {
                    Text = song.Title,
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
                        Children = { avatar, title }
                    },
                    Tag = song
                };

                border.MouseLeftButtonDown += Song_Click;

                SongsWrap.Children.Add(border);
            }
        }

        private void Song_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is SongResponse song)
            {
                if (selectedBorder == border)
                {
                    Unselect();
                    return;
                }

                Unselect();

                border.BorderBrush = Brushes.DeepSkyBlue;
                selectedSong = song;
                selectedBorder = border;
            }
        }

        private void Unselect()
        {
            foreach (var child in SongsWrap.Children)
            {
                if (child is Border b)
                    b.BorderBrush = Brushes.Transparent;
            }

            selectedSong = null;
            selectedBorder = null;
        }

        private void NewSong_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagCreateSong());
        }

        private void ViewSong_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSong == null)
            {
                MessageBox.Show("No song selected!");
                return;
            }

            NavigationService.Navigate(new PagReadSong(selectedSong));
        }

        private void EditSong_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSong == null)
            {
                MessageBox.Show("No song selected!");
                return;
            }

            NavigationService.Navigate(new PagUpdateSong(selectedSong));
        }

        private async void DeleteSong_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSong == null)
            {
                MessageBox.Show("No song selected!");
                return;
            }

            var result = MessageBox.Show(
                $"Delete {selectedSong.Title}?",
                "Confirm",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                await _apiService.DeleteAsync(
                    $"/songs/{selectedSong.Id}?requesterId={Session.CurrentUserId}"
                );

                await LoadSongs();
            }
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}