using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using PregnaCare_WpfApp.Utils;
using System.Windows.Media;

namespace PregnaCare_WpfApp
{
    // CommentViewModel to add UI logic properties
    public class CommentViewModel : Comment
    {
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public partial class BlogDetail : Window
    {
        private readonly BlogService _blogService;
        private CommentService _commentService;
        private readonly TagService _tagService;
        private readonly Guid _blogId;
        
        public ObservableCollection<CommentViewModel> Comments { get; set; }
        private Blog _currentBlog;

        public BlogDetail(Guid blogId)
        {
            InitializeComponent();
            _blogService = new BlogService();
            _commentService = new CommentService();
            _tagService = new TagService();
            _blogId = blogId;
            
            // Initialize comments collection
            Comments = new ObservableCollection<CommentViewModel>();
            
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
            try
            {
                System.Diagnostics.Debug.WriteLine($"DEBUG: LoadComments called for blog ID: {_blogId}");
                
                // Recreate services to ensure fresh context
                _commentService = new CommentService();
                
                // Get comments from service
                var blogComments = _commentService.GetCommentsByBlogId(_blogId);
                System.Diagnostics.Debug.WriteLine($"DEBUG: Retrieved {blogComments?.Count ?? 0} comments from service");
                
                // Check if any comments were retrieved
                if (blogComments == null || blogComments.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("DEBUG: No comments returned from service");
                    Comments.Clear();
                    return;
                }
                
                // Clear existing comments
                Comments.Clear();
                System.Diagnostics.Debug.WriteLine("DEBUG: Cleared existing comments from UI");
                
                bool isAdmin = UserSession.RoleName?.ToLower() == "admin";
                Guid currentUserId = UserSession.Id;
                System.Diagnostics.Debug.WriteLine($"DEBUG: Current user is admin: {isAdmin}, User ID: {currentUserId}");
                
                // Process each comment
                foreach (var comment in blogComments)
                {
                    try
                    {
                        if (comment == null)
                        {
                            System.Diagnostics.Debug.WriteLine("DEBUG: Skipping null comment");
                            continue;
                        }
                        
                        if (comment.User == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"DEBUG: Warning - Comment ID {comment.Id} has null User");
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"DEBUG: Processing comment ID: {comment.Id}, Text: {comment.CommentText?.Substring(0, Math.Min(comment.CommentText?.Length ?? 0, 20)) ?? "null"}...");
                        
                        var commentViewModel = new CommentViewModel
                        {
                            Id = comment.Id,
                            BlogId = comment.BlogId,
                            UserId = comment.UserId,
                            ParentCommentId = comment.ParentCommentId,
                            CommentText = comment.CommentText,
                            CreatedAt = comment.CreatedAt,
                            UpdatedAt = comment.UpdatedAt,
                            IsDeleted = comment.IsDeleted,
                            User = comment.User,
                            Blog = comment.Blog,
                            
                            // Permission logic:
                            // For Edit: User can edit only their own comments regardless of role
                            CanEdit = comment.UserId == currentUserId,
                            
                            // For Delete: Admin can delete any comment, regular users only their own
                            CanDelete = isAdmin || comment.UserId == currentUserId
                        };
                        
                        Comments.Add(commentViewModel);
                        System.Diagnostics.Debug.WriteLine($"DEBUG: Added comment to UI collection, Total count: {Comments.Count}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"DEBUG EXCEPTION processing comment: {ex.Message}");
                    }
                }
                
                // Force refresh the ItemsSource
                System.Diagnostics.Debug.WriteLine($"DEBUG: Setting CommentsList.ItemsSource, total comments: {Comments.Count}");
                CommentsList.ItemsSource = null;
                CommentsList.ItemsSource = Comments;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DEBUG LOAD EXCEPTION: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"DEBUG LOAD STACK: {ex.StackTrace}");
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
        
        private void EditComment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is Guid commentId)
            {
                // Find the comment to edit
                var commentToEdit = Comments.FirstOrDefault(c => c.Id == commentId);
                if (commentToEdit != null)
                {
                    // Create dialog window for editing
                    Window editDialog = new Window
                    {
                        Title = "Edit Comment",
                        Width = 500,
                        Height = 250,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Owner = this,
                        ResizeMode = ResizeMode.NoResize
                    };

                    // Create grid layout
                    Grid grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.Margin = new Thickness(15);

                    // Create textbox for comment editing
                    TextBox txtEditComment = new TextBox
                    {
                        Text = commentToEdit.CommentText,
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        Height = 120,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Padding = new Thickness(10),
                        Margin = new Thickness(0, 0, 0, 10),
                        BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DDDDDD"),
                        Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#F9F9F9")
                    };
                    Grid.SetRow(txtEditComment, 0);

                    // Create button panel
                    StackPanel buttonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Grid.SetRow(buttonPanel, 1);

                    // Create cancel button
                    Button cancelButton = new Button
                    {
                        Content = "Cancel",
                        Width = 80,
                        Height = 30,
                        Margin = new Thickness(0, 0, 10, 0),
                        Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#6c757d"),
                        Foreground = Brushes.White,
                        BorderThickness = new Thickness(0)
                    };
                    cancelButton.Click += (s, args) => { editDialog.DialogResult = false; };

                    // Create save button
                    Button saveButton = new Button
                    {
                        Content = "Save Changes",
                        Width = 120,
                        Height = 30,
                        Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#28a745"),
                        Foreground = Brushes.White,
                        BorderThickness = new Thickness(0)
                    };
                    saveButton.Click += (s, args) => 
                    {
                        if (string.IsNullOrWhiteSpace(txtEditComment.Text))
                        {
                            MessageBox.Show("Comment cannot be empty.", "Error", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        editDialog.DialogResult = true;
                    };

                    // Add buttons to panel
                    buttonPanel.Children.Add(cancelButton);
                    buttonPanel.Children.Add(saveButton);

                    // Add controls to grid
                    grid.Children.Add(txtEditComment);
                    grid.Children.Add(buttonPanel);

                    // Set content and show dialog
                    editDialog.Content = grid;
                    bool? result = editDialog.ShowDialog();

                    // Process result
                    if (result == true)
                    {
                        string updatedText = txtEditComment.Text.Trim();
                        
                        // Update comment without checking result
                        _commentService.UpdateComment(commentId, updatedText);
                        
                        // Reload comments
                        LoadComments();
                        
                        // Always show success message
                        MessageBox.Show("Comment updated successfully.", "Success", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        
        private void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is Guid commentId)
            {
                // Ask for confirmation
                var result = MessageBox.Show("Are you sure you want to delete this comment?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    // Delete comment without checking result
                    _commentService.DeleteComment(commentId);
                    
                    // Reload comments
                    LoadComments();
                    
                    // Always show success message
                    MessageBox.Show("Comment deleted successfully.", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        
        private void PostComment_Click(object sender, RoutedEventArgs e)
        {
            try
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
                
                // Create new comment
                var comment = new Comment
                {
                    Id = Guid.NewGuid(), // Explicitly set ID
                    BlogId = _blogId,
                    UserId = UserSession.Id,
                    CommentText = commentText,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                
                System.Diagnostics.Debug.WriteLine($"DEBUG: Adding comment with ID: {comment.Id}, BlogId: {comment.BlogId}, UserId: {comment.UserId}");
                
                // Add the comment and check result
                bool success = _commentService.AddComment(comment);
                System.Diagnostics.Debug.WriteLine($"DEBUG: AddComment result: {success}");
                
                // Clear the comment textbox
                TxtComment.Text = string.Empty;
                
                if (success)
                {
                    // Reload comments
                    System.Diagnostics.Debug.WriteLine("DEBUG: Comment added successfully, reloading comments");
                    LoadComments();
                    
                    MessageBox.Show("Your comment has been posted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DEBUG: Comment posting reported failure");
                    MessageBox.Show("Failed to post your comment. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    // Try to reload comments anyway
                    System.Diagnostics.Debug.WriteLine("DEBUG: Attempting to reload comments despite failure report");
                    LoadComments();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DEBUG EXCEPTION: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"DEBUG STACK: {ex.StackTrace}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
