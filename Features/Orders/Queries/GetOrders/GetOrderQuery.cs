using ApexTradeHub.Models;
using MediatR;

namespace ApexTradeHub.Features.Orders.Queries.GetOrders;

public record GetOrderQuery(string? Symbol = null) : IRequest<IEnumerable<TradeOrder>>;
