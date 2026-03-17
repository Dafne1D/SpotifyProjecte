using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Screens;
using AppSpotifyWPF.Services;
using AppSpotifyWPF.Classes;
using System.Linq;

namespace AppSpotifyWPF
{
    public partial class PagRoleManagement : Page
    {
        private readonly ApiService _api = new ApiService();
        private Border? selectedBorder = null;
        private RoleResponse? selectedRole = null;

        public PagRoleManagement()
        {
            InitializeComponent();
        }

        private async void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            RolesContainer.Children.Clear();

            var roles = await _api.GetAsync<List<RoleResponse>>("/roles");

            foreach (var role in roles.OrderBy(r => r.Name))
            {
                RolesContainer.Children.Add(CreateRoleUI(role));
            }
        }

        private UIElement CreateRoleUI(RoleResponse role)
        {
            Grid avatar = new Grid
            {
                Width = 80,
                Height = 80,
                Margin = new Thickness(0, 0, 0, 5)
            };

            Ellipse circle = new Ellipse
            {
                Fill = Brushes.LightGray,
                Width = 80,
                Height = 80
            };

            TextBlock letter = new TextBlock
            {
                Text = role.Name.Substring(0, 1).ToUpper(),
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            avatar.Children.Add(circle);
            avatar.Children.Add(letter);

            TextBlock name = new TextBlock
            {
                Text = role.Name,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            Border border = new Border
            {
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(10),
                Width = 100,
                Child = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Children = { avatar, name }
                },
                Tag = role
            };

            border.MouseLeftButtonDown += Role_Click;

            return border;
        }

        private void Role_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is RoleResponse role)
            {
                if (selectedBorder == border)
                {
                    Unselect();
                    return;
                }

                Unselect();

                border.BorderBrush = Brushes.DeepSkyBlue;
                selectedBorder = border;
                selectedRole = role;
            }
        }

        private void Unselect()
        {
            foreach (var child in RolesContainer.Children)
            {
                if (child is Border b)
                    b.BorderBrush = Brushes.Transparent;
            }

            selectedBorder = null;
            selectedRole = null;
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}