using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AppSpotifyWPF.Services;
using AppSpotifyWPF.Classes;
using System.Linq;
using System.Windows.Input;

namespace AppSpotifyWPF.Screens
{
    public partial class PagPermissionManagement : Page
    {
        private readonly ApiService _api = new ApiService();

        private PermissionResponse? selectedPermission = null;
        private Border? selectedBorder = null;

        public PagPermissionManagement()
        {
            InitializeComponent();
        }

        private async void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            PermissionsContainer.Children.Clear();

            var permissions = await _api.GetAsync<List<PermissionResponse>>("/permissions");

            foreach (var p in permissions.OrderBy(x => x.Name))
            {
                PermissionsContainer.Children.Add(CreateUI(p));
            }
        }

        private UIElement CreateUI(PermissionResponse p)
        {
            Grid avatar = new Grid { Width = 80, Height = 80 };

            avatar.Children.Add(new Ellipse
            {
                Fill = Brushes.LightGray,
                Width = 80,
                Height = 80
            });

            avatar.Children.Add(new TextBlock
            {
                Text = p.Name.Substring(0, 1).ToUpper(),
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            });

            Border border = new Border
            {
                Margin = new Thickness(10),
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(2),
                Tag = p,
                Child = new StackPanel
                {
                    Children =
                    {
                        avatar,
                        new TextBlock
                        {
                            Text = p.Name,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold
                        }
                    }
                }
            };

            border.MouseLeftButtonDown += Select;

            return border;
        }

        private void Select(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.Tag is PermissionResponse p)
            {
                foreach (var child in PermissionsContainer.Children)
                    if (child is Border bd)
                        bd.BorderBrush = Brushes.Transparent;

                b.BorderBrush = Brushes.DeepSkyBlue;

                selectedPermission = p;
                selectedBorder = b;
            }
        }

        private void OnCreateClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagCreatePermission());
        }

        private void OnViewClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPermission == null)
            {
                MessageBox.Show("Select a permission");
                return;
            }

            NavigationService.Navigate(new PagReadPermission(selectedPermission));
        }

        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPermission == null)
            {
                MessageBox.Show("Select a permission");
                return;
            }

            NavigationService.Navigate(new PagUpdatePermission(selectedPermission));
        }

        private async void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPermission == null)
            {
                MessageBox.Show("Select a permission");
                return;
            }

            await _api.DeleteAsync($"/permissions/{selectedPermission.Id}");

            OnLoadClicked(null, null);
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}