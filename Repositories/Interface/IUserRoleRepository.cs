using Models;

namespace Repositories.Interface
{
    public interface IUserRoleRepository
    {
        Task<int> CreateUserRoleAsync(UserRoles UserRole);
        Task<bool> DeleteUserRoleAsync(int id);

    }
}
