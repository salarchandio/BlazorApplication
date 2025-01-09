using Models;

namespace Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>> GetAllRolesAsync();
        Task<Roles> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(Roles Role);
        Task<bool> UpdateRoleAsync(Roles Role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
