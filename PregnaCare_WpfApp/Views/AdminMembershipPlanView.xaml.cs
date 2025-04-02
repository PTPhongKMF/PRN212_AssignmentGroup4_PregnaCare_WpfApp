using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Windows;
using System.Windows.Controls;

namespace PregnaCare_WpfApp.Views
{
    public partial class AdminMembershipPlanView : Window
    {
        private readonly MembershipPlanService _membershipPlanService;
        private MembershipPlan? _selectedPlan;

        public AdminMembershipPlanView()
        {
            InitializeComponent();
            _membershipPlanService = new MembershipPlanService();
            LoadMembershipPlans();
        }

        private void LoadMembershipPlans()
        {
            dgMembershipPlans.ItemsSource = _membershipPlanService.GetAllPlans();
        }

        private void dgMembershipPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPlan = dgMembershipPlans.SelectedItem as MembershipPlan;
            btnEdit.IsEnabled = _selectedPlan != null;
            btnDelete.IsEnabled = _selectedPlan != null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MembershipPlanDialog();
            if (dialog.ShowDialog() == true)
            {
                if (_membershipPlanService.AddPlan(dialog.MembershipPlan))
                {
                    MessageBox.Show("Thêm gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMembershipPlans();
                }
                else
                {
                    MessageBox.Show("Thêm gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null) return;

            var dialog = new MembershipPlanDialog(_selectedPlan);
            if (dialog.ShowDialog() == true)
            {
                if (_membershipPlanService.UpdatePlan(dialog.MembershipPlan))
                {
                    MessageBox.Show("Cập nhật gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMembershipPlans();
                }
                else
                {
                    MessageBox.Show("Cập nhật gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null) return;

            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa gói membership này?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_membershipPlanService.DeletePlan(_selectedPlan.Id))
                {
                    MessageBox.Show("Xóa gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMembershipPlans();
                }
                else
                {
                    MessageBox.Show("Xóa gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
} 