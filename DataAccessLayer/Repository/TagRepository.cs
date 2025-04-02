using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class TagRepository
    {
        private PregnaCareAppDbContext _context;

        public TagRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.Where(t => t.IsDeleted != true).ToList();
        }

        public Tag? GetTagById(Guid id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id && t.IsDeleted != true);
        }

        public List<Tag> GetTagsByBlogId(Guid blogId)
        {
            return _context.BlogTags
                .Where(bt => bt.BlogId == blogId && bt.IsDeleted != true)
                .Select(bt => bt.Tag)
                .Where(t => t.IsDeleted != true)
                .ToList();
        }
    }
} 