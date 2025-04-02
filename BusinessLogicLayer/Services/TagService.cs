using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class TagService
    {
        private readonly TagRepository _tagRepository;

        public TagService()
        {
            _tagRepository = new TagRepository();
        }

        public List<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Tag? GetTagById(Guid id)
        {
            return _tagRepository.GetTagById(id);
        }

        public List<Tag> GetTagsByBlogId(Guid blogId)
        {
            return _tagRepository.GetTagsByBlogId(blogId);
        }
    }
} 