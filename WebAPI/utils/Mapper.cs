using MyMedia.Infrastructure.Entities;
using RCL.Dtos.Response;

namespace WebAPI.utils;

public class Mapper
{
    public static ProductResponseDto FromProduct(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            SupplierName = product.Supplier.Name,
            Name = product.Name,
            Description = product.Description,
            Price = product.FinalPrice,
            Stock = product.Stock,
            Status = product.Status.ToString(),
            ImageData = product.ImageData
        };
    }
    
    public static CategoryDto FromCategory(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}