using ApexTradeHub.Data;
using ApexTradeHub.Hubs;
using ApexTradeHub.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ApexTradeHub.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly TradeDbContext _context;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IHubContext<TradeHub> _hubContext;

    public CreateOrderCommandHandler(
        TradeDbContext context,
        ILogger<CreateOrderCommandHandler> logger,
        IHubContext<TradeHub> hubContext
    )
    {
        _context = context;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new TradeOrder
        {
            Symbol = request.Symbol.ToUpperInvariant(),
            Price = request.Price,
            Quantity = request.Quantity,
            IsBuy = request.IsBuy,
            Status = "Executed",
        };

        _context.TradeOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order {OrderId} executed successfully for {Symbol} at ${Price}",
            order.Id,
            order.Symbol,
            order.Price
        );

        //RT Broadcast
        await _hubContext
            .Clients.Group(order.Symbol)
            .SendAsync(
                "ReceiveTradeExecution",
                new
                {
                    order.Id,
                    order.Symbol,
                    order.Price,
                    order.Quantity,
                    order.IsBuy,
                    order.Status,
                    order.CreatedAt,
                },
                cancellationToken
            );

        return order.Id;
    }
}
