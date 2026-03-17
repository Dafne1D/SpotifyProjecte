using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Screens.Playlists;

namespace AppSpotifyWPF.Screens.Playlists
{
    public partial class PagUpdatePlaylist : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private Playlist _playlist;

        public PagUpdatePlaylist(Playlist playlist)
        {
            InitializeComponent();
            _playlist = playlist;

            txtName.Text = playlist.Name;
            txtDescription.Text = playlist.Description;
            txtImageUrl.Text = playlist.ImageUrl;
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name required");
                return;
            }

            _playlist.Name = txtName.Text;
            _playlist.Description = txtDescription.Text;
            _playlist.ImageUrl = txtImageUrl.Text;

            try
            {
                await _apiService.PutAsync($"/playlists/{_playlist.Id}", _playlist);

                MessageBox.Show(
                    $"Updated:\nName: {_playlist.Name}\nDescription: {_playlist.Description}\nImage: {_playlist.ImageUrl}"
                );

                NavigationService.Navigate(new PlaylistManagementPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PlaylistManagementPage());
        }
    }
}