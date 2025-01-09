using DataAccessLayer;
using Models;
using Repositories.Interface;

namespace Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DAL _dal) : base(_dal)
        {

        }
     
        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await DAL.GetAllAsync<Users>("SP_SELECT_USERS");
        }

        public async Task<Users> GetUserByIdAsync(int? ID)
        {
            return await DAL.GetByIdAsync<Users>("SP_SELECT_USERBYID", new { UserID = ID });
        }
        public async Task<int?> CreateUserAsync(Users User)
        {
            var Response = new
            {
                UserName = User.UserName,
                Email = User.Email
            };

            return await DAL.AddAsync("SP_INSERT_USERS", Response);
        }
        public async Task<bool> UpdateUserAsync(Users User)
        {
            var Response = new
            {
                UserID = User.UserID,
                UserName = User.UserName,
                Email = User.Email
            };

            return await DAL.UpdateAsync("SP_UPDATE_USER", User);
        }
        public async Task<bool> DeleteUserAsync(int? ID)
        {
            return await DAL.DeleteAsync("SP_DELETE_USER", new { UserID = ID });
        }
    }
}