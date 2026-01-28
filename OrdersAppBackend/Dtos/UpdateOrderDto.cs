using System.ComponentModel.DataAnnotations;

namespace OrdersAppBackend.Dtos
{
    public class UpdateOrderDto
    {
        [Required, MaxLength(50)]
        [AllowedValues("Solicitado", "Em andamento", "Concluído", "Cancelado")]
        public required string Status { get; set; }
    }
}
