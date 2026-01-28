using Microsoft.EntityFrameworkCore;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _dbContext.OrderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddManyAsync(int orderId, List<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                item.OrderId = orderId;
            }
            await _dbContext.OrderItems.AddRangeAsync(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderItem>> FindManyAsync()
        {
            return await _dbContext.OrderItems
                .AsNoTracking()
                .OrderBy(oi => oi.OrderId)
                .ThenBy(oi => oi.Id)
                .ToListAsync();
        }

        public async Task<OrderItem?> FindFirstByIdAsync(int id)
        {
            return await _dbContext.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Update(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<string>> GetDistinctMakesAsync()
        {
            return await _dbContext.OrderItems
                .AsNoTracking()
                .Select(oi => oi.Make)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctModelsAsync(string make)
        {
            return await _dbContext.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Make == make)
                .Select(oi => oi.Model)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();
        }

        public async Task<List<int>> GetDistinctYearsAsync(string make, string model)
        {
            return await _dbContext.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Make == make && oi.Model == model)
                .Select(oi => oi.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }
    }
}
