using Models;

namespace Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(Products Product);
        Task<bool> UpdateProductAsync(Products Product);
        Task<bool> DeleteProductAsync(int id);
    }
}
