namespace OrdersAppBackend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public required string Make { get; set; }

        public required string Model { get; set; }

        public int Year { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
