using System;
using System.Collections.ObjectModel;
using System.Windows;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using PregnaCare_WpfApp.Utils;

namespace PregnaCare_WpfApp
{
    public partial class BlogDetail : Window
    {
        private readonly BlogService _blogService;
        private readonly CommentService _commentService;
        private readonly TagService _tagService;
        private readonly Guid _blogId;
        
        public ObservableCollection<Comment> Comments { get; set; }
        private Blog _currentBlog;

        public BlogDetail(Guid blogId)
        {
            InitializeComponent();
            _blogService = new BlogService();
            _commentService = new CommentService();
            _tagService = new TagService();
            _blogId = blogId;
            
            // Initialize comments collection
            Comments = new ObservableCollection<Comment>();
            
            // Set DataContext for bindings
            DataContext = this;
            
            LoadBlogDetails();
            LoadComments();
            LoadTags();
        }

        private void LoadBlogDetails()
        {
            _currentBlog = _blogService.GetBlogById(_blogId);
            if (_currentBlog != null)
            {
                BlogTitle.Text = _currentBlog.PageTitle;
                
                try
                {
                    if (!string.IsNullOrEmpty(_currentBlog.FeaturedImageUrl))
                    {
                        BlogImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_currentBlog.FeaturedImageUrl, UriKind.Absolute));
                    }
                }
                catch (Exception ex)
                {
                    // Handle image loading error
                    System.Diagnostics.Debug.WriteLine($"Error loading image: {ex.Message}");
                }
                
                BlogHeading.Text = _currentBlog.Heading;
                BlogShortDescription.Text = _currentBlog.ShortDescription;
                BlogContent.Text = _currentBlog.Content;
                BlogUrlHandle.Text = $"URL: {_currentBlog.UrlHandle}";
                BlogViewCount.Text = $"Views: {_currentBlog.ViewCount}";
                BlogCreatedAt.Text = $"Created: {_currentBlog.CreatedAt?.ToString("dd/MM/yyyy HH:mm")}";
                BlogUpdatedAt.Text = $"Updated: {_currentBlog.UpdatedAt?.ToString("dd/MM/yyyy HH:mm")}";
                BlogIsVisible.Text = _currentBlog.IsVisible.HasValue && _currentBlog.IsVisible.Value ? "Visible: Yes" : "Visible: No";
                
                // Increment the view count
                IncrementViewCount();
            }
            else
            {
                MessageBox.Show("Blog post not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        
        private void LoadComments()
        {
            var blogComments = _commentService.GetCommentsByBlogId(_blogId);
            Comments.Clear();
            
            foreach (var comment in blogComments)
            {
                Comments.Add(comment);
            }
        }
        
        private void LoadTags()
        {
            var tags = _tagService.GetTagsByBlogId(_blogId);
            BlogTags.ItemsSource = _currentBlog.BlogTags;
        }
        
        private void IncrementViewCount()
        {
            if (_currentBlog != null)
            {
                _blogService.IncrementViewCount(_blogId);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BlogList blogList = new BlogList();
            blogList.Show();
            this.Close();
        }
        
        private void PostComment_Click(object sender, RoutedEventArgs e)
        {
            // Check if user is logged in
            if (UserSession.Id == Guid.Empty)
            {
                MessageBox.Show("Please log in to post a comment.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            // Validate comment
            string commentText = TxtComment.Text.Trim();
            if (string.IsNullOrWhiteSpace(commentText))
            {
                MessageBox.Show("Please enter a comment.", "Empty Comment", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Create and save the comment
            var comment = new Comment
            {
                BlogId = _blogId,
                UserId = UserSession.Id,
                CommentText = commentText,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };
            
            bool success = _commentService.AddComment(comment);
            
            if (success)
            {
                // Clear the comment textbox
                TxtComment.Text = string.Empty;
                
                // Reload comments
                LoadComments();
                
                MessageBox.Show("Your comment has been posted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to post your comment. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
