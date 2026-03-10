using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AppSpotifyWPF.Services;

namespace AppSpotifyWPF.Screens
{
    public partial class PagUserRoleAssignment : Page
    {
        private readonly ApiService _api = new ApiService();

        public PagUserRoleAssignment()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            var users = await _api.GetAsync<List<UserResponse>>("/users");
            var roles = await _api.GetAsync<List<RoleResponse>>("/roles");

            UserCombo.ItemsSource = users;
            UserCombo.DisplayMemberPath = "Username";

            RoleCombo.ItemsSource = roles;
            RoleCombo.DisplayMemberPath = "Name";
        }

        private async void OnAssignClicked(object sender, RoutedEventArgs e)
        {
            if (UserCombo.SelectedItem == null || RoleCombo.SelectedItem == null)
            {
                MessageBox.Show("Select user and role");
                return;
            }

            var user = (UserResponse)UserCombo.SelectedItem;
            var role = (RoleResponse)RoleCombo.SelectedItem;

            var request = new
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _api.PostAsync<object>("/userRoles", request);

            MessageBox.Show("Role Assigned Successfully!");
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}