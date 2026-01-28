using OrdersAppBackend.Models;

namespace OrdersAppBackend.Data.Repositories
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem);
        Task AddManyAsync(int orderId, List<OrderItem> orderItems);
        Task<List<OrderItem>> FindManyAsync();
        Task<OrderItem?> FindFirstByIdAsync(int id);
        Task UpdateAsync(OrderItem orderItem);
        Task RemoveAsync(OrderItem orderItem);

        Task<List<string>> GetDistinctMakesAsync();
        Task<List<string>> GetDistinctModelsAsync(string make);
        Task<List<int>> GetDistinctYearsAsync(string make, string model);
    }
}
