using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Services
{
    public interface IOrderService
    {
        Task<List<OrderWithItems>> GetManyAsync(string? make = null, string? model = null, int? year = null);
        Task<Order> CreateAsync(CreateOrderDto dto);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> UpdateAsync(int id, UpdateOrderDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
