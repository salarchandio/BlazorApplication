using Models;

namespace Repositories.Interface
{
    public interface IStampFieldsRepository
    {
        Task<IEnumerable<StampFields>> GetAllStampFieldsAsync();
    }
}
