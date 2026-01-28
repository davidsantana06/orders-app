namespace OrdersAppBackend.Models
{
    public class OrderWithItems
    {
        public int Id { get; set; }
        public decimal TotalValue { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
