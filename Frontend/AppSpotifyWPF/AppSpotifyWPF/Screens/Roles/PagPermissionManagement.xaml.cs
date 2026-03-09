using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AppSpotifyWPF.Screens
{
    public partial class PagPermissionManagement : Page
    {
        public PagPermissionManagement() { InitializeComponent(); }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            PermissionsContainer.Children.Clear();
            // Test Data
            var perms = new List<string> { "Read", "Write", "Delete", "Admin" };
            foreach (var p in perms)
            {
                StackPanel st = new StackPanel { Margin = new Thickness(10) };
                Ellipse el = new Ellipse { Width = 60, Height = 60, Fill = Brushes.LightGray };
                st.Children.Add(el);
                st.Children.Add(new TextBlock { Text = p, HorizontalAlignment = HorizontalAlignment.Center });
                PermissionsContainer.Children.Add(st);
            }
        }

        private void OnBackClicked(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}