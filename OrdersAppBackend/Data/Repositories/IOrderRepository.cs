using OrdersAppBackend.Models;

namespace OrdersAppBackend.Data.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<OrderWithItems>> FindManyAsync(string? make = null, string? model = null, int? year = null);
        Task<Order?> FindFirstByIdAsync(int id);
        Task UpdateAsync(Order order);
        Task RemoveAsync(Order order);
    }
}
