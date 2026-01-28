using System.ComponentModel.DataAnnotations;

namespace OrdersAppBackend.Dtos
{
    public class CreateOrderDto
    {
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public List<CreateOrderItemDto>? Items { get; set; }
    }
}
