using DataAccessLayer;
using Models;
using Repositories;

namespace Services
{
    public class UserService
    {
        private UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<int?> CreateUserAsync(Users user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int? id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

    }
}
