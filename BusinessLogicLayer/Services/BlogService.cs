using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService()
        {
            _blogRepository = new BlogRepository();
        }
        // Lấy danh sách tất cả blog
        public List<Blog> GetAllBlogs()
        {
            return _blogRepository.GetAllBlogs();
        }

        // Lấy blog theo ID
        public Blog? GetBlogById(Guid id)
        {
            return _blogRepository.GetBlogById(id);
        }

        // Thêm một blog mới
        public bool AddBlog(Blog blog, Guid userId)
        {
            return _blogRepository.AddBlog(blog, userId);
        }

        // Cập nhật thông tin blog
        public bool UpdateBlog(Blog blog)
        {
            return _blogRepository.UpdateBlog(blog);
        }

        // Xóa blog (đánh dấu đã xoá thay vì xoá khỏi DB)
        public bool DeleteBlog(Guid id)
        {
            return _blogRepository.DeleteBlog(id);
        }
    }
}
