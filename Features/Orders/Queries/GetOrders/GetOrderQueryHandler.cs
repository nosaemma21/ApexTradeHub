using ApexTradeHub.Data;
using ApexTradeHub.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApexTradeHub.Features.Orders.Queries.GetOrders;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, IEnumerable<TradeOrder>>
{
    private readonly TradeDbContext _context;

    public GetOrderQueryHandler(TradeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TradeOrder>> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _context.TradeOrders.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Symbol))
        {
            query = query.Where(o => o.Symbol == request.Symbol.ToUpperInvariant());
        }

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
    }
}
