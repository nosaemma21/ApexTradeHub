using MediatR;

namespace ApexTradeHub.Features.Orders.Commands.CreateOrder;

public record class CreateOrderCommand(string Symbol, decimal Price, decimal Quantity, bool IsBuy)
    : IRequest<Guid>;
