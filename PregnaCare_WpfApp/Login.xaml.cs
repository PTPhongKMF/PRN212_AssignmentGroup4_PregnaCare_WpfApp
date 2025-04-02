using BusinessLogicLayer.Services;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UserService _userService;
        public Login()
        {
            _userService = new UserService();
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var useraccount = _userService.GetUser(email, password);
                if (useraccount != null)
                {
                    MessageBox.Show("Login successful");
                    UserSession.Id = useraccount.Id;
                    //MainWindow mainWindow = new MainWindow();
                    //UserInformation userInformation = new UserInformation();
                    //mainWindow.Show();
                    //userInformation.Show();
                    BlogList blogList = new BlogList();
                    blogList.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("You have no permission to access this function");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RegisterLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Register registerWindow = new Register(); // Tạo cửa sổ đăng ký
            registerWindow.Show(); // Mở cửa sổ đăng ký
            this.Close(); // Đóng cửa sổ đăng nhập
        }
    }
}
