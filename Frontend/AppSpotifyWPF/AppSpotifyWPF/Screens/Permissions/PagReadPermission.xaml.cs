using System.Windows.Controls;
using AppSpotifyWPF.Classes;

namespace AppSpotifyWPF.Screens
{
    public partial class PagReadPermission : Page
    {
        public PagReadPermission(PermissionResponse p)
        {
            InitializeComponent();

            txtCode.Text = p.Code;
            txtName.Text = p.Name;
            txtDescription.Text = p.Description;
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagPermissionManagement());
        }
    }
}