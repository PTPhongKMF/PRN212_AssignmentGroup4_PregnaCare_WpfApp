using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using Microsoft.Win32;
using PregnaCare_WpfApp.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PregnaCare_WpfApp
{
    public partial class AddBlogWindow : Window
    {
        private readonly BlogService _blogService;

        public AddBlogWindow()
        {
            InitializeComponent();
            _blogService = new BlogService();
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedFilePath);
                string saveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedImages");

                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                string savedFilePath = Path.Combine(saveDirectory, fileName);

                try
                {
                    File.Copy(selectedFilePath, savedFilePath, true);
                    txtFeaturedImageUrl.Text = savedFilePath; // Lưu đường dẫn ảnh vào TextBox ẩn

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(savedFilePath, UriKind.Absolute);
                    bitmap.EndInit();
                    imgPreview.Source = bitmap; // Hiển thị ảnh preview
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPageTitle.Text) ||
                    string.IsNullOrWhiteSpace(txtHeading.Text) ||
                    string.IsNullOrWhiteSpace(txtContent.Text))
                {
                    MessageBox.Show("Page Title, Heading, and Content are required fields.",
                                  "Validation Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                Guid userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    MessageBox.Show("Invalid User. Please log in again.",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    return;
                }

                var newBlog = new Blog
                {
                    PageTitle = txtPageTitle.Text,
                    Heading = txtHeading.Text,
                    ShortDescription = txtShortDescription.Text,
                    Content = txtContent.Text,
                    FeaturedImageUrl = txtFeaturedImageUrl.Text, // Đường dẫn ảnh đã chọn
                    IsVisible = chkIsVisible.IsChecked ?? true
                };

                bool success = _blogService.AddBlog(newBlog, userId);

                if (success)
                {
                    MessageBox.Show("Blog post added successfully!",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    BlogList blogList = new BlogList();
                    blogList.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add blog post.",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nInner Exception: {ex.InnerException?.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
        // Hàm giả định lấy UserId từ user hiện tại
        private Guid GetCurrentUserId()
        {
            // TODO: Lấy UserId từ session hoặc database
            return UserSession.Id;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            BlogList blogList = new BlogList();
            blogList.Show();
            this.Close();
        }
    }
}