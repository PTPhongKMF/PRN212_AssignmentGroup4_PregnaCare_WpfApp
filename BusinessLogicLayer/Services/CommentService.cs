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

        public CommentService()
        {
            _commentRepository = new CommentRepository();
        }

        public List<Comment> GetCommentsByBlogId(Guid blogId)
        {
            return _commentRepository.GetCommentsByBlogId(blogId);
        }

        public bool AddComment(Comment comment)
        {
            return _commentRepository.AddComment(comment);
        }

        public bool DeleteComment(Guid commentId)
        {
            return _commentRepository.DeleteComment(commentId);
        }
    }
} 