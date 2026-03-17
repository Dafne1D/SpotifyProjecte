using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Services;
using AppSpotifyWPF.Classes;

namespace AppSpotifyWPF.Screens
{
    public partial class PagUpdatePermission : Page
    {
        private readonly ApiService _api = new ApiService();
        private PermissionResponse _p;

        public PagUpdatePermission(PermissionResponse p)
        {
            InitializeComponent();
            _p = p;

            txtName.Text = p.Name;
            txtDescription.Text = p.Description;
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            _p.Name = txtName.Text;
            _p.Description = txtDescription.Text;

            await _api.PutAsync($"/permissions/{_p.Id}", _p);

            MessageBox.Show("Permission updated");

            NavigationService.Navigate(new PagPermissionManagement());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagPermissionManagement());
        }
    }
}