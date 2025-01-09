using Models;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int? id);
        Task<int?> CreateUserAsync(Users User);
        Task<bool> UpdateUserAsync(Users User);
        Task<bool> DeleteUserAsync(int? id);
    }
}
