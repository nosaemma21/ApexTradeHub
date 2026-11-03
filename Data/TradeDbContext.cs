using System;
using ApexTradeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ApexTradeHub.Data;

public class TradeDbContext : DbContext
{
    public TradeDbContext(DbContextOptions<TradeDbContext> options)
        : base(options) { }

    public DbSet<TradeOrder> TradeOrders => Set<TradeOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TradeOrder>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Symbol).IsRequired().HasMaxLength(20);

            entity.Property(o => o.Price).HasPrecision(18, 4);

            entity.Property(o => o.Quantity).HasPrecision(18, 4);

            entity.Property(o => o.Status).HasMaxLength(30).HasDefaultValue("Pending");

            // Explicitly map CreatedAt so EF Core's query translator recognizes it
            entity
                .Property(o => o.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexing the symbol for ultra-fast order book lookups
            entity.HasIndex(o => o.Symbol);
        });
    }
}
