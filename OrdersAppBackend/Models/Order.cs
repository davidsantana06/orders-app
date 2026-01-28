namespace OrdersAppBackend.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal TotalValue { get; set; }

        public required string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
