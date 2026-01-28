using OrdersAppBackend.Data.Repositories;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderItem?> CreateAsync(CreateOrderItemDto dto, int orderId)
        {
            var order = await _orderRepository.FindFirstByIdAsync(orderId);
            if (order is null) return null;

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                UnitPrice = dto.UnitPrice,
                SubTotal = dto.SubTotal
            };

            await _orderItemRepository.AddAsync(orderItem);
            return orderItem;
        }

        public async Task<List<OrderItem>> GetManyAsync()
        {
            return await _orderItemRepository.FindManyAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _orderItemRepository.FindFirstByIdAsync(id);
        }

        public async Task<OrderItem?> UpdateAsync(int id, UpdateOrderItemDto dto)
        {
            var orderItem = await _orderItemRepository.FindFirstByIdAsync(id);
            if (orderItem is null) return null;

            orderItem.Make = dto.Make;
            orderItem.Model = dto.Model;
            orderItem.Year = dto.Year;
            orderItem.UnitPrice = dto.UnitPrice;
            orderItem.SubTotal = dto.SubTotal;

            await _orderItemRepository.UpdateAsync(orderItem);

            return orderItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderItem = await _orderItemRepository.FindFirstByIdAsync(id);
            if (orderItem is null) return false;

            await _orderItemRepository.RemoveAsync(orderItem);
            return true;
        }

        // Filter helpers
        public async Task<List<string>> GetMakesAsync()
        {
            return await _orderItemRepository.GetDistinctMakesAsync();
        }

        public async Task<List<string>> GetModelsAsync(string make)
        {
            return await _orderItemRepository.GetDistinctModelsAsync(make);
        }

        public async Task<List<int>> GetYearsAsync(string make, string model)
        {
            return await _orderItemRepository.GetDistinctYearsAsync(make, model);
        }
    }
}
