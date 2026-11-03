using ApexTradeHub.Features.Orders.Commands.CreateOrder;
using ApexTradeHub.Features.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApexTradeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(PlaceOrder),
                new { id = orderId },
                new { OrderId = orderId, Message = "Order places successfully!!!" }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? symbol)
        {
            var query = new GetOrderQuery(symbol);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
    }
}
