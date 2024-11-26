using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;

namespace MonsterTradingCardsGame.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _userRepository.GetById(userId);
        }

        public async Task<User?> GetUserByUsername(string name)
        {
            return _userRepository.GetByUsername(name).Result.user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            if (user.Id == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.");
            }

            return await _userRepository.Update(user);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            return await _userRepository.Delete(userId);
        }
    }
}