using Models;

namespace Repositories.Interface
{
    public interface IUserPasswordRepository
    {
        Task<UserPasswords> GetUserPasswordByIdAsync(int id);
        Task<int> CreateUserPasswordAsync(UserPasswords UserPassword);
        Task<bool> UpdateUserPasswordAsync(UserPasswords UserPassword);
        Task<bool> DeleteUserPasswordAsync(int id);
    }
}
