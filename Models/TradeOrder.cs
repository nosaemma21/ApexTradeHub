using System;

namespace ApexTradeHub.Models;

public class TradeOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public bool IsBuy { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt = DateTime.UtcNow;
}
