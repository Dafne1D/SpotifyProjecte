using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Services;
using AppSpotifyWPF.Classes;

namespace AppSpotifyWPF.Screens
{
    public partial class PagCreatePermission : Page
    {
        private readonly ApiService _api = new ApiService();

        public PagCreatePermission()
        {
            InitializeComponent();
            LoadRoles();
        }

        private async void LoadRoles()
        {
            var roles = await _api.GetAsync<List<RoleResponse>>("/roles");

            RoleCombo.ItemsSource = roles;
            RoleCombo.DisplayMemberPath = "Name";
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var body = new
            {
                Code = txtCode.Text,
                Name = txtName.Text,
                Description = txtDescription.Text
            };

            // 1. CREATE PERMISSION
            var created = await _api.PostAsync<PermissionResponse>("/permissions", body);

            // 2. ASSIGN TO ROLE (if selected)
            if (RoleCombo.SelectedItem != null)
            {
                var role = (RoleResponse)RoleCombo.SelectedItem;

                var assignBody = new
                {
                    RoleId = role.Id,
                    PermissionId = created.Id
                };

                await _api.PostAsync<object>("/rolePermissions", assignBody);
            }

            MessageBox.Show("Permission created and assigned");

            NavigationService.Navigate(new PagPermissionManagement());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagPermissionManagement());
        }
    }
}