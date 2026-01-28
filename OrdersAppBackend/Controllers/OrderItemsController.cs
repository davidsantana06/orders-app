using Microsoft.AspNetCore.Mvc;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Services;

namespace OrdersAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var items = await _orderItemService.GetManyAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _orderItemService.GetByIdAsync(id);
            if (item is null)
                return NotFound(new { message = $"Item with ID {id} not found." });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemDto dto, [FromQuery] int orderId)
        {
            var item = await _orderItemService.CreateAsync(dto, orderId);
            if (item is null)
                return NotFound(new { message = $"Order with ID {orderId} not found." });
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemDto dto)
        {
            var item = await _orderItemService.UpdateAsync(id, dto);
            if (item is null)
                return NotFound(new { message = $"Item with ID {id} not found." });
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderItemService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Item with ID {id} not found." });
            return NoContent();
        }

        [HttpGet("makes")]
        public async Task<IActionResult> GetMakes()
        {
            var makes = await _orderItemService.GetMakesAsync();
            return Ok(makes);
        }

        [HttpGet("{make}/models")]
        public async Task<IActionResult> GetModels(string make)
        {
            var models = await _orderItemService.GetModelsAsync(make);
            return Ok(models);
        }

        [HttpGet("{make}/{model}/years")]
        public async Task<IActionResult> GetYears(string make, string model)
        {
            var years = await _orderItemService.GetYearsAsync(make, model);
            return Ok(years);
        }
    }
}
