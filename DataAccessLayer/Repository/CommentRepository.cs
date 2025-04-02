using DataAccessLayer.Entities;
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
            return _context.Comments
                .Where(c => c.BlogId == blogId && c.IsDeleted != true)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public bool AddComment(Comment comment)
        {
            try
            {
                if (comment.Id == Guid.Empty)
                {
                    comment.Id = Guid.NewGuid();
                }
                
                _context.Comments.Add(comment);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(Guid commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) return false;

            comment.IsDeleted = true;
            return _context.SaveChanges() > 0;
        }
    }
} 