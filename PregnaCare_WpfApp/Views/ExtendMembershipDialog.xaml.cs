using System.Windows;

namespace PregnaCare_WpfApp.Views
{
    public partial class ExtendMembershipDialog : Window
    {
        public int Days { get; private set; }

        public ExtendMembershipDialog()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtDays.Text, out int days) || days <= 0)
            {
                MessageBox.Show("Vui lòng nhập số ngày hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Days = days;
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