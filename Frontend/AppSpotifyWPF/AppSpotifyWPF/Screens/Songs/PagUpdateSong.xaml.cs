using AppSpotifyWPF.Screens;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AppSpotifyWPF.Screens.Songs
{
    public partial class PagUpdateSong : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private SongResponse _song;

        public PagUpdateSong(SongResponse song)
        {
            InitializeComponent();
            _song = song;

            txtName.Text = song.Title;
            txtArtist.Text = song.Artist;
            txtAlbum.Text = song.Album;
            txtDuration.Text = song.Duration.ToString();
            txtGenre.Text = song.Genre;
            txtImageUrl.Text = song.ImageUrl;
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            _song.Title = txtName.Text;
            _song.Artist = txtArtist.Text;
            _song.Album = txtAlbum.Text;
            _song.Duration = int.Parse(txtDuration.Text);
            _song.Genre = txtGenre.Text;
            _song.ImageUrl = txtImageUrl.Text;

            await _apiService.PutAsync(
                $"/songs/{_song.Id}?requesterId={Session.CurrentUserId}",
                _song
            );

            MessageBox.Show("Song updated");

            NavigationService.Navigate(new SongManagementPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SongManagementPage());
        }
    }
}