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
            SupplierId = product.SupplierId,
            SupplierName = product.Supplier.Name ?? "",
            Name = product.Name,
            Categories =
                product.ProductCategories?
                    .Where(pc => pc.Category != null)
                    .Select(pc => pc.Category!.Name)
                    .Distinct()
                    .OrderBy(n => n)
                    .ToList()
                ?? new List<string>(),
            Description = product.Description,
            Price = product.Price,
            FinalPrice = product.FinalPrice,
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

    public static List<DeliveryMethodsResponseDto> FromDeliveryMethods(
        List<DeliveryMode> deliveryModes)
    {
        List<DeliveryMethodsResponseDto> deliveryMethodsResponse = new List<DeliveryMethodsResponseDto>();
        foreach (var deliveryMode in deliveryModes)
        {
            deliveryMethodsResponse.Add(new DeliveryMethodsResponseDto()
            {
                Id = deliveryMode.Id,
                Name = deliveryMode.Name,
            });
        }

        return deliveryMethodsResponse;
    }
}