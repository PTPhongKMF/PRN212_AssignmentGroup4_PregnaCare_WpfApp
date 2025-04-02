using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;

namespace PregnaCare_WpfApp
{
    public partial class BlogList : Window
    {
        private readonly BlogService _blogService;
        public ObservableCollection<Blog> Blogs { get; set; }
        public Blog SelectedBlog { get; set; }

        public BlogList()
        {
            InitializeComponent();
            _blogService = new BlogService();
            LoadBlogs();
            DataContext = this;
        }

        public void LoadBlogs()
        {
            var blogs = _blogService.GetAllBlogs();
            Blogs = new ObservableCollection<Blog>(blogs);
        }

        private void BlogItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (SelectedBlog != null)
            {
                BlogDetail detailWindow = new BlogDetail(SelectedBlog.Id);
                detailWindow.Show();
                this.Close();
            }
        }

        private void AddBlog_Click(object sender, RoutedEventArgs e)
        {
            AddBlogWindow addBlogWindow = new AddBlogWindow();
            addBlogWindow.ShowDialog();
            LoadBlogs(); // Refresh danh sách sau khi thêm bài viết mới
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
