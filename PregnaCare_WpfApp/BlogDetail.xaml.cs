using System;
using System.Windows;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace PregnaCare_WpfApp
{
    public partial class BlogDetail : Window
    {
        private readonly BlogService _blogService;
        private readonly Guid _blogId;

        public BlogDetail(Guid blogId)
        {
            InitializeComponent();
            _blogService = new BlogService();
            _blogId = blogId;
            LoadBlogDetails();
        }

        private void LoadBlogDetails()
        {
            Blog blog = _blogService.GetBlogById(_blogId);
            if (blog != null)
            {
                BlogTitle.Text = blog.PageTitle;
                BlogImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(blog.FeaturedImageUrl, UriKind.Absolute));
                BlogHeading.Text = blog.Heading;
                BlogShortDescription.Text = blog.ShortDescription;
                BlogContent.Text = blog.Content;
                BlogUrlHandle.Text = $"URL: {blog.UrlHandle}";
                BlogViewCount.Text = $"Lượt xem: {blog.ViewCount}";
                BlogCreatedAt.Text = $"Tạo lúc: {blog.CreatedAt}";
                BlogUpdatedAt.Text = $"Cập nhật: {blog.UpdatedAt}";
                BlogIsVisible.Text = blog.IsVisible.HasValue && blog.IsVisible.Value ? "Hiển thị: Có" : "Hiển thị: Không";
            }
            else
            {
                MessageBox.Show("Không tìm thấy bài viết!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BlogList blogList = new BlogList();
            blogList.Show();
            this.Close();
        }
    }
}
