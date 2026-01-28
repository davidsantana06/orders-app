using Microsoft.AspNetCore.Mvc;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Services;

namespace OrdersAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(
            [FromQuery] string? make = null,
            [FromQuery] string? model = null,
            [FromQuery] int? year = null
        )
        {
            var orders = await _orderService.GetManyAsync(make, model, year);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order is null)
                return NotFound(new { message = $"Order with ID {id} not found." });
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto dto)
        {
            var order = await _orderService.UpdateAsync(id, dto);
            if (order is null)
                return NotFound(new { message = $"Order with ID {id} not found." });
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Order with ID {id} not found." });
            return NoContent();
        }
    }
}
