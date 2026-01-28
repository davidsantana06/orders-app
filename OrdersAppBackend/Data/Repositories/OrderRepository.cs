using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderWithItems>> FindManyAsync(string? make = null, string? model = null, int? year = null)
        {
            // Use Stored Procedure sp_GetOrdersWithFilters
            var makeParam = new SqlParameter("@Make", (object?)make ?? DBNull.Value);
            var modelParam = new SqlParameter("@Model", (object?)model ?? DBNull.Value);
            var yearParam = new SqlParameter("@Year", (object?)year ?? DBNull.Value);

            var sql = "EXEC sp_GetOrdersWithFilters @Make, @Model, @Year";

            // Execute stored procedure and map to OrderWithItems
            var orders = new Dictionary<int, OrderWithItems>();

            await using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add(makeParam);
            command.Parameters.Add(modelParam);
            command.Parameters.Add(yearParam);

            await _dbContext.Database.OpenConnectionAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));

                if (!orders.ContainsKey(orderId))
                {
                    orders[orderId] = new OrderWithItems
                    {
                        Id = orderId,
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        TotalValue = reader.GetDecimal(reader.GetOrdinal("TotalValue")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("OrderCreatedAt")),
                        UpdatedAt = reader.GetDateTime(reader.GetOrdinal("OrderUpdatedAt")),
                        Items = new List<OrderItem>()
                    };
                }

                // Add item if not null (LEFT JOIN case)
                if (!reader.IsDBNull(reader.GetOrdinal("ItemId")))
                {
                    var item = new OrderItem
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ItemId")),
                        OrderId = orderId,
                        Make = reader.GetString(reader.GetOrdinal("Make")),
                        Model = reader.GetString(reader.GetOrdinal("Model")),
                        Year = reader.GetInt32(reader.GetOrdinal("Year")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        SubTotal = reader.IsDBNull(reader.GetOrdinal("SubTotal"))
                            ? null
                            : reader.GetDecimal(reader.GetOrdinal("SubTotal"))
                    };
                    orders[orderId].Items.Add(item);
                }
            }

            await _dbContext.Database.CloseConnectionAsync();

            return orders.Values.ToList();
        }

        public async Task<Order?> FindFirstByIdAsync(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Order order)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
