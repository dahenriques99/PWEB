using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure.Entities;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Request;
using RCL.Dtos.Response;

namespace WebAPI.Repositories;

public sealed class CheckoutResult
{
    public bool Success { get; init; }
    public CheckoutResponseDto? Response { get; init; }
    public List<string> Errors { get; init; } = new();
}

public interface IOrderRepository
{
    Task<CheckoutResult> CreateOrderFromCart(string userId, List<CheckoutItemDto> items);
}

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<CheckoutResult> CreateOrderFromCart(
        string userId,
        List<CheckoutItemDto> items)
    {
        var errors = new List<string>();

        // 1) sanitize + merge duplicates
        var clean = items
            .Where(i => i.Quantity > 0)
            .GroupBy(i => i.ProductId)
            .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
            .ToList();

        if (clean.Count == 0)
            return new CheckoutResult { Success = false, Errors = new() { "Cart is empty." } };

        // 2) load products in one query
        var ids = clean.Select(x => x.ProductId).ToList();

        var products = await dbContext.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        // 3) validate existence
        if (products.Count != ids.Count)
            errors.Add("One or more products no longer exist.");

        // 4) validate stock
        foreach (var product in clean)
        {
            var p = products.FirstOrDefault(x => x.Id == product.ProductId);
            if (p is null) continue;

            if (product.Quantity > p.Stock)
                errors.Add($"Not enough stock for '{p.Name}'. Available: {p.Stock}.");
        }

        if (errors.Any())
            return new CheckoutResult { Success = false, Errors = errors };

        // 5) create order + items
        var now = DateTime.UtcNow;
        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.PendingPayment,
            Date = now,
            TotalAmount = 0m,
            OrderItems = new List<OrderItem>()
        };

        decimal total = 0m;

        foreach (var product in clean)
        {
            var p = products.First(x => x.Id == product.ProductId);

            var unitPrice = p.FinalPrice;
            total += unitPrice * product.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = p.Id,
                Quantity = product.Quantity,
                Price = unitPrice,
                Date = now
            });

            p.Stock -= product.Quantity;
        }

        order.TotalAmount = total;

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        return new CheckoutResult
        {
            Success = true,
            Response = new CheckoutResponseDto
            {
                OrderId = order.Id,
                Total = order.TotalAmount,
                Date = order.Date,
                Status = order.Status.ToString()
            }
        };
    }
}