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
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace PregnaCare_WpfApp
{
    /// <summary>
    /// Interaction logic for PregnancyRecordWindow.xaml
    /// </summary>
    public partial class PregnancyRecordWindow : Window
    {
        private readonly PregnancyRecordService _pregnancyRecordService;
        public PregnancyRecordWindow()
        {
            InitializeComponent();
            _pregnancyRecordService = new PregnancyRecordService();
        }
        // Lấy tất cả hồ sơ mang thai của người dùng và hiển thị
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userId = Guid.NewGuid(); // Chú ý: Bạn cần thay thế bằng UserId thực tế của người dùng đã đăng nhập
            var records = _pregnancyRecordService.GetAllPregnancyRecords(userId);
            pregnancyRecordDataGrid.ItemsSource = records;
        }

        // Thêm hồ sơ mang thai mới
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newRecord = new PregnancyRecord
            {
                UserId = Guid.NewGuid(),  // Thay bằng UserId thực tế của người dùng
                BabyName = babyNameTextBox.Text,
                PregnancyStartDate = DateOnly.FromDateTime(pregnancyStartDatePicker.SelectedDate.Value),
                ExpectedDueDate = DateOnly.FromDateTime(expectedDueDatePicker.SelectedDate.Value),
                BabyGender = (babyGenderComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                ImageUrl = imageUrlTextBox.Text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            var result = _pregnancyRecordService.AddPregnancyRecord(newRecord);
            if (result)
            {
                MessageBox.Show("Hồ sơ mang thai đã được thêm thành công!");
                Window_Loaded(sender, e);  // Refresh lại dữ liệu
            }
            else
            {
                MessageBox.Show("Thêm hồ sơ mang thai thất bại!");
            }
        }

        // Cập nhật hồ sơ mang thai
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (pregnancyRecordDataGrid.SelectedItem is PregnancyRecord selectedRecord)
            {
                selectedRecord.BabyName = babyNameTextBox.Text;
                selectedRecord.PregnancyStartDate = DateOnly.FromDateTime(pregnancyStartDatePicker.SelectedDate.Value);
                selectedRecord.ExpectedDueDate = DateOnly.FromDateTime(expectedDueDatePicker.SelectedDate.Value);
                selectedRecord.BabyGender = (babyGenderComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                selectedRecord.ImageUrl = imageUrlTextBox.Text;
                selectedRecord.UpdatedAt = DateTime.Now;
                var result = _pregnancyRecordService.UpdatePregnancyRecord(selectedRecord);
                if (result)
                {
                    MessageBox.Show("Hồ sơ mang thai đã được cập nhật thành công!");
                    Window_Loaded(sender, e);  // Refresh lại dữ liệu
                }
                else
                {
                    MessageBox.Show("Cập nhật hồ sơ mang thai thất bại!");
                }
            }
        }

        // Xóa hồ sơ mang thai
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (pregnancyRecordDataGrid.SelectedItem is PregnancyRecord selectedRecord)
            {
                var result = _pregnancyRecordService.DeletePregnancyRecord(selectedRecord.Id);
                if (result)
                {
                    MessageBox.Show("Hồ sơ mang thai đã được xóa!");
                    Window_Loaded(sender, e);  // Refresh lại dữ liệu
                }
                else
                {
                    MessageBox.Show("Xóa hồ sơ mang thai thất bại!");
                }
            }
        }

        // Tìm kiếm hồ sơ mang thai
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = searchTextBox.Text;
            var searchResults = _pregnancyRecordService.SearchPregnancyRecords(searchTerm);
            pregnancyRecordDataGrid.ItemsSource = searchResults;
        }
    }
}
