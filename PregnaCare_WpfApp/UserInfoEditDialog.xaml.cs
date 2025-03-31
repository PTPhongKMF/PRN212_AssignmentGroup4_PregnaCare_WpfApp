using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
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

namespace PregnaCare_WpfApp
{
    /// <summary>
    /// Interaction logic for UserInfoEditDialog.xaml
    /// </summary>
    public partial class UserInfoEditDialog : Window
    {
        private readonly UserService _userService;
        private readonly User _user;

        public UserInfoEditDialog(User user)
        {
            InitializeComponent();
            _userService = new UserService();
            _user = user;
            
            this.Loaded += UserInfoEditDialog_Loaded;
        }

        private void UserInfoEditDialog_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (_user == null) return;
            
            TxtFullName.Text = _user.FullName ?? string.Empty;
            TxtEmail.Text = _user.Email ?? string.Empty;
            TxtPhoneNumber.Text = _user.PhoneNumber ?? string.Empty;
            
            // Set gender
            if (!string.IsNullOrEmpty(_user.Gender))
            {
                CmbGender.SelectedIndex = _user.Gender switch
                {
                    "Male" => 0,
                    "Female" => 1,
                    "Other" => 2,
                    _ => -1
                };
            }
            
            // Set date of birth
            if (_user.DateOfBirth.HasValue)
            {
                DpDateOfBirth.SelectedDate = _user.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }
            
            TxtAddress.Text = _user.Address ?? string.Empty;
            TxtImageUrl.Text = _user.ImageUrl ?? string.Empty;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate fields
            if (string.IsNullOrWhiteSpace(TxtFullName.Text))
            {
                MessageBox.Show("Full name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtFullName.Focus();
                return;
            }

            // Update user information
            _user.FullName = TxtFullName.Text.Trim();
            _user.PhoneNumber = TxtPhoneNumber.Text.Trim();
            
            // Get gender from combobox
            if (CmbGender.SelectedItem is ComboBoxItem selectedGender)
            {
                _user.Gender = selectedGender.Content.ToString();
            }
            
            // Get date of birth
            if (DpDateOfBirth.SelectedDate.HasValue)
            {
                _user.DateOfBirth = DateOnly.FromDateTime(DpDateOfBirth.SelectedDate.Value);
            }
            
            _user.Address = TxtAddress.Text.Trim();
            _user.ImageUrl = TxtImageUrl.Text.Trim();
            
            // Update user in database
            bool success = _userService.UpdateUser(_user);
            
            if (success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to update user information. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 