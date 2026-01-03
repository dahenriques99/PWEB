using MyMedia.Infrastructure.Entities;
using RCL.Dtos.Response;

namespace WebAPI.utils;

public class Mapper
{
    public static ProductDto FromProduct(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            SupplierName = product.Supplier.Name,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
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