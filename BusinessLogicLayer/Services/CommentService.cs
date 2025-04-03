using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CommentService
    {
        private readonly CommentRepository _commentRepository;
        private readonly UserService _userService;

        public CommentService()
        {
            _commentRepository = new CommentRepository();
            _userService = new UserService();
        }

        public List<Comment> GetCommentsByBlogId(Guid blogId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"CommentService: Getting comments for blog ID {blogId}");
                
                // Create a fresh repository instance for this operation
                var freshRepository = new CommentRepository();
                
                var comments = freshRepository.GetCommentsByBlogId(blogId)
                    .Where(c => c.IsDeleted == false || c.IsDeleted == null)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"CommentService: Found {comments.Count} comments");

                // Load user information for each comment
                foreach (var comment in comments)
                {
                    try
                    {
                        if (comment.UserId != Guid.Empty)
                        {
                            // Create a fresh user service instance
                            var freshUserService = new UserService();
                            comment.User = freshUserService.GetUserById(comment.UserId);
                            
                            if (comment.User == null)
                            {
                                System.Diagnostics.Debug.WriteLine($"CommentService: Warning - User not found for comment ID {comment.Id}, User ID {comment.UserId}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"CommentService: Error loading user for comment {comment.Id}: {ex.Message}");
                    }
                }

                return comments;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CommentService: Exception in GetCommentsByBlogId: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Comment>(); // Return empty list instead of throwing exception
            }
        }

        public bool AddComment(Comment comment)
        {
            return _commentRepository.AddComment(comment);
        }

        public bool UpdateComment(Guid commentId, string newText)
        {
            return _commentRepository.UpdateComment(commentId, newText);
        }

        public bool DeleteComment(Guid commentId)
        {
            return _commentRepository.DeleteComment(commentId);
        }
    }
} 