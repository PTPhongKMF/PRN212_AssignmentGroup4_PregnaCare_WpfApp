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
    /// Interaction logic for StaffRecordWindow.xaml
    /// </summary>
    public partial class StaffRecordWindow : Window
    {
        private readonly PregnancyRecordService _pregnancyRecordService;
       
        public StaffRecordWindow()
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
        // Tìm kiếm hồ sơ mang thai
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = searchTextBox.Text;
            var searchResults = _pregnancyRecordService.SearchPregnancyRecords(searchTerm);
            pregnancyRecordDataGrid.ItemsSource = searchResults;
        }
    }
}
