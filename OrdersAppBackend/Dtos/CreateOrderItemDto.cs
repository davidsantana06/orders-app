using System.ComponentModel.DataAnnotations;

namespace OrdersAppBackend.Dtos
{
    public class CreateOrderItemDto
    {
        [Required, MaxLength(100)]
        public required string Make { get; set; }

        [Required, MaxLength(100)]
        public required string Model { get; set; }

        [Required, Range(1900, 2100)]
        public int Year { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SubTotal { get; set; }
    }
}
