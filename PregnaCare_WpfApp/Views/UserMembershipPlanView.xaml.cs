using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using PregnaCare_WpfApp.Utils;
using System.Windows;
using System.Windows.Controls;

namespace PregnaCare_WpfApp.Views
{
    public partial class UserMembershipPlanView : Window
    {
        private readonly UserMembershipPlanService _userMembershipPlanService;
        private UserMembershipPlan? _selectedPlan;

        public UserMembershipPlanView()
        {
            InitializeComponent();
            _userMembershipPlanService = new UserMembershipPlanService();
            LoadUserMembershipPlans();
        }

        private void LoadUserMembershipPlans()
        {
            dgUserMembershipPlans.ItemsSource = _userMembershipPlanService.GetUserMembershipPlans(UserSession.Id);
        }

        private void dgUserMembershipPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPlan = dgUserMembershipPlans.SelectedItem as UserMembershipPlan;
            btnExtend.IsEnabled = _selectedPlan != null && _selectedPlan.IsActive == true;
            btnDeactivate.IsEnabled = _selectedPlan != null && _selectedPlan.IsActive == true;
        }

        private void btnExtend_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null) return;

            var dialog = new ExtendMembershipDialog();
            if (dialog.ShowDialog() == true)
            {
                if (_userMembershipPlanService.ExtendMembershipPlan(_selectedPlan.Id, dialog.Days))
                {
                    MessageBox.Show("Gia hạn gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserMembershipPlans();
                }
                else
                {
                    MessageBox.Show("Gia hạn gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPlan == null) return;

            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn vô hiệu hóa gói membership này?",
                "Xác nhận vô hiệu hóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_userMembershipPlanService.DeactivateMembershipPlan(_selectedPlan.Id))
                {
                    MessageBox.Show("Vô hiệu hóa gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserMembershipPlans();
                }
                else
                {
                    MessageBox.Show("Vô hiệu hóa gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnViewAvailablePlans_Click(object sender, RoutedEventArgs e)
        {
            var membershipPlanView = new MembershipPlanView();
            membershipPlanView.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var userInformationView = new UserInformation();
            userInformationView.Show();
            this.Close();
        }
    }
} 