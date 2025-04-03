using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class CommentRepository
    {
        private readonly PregnaCareAppDbContext _context;

        public CommentRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<Comment> GetCommentsByBlogId(Guid blogId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"REPOSITORY: Getting comments for blog ID {blogId}");
                
                // Clear any tracked entities from previous operations
                _context.ChangeTracker.Clear();
                
                // Use AsNoTracking to prevent entity caching
                var comments = _context.Comments
                    .AsNoTracking()
                    .Include(c => c.User)
                    .Where(c => c.BlogId == blogId && (c.IsDeleted == false || c.IsDeleted == null))
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                
                System.Diagnostics.Debug.WriteLine($"REPOSITORY: Found {comments.Count} comments for blog ID {blogId}");
                
                return comments;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"REPOSITORY EXCEPTION in GetCommentsByBlogId: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"REPOSITORY STACK: {ex.StackTrace}");
                return new List<Comment>();
            }
        }

        public bool AddComment(Comment comment)
        {
            // Create a new context instance for this operation to ensure clean transaction
            using (var isolatedContext = new PregnaCareAppDbContext())
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"REPOSITORY: AddComment called with comment ID: {comment.Id}, BlogId: {comment.BlogId}, UserId: {comment.UserId}");
                    
                    // Ensure comment has an ID
                    if (comment.Id == Guid.Empty)
                    {
                        comment.Id = Guid.NewGuid();
                        System.Diagnostics.Debug.WriteLine($"REPOSITORY: Generated new ID for comment: {comment.Id}");
                    }
                    
                    // Add the comment to the isolated context
                    isolatedContext.Comments.Add(comment);
                    System.Diagnostics.Debug.WriteLine("REPOSITORY: Comment added to isolated context");
                    
                    // Force transaction completion by saving immediately
                    int result = isolatedContext.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"REPOSITORY: SaveChanges result: {result}");
                    
                    bool success = result > 0;
                    
                    if (success)
                    {
                        System.Diagnostics.Debug.WriteLine($"REPOSITORY: Comment {comment.Id} successfully saved to database");
                        
                        // Explicitly clear the main context's change tracker
                        _context.ChangeTracker.Clear();
                        
                        // Verify the comment exists in the database by looking it up from the main context
                        var savedComment = _context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == comment.Id);
                        if (savedComment != null)
                        {
                            System.Diagnostics.Debug.WriteLine("REPOSITORY: Verified comment exists in database");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("REPOSITORY: WARNING - Comment not found in database after save!");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"REPOSITORY: Failed to save comment {comment.Id}");
                    }
                    
                    return success;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"REPOSITORY EXCEPTION: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"REPOSITORY STACK: {ex.StackTrace}");
                    return false;
                }
            }
        }

        public bool UpdateComment(Guid commentId, string newText)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    return false;
                }
                
                comment.CommentText = newText;
                comment.UpdatedAt = DateTime.Now;
                
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Error updating comment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteComment(Guid commentId)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    return false;
                }
                
                // Soft delete - just mark as deleted
                comment.IsDeleted = true;
                comment.UpdatedAt = DateTime.Now;
                
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Error deleting comment: {ex.Message}");
                return false;
            }
        }
    }
} 