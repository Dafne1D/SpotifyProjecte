using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AppSpotifyWPF
{
    public partial class PagRoleManagement : Page
    {
        public PagRoleManagement()
        {
            InitializeComponent();
        }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            RolesContainer.Children.Clear();
            // Mock data - replace with your PermissionADO.GetAll call
            var roles = new List<string> { "admin", "editor", "viewer", "test" };

            foreach (var role in roles)
            {
                RolesContainer.Children.Add(CreateRoleUI(role));
            }
        }

        private UIElement CreateRoleUI(string name)
        {
            StackPanel stack = new StackPanel { Width = 120, Margin = new Thickness(10) };

            // Create the Circle (Ellipse in WPF)
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

            // Click detection
            stack.MouseDown += (s, e) => {
                // Clear other selections
                foreach (StackPanel child in RolesContainer.Children)
                    ((Ellipse)((Grid)child.Children[0]).Children[0]).Fill = new SolidColorBrush(Color.FromRgb(224, 224, 224));

                circle.Fill = Brushes.LightBlue;
            };

            return stack;
        }

        private void OnBackClicked(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void OnCreateClicked(object sender, RoutedEventArgs e) { /* Navigate to Create Page */ }
        private void OnViewClicked(object sender, RoutedEventArgs e) { /* Logic */ }
        private void OnUpdateClicked(object sender, RoutedEventArgs e) { /* Logic */ }
        private void OnDeleteClicked(object sender, RoutedEventArgs e) { /* Logic */ }
    }
}