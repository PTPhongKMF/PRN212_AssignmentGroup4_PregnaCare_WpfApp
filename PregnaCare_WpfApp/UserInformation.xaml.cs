using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using PregnaCare_WpfApp.Utils;
using PregnaCare_WpfApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PregnaCare_WpfApp {
    /// <summary>
    /// Interaction logic for UserInformation.xaml
    /// </summary>
    public partial class UserInformation : Window {
        private UserService _userService;
        private User? _currentUser;
        
        public UserInformation() {
            InitializeComponent();
            _userService = new UserService();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // Get user data from UserSession
            if (UserSession.Id != Guid.Empty) {
                _currentUser = _userService.GetUserById(UserSession.Id);
                
                // If user not found, handle the error
                if (_currentUser == null) {
                    MessageBox.Show("Unable to load user data. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                DisplayUserInfo();
                
                // Set visibility of buttons based on role
                bool isAdmin = UserSession.RoleName?.ToLower() == "admin";
                
                // Blog buttons
                BtnViewBlogs.Visibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
                BtnManageBlogs.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                TxtUserBlogDesc.Visibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
                TxtAdminBlogDesc.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                
                // Membership buttons
                BtnViewMembership.Visibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
                BtnManageMembership.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                TxtUserMembershipDesc.Visibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
                TxtAdminMembershipDesc.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            } else {
                MessageBox.Show("No active user session found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        
        private void DisplayUserInfo() {
            if (_currentUser == null) return;
            
            TxtFullName.Text = _currentUser.FullName ?? "Not set";
            TxtEmail.Text = _currentUser.Email ?? "Not set";
            TxtPhoneNumber.Text = _currentUser.PhoneNumber ?? "Not set";
            TxtGender.Text = _currentUser.Gender ?? "Not set";
            TxtDateOfBirth.Text = _currentUser.DateOfBirth?.ToString("dd/MM/yyyy") ?? "Not set";
            TxtAddress.Text = _currentUser.Address ?? "Not set";
            
            // Set user image if available
            if (!string.IsNullOrEmpty(_currentUser.ImageUrl)) {
                try {
                    UserImage.Source = new BitmapImage(new Uri(_currentUser.ImageUrl));
                } catch {
                    // Use default image if loading fails
                    UserImage.Source = new BitmapImage(new Uri("https://png.pngtree.com/png-vector/20190820/ourmid/pngtree-no-image-vector-illustration-isolated-png-image_1694547.jpg"));
                }
            } else {
                // No image URL, set default image
                UserImage.Source = new BitmapImage(new Uri("https://png.pngtree.com/png-vector/20190820/ourmid/pngtree-no-image-vector-illustration-isolated-png-image_1694547.jpg"));
            }
        }
        
        private void BtnEditInfo_Click(object sender, RoutedEventArgs e) {
            if (_currentUser == null) return;
            
            var editDialog = new UserInfoEditDialog(_currentUser);
            bool? result = editDialog.ShowDialog();
            
            if (result == true) {
                // Refresh user data after update
                _currentUser = _userService.GetUserById(_currentUser.Id);
                DisplayUserInfo();
                MessageBox.Show("User information updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e) {
            UserSession.Clear();
            
            Login loginWindow = new Login();
            loginWindow.Show();
            
            this.Close();
        }

        private void BtnViewMembership_Click(object sender, RoutedEventArgs e) {
            UserMembershipPlanView userMembership = new();
            userMembership.Show();
            this.Close();
        }
        
        private void BtnManageMembership_Click(object sender, RoutedEventArgs e) {
            AdminMembershipPlanView adminMembership = new AdminMembershipPlanView();
            adminMembership.Show();
            this.Close();
        }
        
        private void BtnViewBlogs_Click(object sender, RoutedEventArgs e) {
            BlogList blogList = new BlogList();
            blogList.Show();
            this.Close();
        }
        
        private void BtnManageBlogs_Click(object sender, RoutedEventArgs e) {
            BlogList blogList = new BlogList();
            blogList.Show();
            this.Close();
        }
    }
}
