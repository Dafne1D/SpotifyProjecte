using AppSpotifyWPF;
using AppSpotifyWPF.Classes;
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
            string imageUrl = txtImageUrl.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist))
            {
                MessageBox.Show("Title and Artist are required");
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration))
            {
                MessageBox.Show("Duration must be a number");
                return;
            }

            var newSong = new Song
            {
                Title = title,
                Artist = artist,
                Album = album,
                Duration = duration,
                Genre = genre,
                ImageUrl = imageUrl
            };

            try
            {
                var createdSong = await _apiService.PostAsync<Song>(
                    $"/songs?requesterId={Session.CurrentUserId}",
                    newSong
                );

                MessageBox.Show(
                    $"Created:\nTitle: {createdSong.Title}\nArtist: {createdSong.Artist}"
                );

                ClearBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }

        private void ClearBoxes()
        {
            txtName.Clear();
            txtArtist.Clear();
            txtAlbum.Clear();
            txtDuration.Clear();
            txtGenre.Clear();
            txtImageUrl.Clear();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}