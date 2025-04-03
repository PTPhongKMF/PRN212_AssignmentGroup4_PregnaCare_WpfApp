using DataAccessLayer.Entities;
using System.Windows;

namespace PregnaCare_WpfApp.Views
{
    public partial class MembershipPlanDialog : Window
    {
        public MembershipPlan MembershipPlan { get; private set; }

        public MembershipPlanDialog(MembershipPlan? plan = null)
        {
            InitializeComponent();
            MembershipPlan = plan ?? new MembershipPlan
            {
                Id = Guid.NewGuid(),
                PlanName = "",
                Price = 0,
                Duration = 0,
                Description = "",
                ImageUrl = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            if (plan != null)
            {
                txtPlanName.Text = plan.PlanName;
                txtPrice.Text = plan.Price.ToString();
                txtDuration.Text = plan.Duration.ToString();
                txtDescription.Text = plan.Description;
                txtImageUrl.Text = plan.ImageUrl;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlanName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên gói!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Vui lòng nhập thời hạn hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MembershipPlan.PlanName = txtPlanName.Text;
            MembershipPlan.Price = price;
            MembershipPlan.Duration = duration;
            MembershipPlan.Description = txtDescription.Text;
            MembershipPlan.ImageUrl = txtImageUrl.Text;
            MembershipPlan.UpdatedAt = DateTime.Now;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 