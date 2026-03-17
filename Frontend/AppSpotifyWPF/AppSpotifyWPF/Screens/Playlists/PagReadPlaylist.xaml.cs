using AppSpotifyWPF.Classes;
using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Screens.Playlists;

namespace AppSpotifyWPF.Screens.Playlists
{
    public partial class PagReadPlaylist : Page
    {
        private Playlist _playlist;

        public PagReadPlaylist(Playlist playlist)
        {
            InitializeComponent();
            _playlist = playlist;

            txtName.Text = playlist.Name;
            txtDescription.Text = playlist.Description;
            txtImageUrl.Text = playlist.ImageUrl;
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PlaylistManagementPage());
        }
    }
}