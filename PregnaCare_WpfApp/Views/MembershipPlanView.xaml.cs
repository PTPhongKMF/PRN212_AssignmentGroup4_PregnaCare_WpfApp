using BusinessLogicLayer.Services;
using PregnaCare_WpfApp.Utils;
using System.Windows;
using System.Windows.Controls;

namespace PregnaCare_WpfApp.Views
{
    public partial class MembershipPlanView : Window
    {
        private readonly MembershipPlanService _membershipPlanService;
        private readonly UserMembershipPlanService _userMembershipPlanService;

        public MembershipPlanView()
        {
            InitializeComponent();
            _membershipPlanService = new MembershipPlanService();
            _userMembershipPlanService = new UserMembershipPlanService();
            LoadAvailablePlans();
        }

        private void LoadAvailablePlans()
        {
            itemsControl.ItemsSource = _userMembershipPlanService.GetAvailablePlans(UserSession.Id);
        }

        private void btnSubscribe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid planId)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng ký gói membership này?",
                    "Xác nhận đăng ký",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_membershipPlanService.SubscribeToPlan(UserSession.Id, planId))
                    {
                        MessageBox.Show("Đăng ký gói membership thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        var userMembershipView = new UserMembershipPlanView();
                        userMembershipView.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký gói membership thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
} 