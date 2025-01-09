using DataAccessLayer;
using Models;
using Repositories.Interface;

namespace Repositories
{
    public class UserRoleRepository : BaseRepository,IUserRoleRepository
    {
       
        public UserRoleRepository(DAL _dal): base(_dal) 
        { 
        }
        public Task<int> CreateUserRoleAsync(UserRoles UserRole)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserRoleAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}