using Microsoft.AspNetCore.SignalR;

namespace ApexTradeHub.Hubs;

public class TradeHub : Hub
{
    public async Task JoinSymbolGroup(string symbol)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, symbol.ToUpperInvariant());

        await Clients.Caller.SendAsync(
            "Receive Notification",
            $"Successfully subscribed to ticker: {symbol.ToUpperInvariant()}"
        );
    }

    public async Task LeaveSymbolGroup(string symbol)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, symbol.ToUpperInvariant());

        await Clients.Caller.SendAsync(
            "Receive Notification",
            $"Unsubscribed from ticker: {symbol.ToUpperInvariant()}"
        );
    }
}
