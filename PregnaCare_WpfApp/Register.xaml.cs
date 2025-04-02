using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.Win32;
using PregnaCare_WpfApp.Utils;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private readonly UserService _userService;
        private string _imagePath = null;
        public Register()
        {
            _userService = new UserService();
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ các field
            string fullName = txtFullName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string phoneNumber = txtPhoneNumber.Text;
            string gender = cmbGender.Text;
            DateOnly? dateOfBirth = dpDateOfBirth.SelectedDate.HasValue
            ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value)
            : null;
            string address = txtAddress.Text;
            string imageUrl = _imagePath;

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(gender) ||
                dateOfBirth == null ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please fill in all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password, // Cần mã hóa trước khi lưu
                PhoneNumber = phoneNumber,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Address = address,
                ImageUrl = imageUrl
            };
            bool isAdded = _userService.AddUser(newUser);
            if (isAdded)
            {
                MessageBox.Show("Registration Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                var useraccount = _userService.GetUser(email, password);
                if (useraccount != null)
                {
                    UserSession.Id = useraccount.Id;
                    UserInformation userInformation = new UserInformation();
                    userInformation.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("You have no permission to access this function");
                }
            }
            else
            {
                MessageBox.Show("Failed to register. Email may already be in use.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Profile Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                imgProfile.Source = new BitmapImage(new Uri(_imagePath));
            }
        }
    }
}
