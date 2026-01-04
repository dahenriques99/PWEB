using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure.Entities;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Request;
using RCL.Dtos.Response;
using WebAPI.utils;

namespace WebAPI.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(ProductQuery query);
    Task<ProductResponseDto?> GetProduct(int id);
    Task<(ProductResponseDto? Product, List<string> Errors)> AddProduct(string supplierId, CreateProductDto dto);
    Task<(bool Ok, ProductResponseDto? Product, List<string> Errors)> 
        UpdateProduct(string supplierId, int productId, UpdateProductDto dto);
}

public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProducts(ProductQuery query)
    {
        IQueryable<Product> products = dbContext.Products
            .AsNoTracking()
            .Where(p => p.Status == ProductStatus.Active)
            .Include(p => p.ProductCategories)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (query.CategoryIds is { Count: > 0 })
        {
            var ids = query.CategoryIds.Distinct().ToList();

            if (!query.MatchAllCategories)
            {
                products = products.Where(p =>
                    p.ProductCategories.Any(pc => ids.Contains(pc.CategoryId)));
            }
            else
            {
                products = products.Where(p =>
                    ids.All(id => p.ProductCategories.Any(pc => pc.CategoryId == id)));
            }
        }

        if (query.OnlyInStock)
        {
            products = products.Where(p => p.Stock > 0);
        }

        return await products.ToListAsync();
    }

    public async Task<ProductResponseDto?> GetProduct(int id)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Supplier)
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        return Mapper.FromProduct(product);
    }

    public async Task<(ProductResponseDto? Product, List<string> Errors)> AddProduct(string supplierId,
        CreateProductDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name)) errors.Add("Name is required.");
        if (dto.Price <= 0) errors.Add("Price must be > 0.");
        if (dto.Stock < 0) errors.Add("Stock must be >= 0.");

        var distinctCats = dto.CategoryIds.Distinct().ToList();

        //Check all categories IDs exist in DB
        if (distinctCats.Count > 0)
        {
            var existing = await dbContext.Categories
                .Where(c => distinctCats.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            if (existing.Count != distinctCats.Count)
                errors.Add("One or more categories do not exist.");
        }

        if (errors.Any())
            return (null, errors);

        var product = new Product
        {
            SupplierId = supplierId,
            Name = dto.Name.Trim(),
            Description = dto.Description,
            Price = dto.Price,
            FinalPrice = dto.Price,
            Stock = dto.Stock,
            ImageData = dto.ImageData ?? Array.Empty<byte>(),

            Status = ProductStatus.Pending
        };

        foreach (var catId in distinctCats)
        {
            product.ProductCategories.Add(new ProductCategory
            {
                CategoryId = catId
            });
        }

        var supplierName = await dbContext.Users
            .Where(u => u.Id == supplierId)
            .Select(u => u.Name)
            .FirstOrDefaultAsync() ?? "";

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var productResponse = new ProductResponseDto
        {
            Id = product.Id,
            SupplierName = supplierName,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            FinalPrice = product.FinalPrice,
            Stock = product.Stock,
            Status = product.Status.ToString(),
            ImageData = product.ImageData
        };

        return (productResponse, errors);
    }

    public async Task<(bool Ok, ProductResponseDto? Product, List<string> Errors)>
        UpdateProduct(string supplierId, int productId, UpdateProductDto dto)
    {
        var errors = new List<string>();

        var product = await dbContext.Products
            .Include(p => p.ProductCategories)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null) return (false, null, new() { "Product not found." });
        if (product.SupplierId != supplierId) return (false, null, new() { "Not allowed." });

        if (string.IsNullOrWhiteSpace(dto.Name)) errors.Add("Name is required.");
        if (dto.Price <= 0) errors.Add("Price must be > 0.");
        if (dto.Stock < 0) errors.Add("Stock must be >= 0.");

        var distinctCats = (dto.CategoryIds ?? new()).Distinct().ToList();
        if (distinctCats.Count > 0)
        {
            var existing = await dbContext.Categories
                .Where(c => distinctCats.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            if (existing.Count != distinctCats.Count)
                errors.Add("One or more categories do not exist.");
        }

        if (errors.Any()) return (false, null, errors);

        product.Name = dto.Name.Trim();
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        if (dto.ImageData is { Length: > 0 })
            product.ImageData = dto.ImageData;

        product.ProductCategories.Clear();
        foreach (var catId in distinctCats)
            product.ProductCategories.Add(new ProductCategory { CategoryId = catId });

        await dbContext.SaveChangesAsync();

        return (true, Mapper.FromProduct(product), errors);
    }
}