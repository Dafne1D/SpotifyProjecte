using AppSpotifyWPF.Models;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens.Songs
{
    public partial class PagCreateSong : Page
    {
        private readonly ApiService _apiService = new ApiService();

        public PagCreateSong()
        {
            InitializeComponent();
        }

        private async void CreateSong_Click(object sender, RoutedEventArgs e)
        {
            string title = txtName.Text;
            string artist = txtArtist.Text;
            string album = txtAlbum.Text;
            string genre = txtGenre.Text;

            if (title == null || artist == null)
            {
                MessageBox.Show("The title and the artist are mandatory");
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration))
            {
                MessageBox.Show("Please, introduce a valid duration (numbers only).");
                return;
            }


            var newSong = new Song
            {
                Title = title,
                Artist = artist,
                Album = album,
                Duration = duration,
                Genre = genre

            };

            try
            {
                var createdSong = await _apiService.PostAsync<User>("/songs", newSong);
                MessageBox.Show($"Song created! ID: {createdSong.Id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating song: {ex.Message}");
            }
            ClearBoxes();
        }

        private void ClearBoxes()
        {
            txtName.Clear();
            txtArtist.Clear();
            txtAlbum.Clear();
            txtDuration.Clear();
            txtGenre.Clear();
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
