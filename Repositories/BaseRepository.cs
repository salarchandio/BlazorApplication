using DataAccessLayer;
using Models;
using Repositories.Interface;

namespace Repositories
{
    public class BaseRepository
    {
        private DAL _DAL;

        public BaseRepository(DAL dal)
        {
            _DAL = dal;
        }

        public DAL DAL { set { _DAL = value; } get { return _DAL; }}
    }
}