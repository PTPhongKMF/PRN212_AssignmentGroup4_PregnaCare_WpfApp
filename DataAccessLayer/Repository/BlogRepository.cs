using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class BlogRepository
    {
        private readonly PregnaCareAppDbContext _context;

        public BlogRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<Blog> GetAllBlogs()
        {
            return _context.Blogs.Where(b => b.IsDeleted == false && b.IsVisible == true).ToList();
        }

        public Blog? GetBlogById(Guid id)
        {
            return _context.Blogs.FirstOrDefault(b => b.Id == id && b.IsDeleted == false);
        }

        public bool AddBlog(Blog blog, Guid userId)
        {
            if (blog == null) return false;

            blog.Id = Guid.NewGuid();
            blog.UserId = userId;  // Gán UserId hợp lệ
            blog.CreatedAt = DateTime.Now;
            blog.UpdatedAt = DateTime.Now;
            blog.IsDeleted = false;
            blog.ViewCount = 0;  // Đặt ViewCount = 0
            blog.UrlHandle = GenerateUrlHandle(blog.PageTitle); // Tạo UrlHandle nếu chưa có

            _context.Blogs.Add(blog);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateBlog(Blog blog)
        {
            var existingBlog = _context.Blogs.FirstOrDefault(b => b.Id == blog.Id && b.IsDeleted == false);
            if (existingBlog == null) return false;

            existingBlog.PageTitle = blog.PageTitle;
            existingBlog.Heading = blog.Heading;
            existingBlog.Content = blog.Content;
            existingBlog.ShortDescription = blog.ShortDescription;
            existingBlog.FeaturedImageUrl = blog.FeaturedImageUrl;
            existingBlog.IsVisible = blog.IsVisible;
            existingBlog.UpdatedAt = DateTime.Now;
            existingBlog.UrlHandle = GenerateUrlHandle(blog.PageTitle); // Cập nhật UrlHandle nếu cần

            return _context.SaveChanges() > 0;
        }

        public bool DeleteBlog(Guid id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return false;

            blog.IsDeleted = true;
            return _context.SaveChanges() > 0;
        }

        private string GenerateUrlHandle(string title)
        {
            return title.ToLower().Replace(" ", "-"); // Tạo URL handle từ tiêu đề
        }
    }
}
