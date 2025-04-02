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
        
        public User? GetUserById(Guid id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }
        public bool AddUser(User user)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return false; // Không cho phép đăng ký trùng email
                }

                user.Id = Guid.NewGuid();
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.IsDeleted = false;

                _context.Users.Add(user);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                var existingUser = _context.Users.SingleOrDefault(u => u.Id == user.Id);
                if (existingUser == null) return false;
                
                // Check if trying to change to an email that's already in use by another user
                if (existingUser.Email != user.Email && _context.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
                {
                    return false;
                }
                
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Gender = user.Gender;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Address = user.Address;
                existingUser.ImageUrl = user.ImageUrl;
                existingUser.UpdatedAt = DateTime.Now;
                
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public List<User> GetAllUsers()
        {
            return _context.Users.Where(u => u.IsDeleted != true).ToList();
        }

        public bool IsEmailInUse(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
