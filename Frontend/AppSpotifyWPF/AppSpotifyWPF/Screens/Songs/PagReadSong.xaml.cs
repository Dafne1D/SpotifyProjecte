using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Screens;
using System;
using AppSpotifyWPF;
using AppSpotifyWPF.Classes;

namespace AppSpotifyWPF.Screens.Songs
{
    public partial class PagReadSong : Page
    {
        private SongResponse _song;

        public PagReadSong(SongResponse song)
        {
            InitializeComponent();
            _song = song;

            txtTitle.Text = song.Title;
            txtArtist.Text = song.Artist;
            txtAlbum.Text = song.Album;
            txtDuration.Text = song.Duration.ToString();
            txtGenre.Text = song.Genre;
            txtImageUrl.Text = song.ImageUrl;
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SongManagementPage());
        }
    }
}