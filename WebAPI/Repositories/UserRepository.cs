using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Response;
using WebAPI.utils;

namespace WebAPI.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<OrderSummaryDto>> GetUserOrders(string userId);
    Task<IEnumerable<ProductResponseDto>> GetUserProducts(string supplierId);
}

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<IEnumerable<OrderSummaryDto>> GetUserOrders(string userId)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId && o.Status != OrderStatus.InCart)
            .OrderByDescending(o => o.Date)
            .Select(o => new OrderSummaryDto
            {
                OrderId = o.Id,
                Date = o.Date,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString()
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetUserProducts(string supplierId)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Supplier)
            .Where(p => p.SupplierId == supplierId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return products.Select(Mapper.FromProduct);
    }
}