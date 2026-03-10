using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Screens;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF
{
    public partial class PagRoleManagement : Page
    {
        private readonly ApiService _api = new ApiService();

        public PagRoleManagement()
        {
            InitializeComponent();
        }

        private async void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            RolesContainer.Children.Clear();

            var roles = await _api.GetAsync<List<RoleResponse>>("/roles");

            foreach (var role in roles)
            {
                RolesContainer.Children.Add(CreateRoleUI(role.Name));
            }
        }

        private UIElement CreateRoleUI(string name)
        {
            StackPanel stack = new StackPanel
            {
                Width = 120,
                Margin = new Thickness(10)
            };

            Grid circleGrid = new Grid { Width = 80, Height = 80 };

            Ellipse circle = new Ellipse
            {
                Fill = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Width = 80,
                Height = 80
            };

            TextBlock letter = new TextBlock
            {
                Text = name.Substring(0, 1).ToUpper(),
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            circleGrid.Children.Add(circle);
            circleGrid.Children.Add(letter);

            TextBlock label = new TextBlock
            {
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0),
                FontWeight = FontWeights.Bold
            };

            stack.Children.Add(circleGrid);
            stack.Children.Add(label);

            return stack;
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        private void OnCreateClicked(object sender, RoutedEventArgs e) { }
        private void OnViewClicked(object sender, RoutedEventArgs e) { }
        private void OnUpdateClicked(object sender, RoutedEventArgs e) { }
        private void OnDeleteClicked(object sender, RoutedEventArgs e) { }
    }
}