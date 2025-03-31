using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UserRepository
    {
        private PregnaCareAppDbContext _context;

        public UserRepository()
        {
            _context = new PregnaCareAppDbContext();
        }
        public User? GetUser(string email, string password)
        {
            return _context.Users.SingleOrDefault(m => m.Email == email && m.Password == password);
        }
    }
}
