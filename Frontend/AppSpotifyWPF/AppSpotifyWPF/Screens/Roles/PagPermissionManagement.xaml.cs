using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF.Screens
{
    public partial class PagPermissionManagement : Page
    {
        private readonly ApiService _api = new ApiService();

        public PagPermissionManagement()
        {
            InitializeComponent();
        }

        private async void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            PermissionsContainer.Children.Clear();

            var permissions = await _api.GetAsync<List<PermissionResponse>>("/permissions");

            foreach (var p in permissions)
            {
                StackPanel st = new StackPanel { Margin = new Thickness(10) };

                Ellipse el = new Ellipse
                {
                    Width = 60,
                    Height = 60,
                    Fill = Brushes.LightGray
                };

                st.Children.Add(el);

                st.Children.Add(new TextBlock
                {
                    Text = p.Name,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                PermissionsContainer.Children.Add(st);
            }
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        private void OnCreateClicked(object sender, RoutedEventArgs e) { }
        private void OnViewClicked(object sender, RoutedEventArgs e) { }
        private void OnEditClicked(object sender, RoutedEventArgs e) { }
        private void OnDeleteClicked(object sender, RoutedEventArgs e) { }
    }
}