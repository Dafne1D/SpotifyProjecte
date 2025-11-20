using AppSpotifyWPF.Classes;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF.Screens.Users
{
    public partial class ViewUserPage : Page
    {
        private User selectedUser;

        public ViewUserPage(User user)
        {
            InitializeComponent();
            selectedUser = user;

            lblName.Text = selectedUser.Username;
            lblEmail.Text = selectedUser.Email;

            lblRole.Text = "User";
            lblPermissions.Text = "Cap permís";
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
