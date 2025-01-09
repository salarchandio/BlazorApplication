using Models;

namespace Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllOrdersAsync();
        Task<Orders> GetOrderByIdAsync(int id);
        Task<int> CreateOrderAsync(Orders Order);
        Task<bool> UpdateOrderAsync(Orders Order);
        Task<bool> DeleteOrderAsync(int id);
    }
}
