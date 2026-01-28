using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem?> CreateAsync(CreateOrderItemDto dto, int orderId);
        Task<List<OrderItem>> GetManyAsync();
        Task<OrderItem?> GetByIdAsync(int id);
        Task<OrderItem?> UpdateAsync(int id, UpdateOrderItemDto dto);
        Task<bool> DeleteAsync(int id);

        // Filter helpers
        Task<List<string>> GetMakesAsync();
        Task<List<string>> GetModelsAsync(string make);
        Task<List<int>> GetYearsAsync(string make, string model);
    }
}
