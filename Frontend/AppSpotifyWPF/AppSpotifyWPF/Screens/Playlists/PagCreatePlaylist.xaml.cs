using AppSpotifyWPF;
using AppSpotifyWPF.Classes;
using AppSpotifyWPF.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Screens.Playlists;

namespace AppSpotifyWPF.Screens.Playlists
{
    public partial class PagCreatePlaylist : Page
    {
        private readonly ApiService _apiService = new ApiService();

        public PagCreatePlaylist()
        {
            InitializeComponent();
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string description = txtDescription.Text;
            string imageUrl = txtImageUrl.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name required");
                return;
            }

            var playlist = new Playlist
            {
                UserId = Session.CurrentUserId,
                Name = name,
                Description = description,
                ImageUrl = imageUrl
            };

            try
            {
                var created = await _apiService.PostAsync<Playlist>("/playlists", playlist);

                MessageBox.Show(
                    $"Created:\nName: {created.Name}\nDescription: {created.Description}\nImage: {created.ImageUrl}"
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