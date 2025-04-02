using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace PregnaCare_WpfApp
{
    public partial class BlogList : Window
    {
        private readonly BlogService _blogService;
        private readonly TagService _tagService;
        
        // Collections for binding
        public ObservableCollection<Blog> Blogs { get; set; }
        public ObservableCollection<Blog> FilteredBlogs { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }
        
        // Selected objects
        public Blog SelectedBlog { get; set; }
        private Guid? _selectedTagId = null;
        private string _searchText = string.Empty;

        public BlogList()
        {
            InitializeComponent();
            _blogService = new BlogService();
            _tagService = new TagService();
            
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            // Load blogs
            var blogs = _blogService.GetAllBlogs();
            Blogs = new ObservableCollection<Blog>(blogs);
            FilteredBlogs = new ObservableCollection<Blog>(blogs);
            
            // Load tags with "All" option
            var allTags = _tagService.GetAllTags();
            
            // Add "All" tag at the beginning
            var allTag = new Tag { Id = Guid.Empty, Name = "All Tags" };
            Tags = new ObservableCollection<Tag>();
            Tags.Add(allTag);
            
            foreach (var tag in allTags)
            {
                Tags.Add(tag);
            }
            
            // Set default selection to "All Tags"
            CmbTagFilter.SelectedIndex = 0;
        }
        
        private void ApplyFilters()
        {
            IEnumerable<Blog> result = Blogs;
            
            // Apply tag filter if a tag is selected
            if (_selectedTagId.HasValue && _selectedTagId.Value != Guid.Empty)
            {
                result = result.Where(b => 
                    b.BlogTags.Any(bt => bt.TagId == _selectedTagId.Value && bt.IsDeleted != true));
            }
            
            // Apply search text filter
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                string searchLower = _searchText.ToLower();
                result = result.Where(b => 
                    b.PageTitle.ToLower().Contains(searchLower) || 
                    (b.ShortDescription != null && b.ShortDescription.ToLower().Contains(searchLower)));
            }
            
            // Update filtered blogs
            FilteredBlogs.Clear();
            foreach (var blog in result)
            {
                FilteredBlogs.Add(blog);
            }
        }

        private void TxtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _searchText = TxtSearch.Text;
            ApplyFilters();
        }

        private void CmbTagFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CmbTagFilter.SelectedItem is Tag selectedTag)
            {
                _selectedTagId = selectedTag.Id == Guid.Empty ? null : selectedTag.Id;
                ApplyFilters();
            }
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
            
            // Refresh the blog list
            LoadData();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
