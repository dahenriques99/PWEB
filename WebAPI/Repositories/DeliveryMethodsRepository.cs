using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities;
using RCL.Dtos.Response;
using WebAPI.utils;

namespace WebAPI.Repositories;

public interface IDeliveryMethodsRepository
{
    Task<List<DeliveryMethodsResponseDto>> GetDeliveryMethods();
}

public class DeliveryMethodsRepository(ApplicationDbContext dbContext) : IDeliveryMethodsRepository
{
    public async Task<List<DeliveryMethodsResponseDto>> GetDeliveryMethods()
    {
        var deliveryModes = await dbContext.DeliveryModes
            .OrderBy(o => o.Name)
            .ToListAsync();
        
        return Mapper.FromDeliveryMethods(deliveryModes);
    }
}