using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public User? GetUser(string email, string password)
        {
            return _userRepository.GetUser(email, password);
        }
        
        public User? GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }
        
        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }
        
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public bool AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public bool IsEmailInUse(string email)
        {
            return _userRepository.IsEmailInUse(email);
        }

        public string GetUserRoleName(Guid userId)
        {
            return _userRepository.GetUserRoleName(userId);
        }
    }
}
