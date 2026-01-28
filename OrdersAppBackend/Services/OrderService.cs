using OrdersAppBackend.Data.Repositories;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<List<OrderWithItems>> GetManyAsync(
            string? make = null,
            string? model = null,
            int? year = null
        )
        {
            return await _orderRepository.FindManyAsync(make, model, year);
        }

        public async Task<Order> CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                Status = "Solicitado",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _orderRepository.AddAsync(order);

            if (dto.Items != null && dto.Items.Count > 0)
            {
                var orderItems = dto.Items.Select(item => new OrderItem
                {
                    OrderId = order.Id,
                    Make = item.Make,
                    Model = item.Model,
                    Year = item.Year,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList();

                await _orderItemRepository.AddManyAsync(order.Id, orderItems);
            }

            return order;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepository.FindFirstByIdAsync(id);
        }

        public async Task<Order?> UpdateAsync(int id, UpdateOrderDto dto)
        {
            var order = await _orderRepository.FindFirstByIdAsync(id);
            if (order is null) return null;

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.FindFirstByIdAsync(id);
            if (order is null) return false;

            await _orderRepository.RemoveAsync(order);
            return true;
        }
    }
}
