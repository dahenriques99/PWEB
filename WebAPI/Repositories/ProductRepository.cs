using Microsoft.EntityFrameworkCore;
using MyMedia.Domain.Entities;
using MyMedia.Infrastructure;
using RCL.Dtos.Request;

namespace WebAPI.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(ProductQuery query);
    Task<Product> GetProduct(int id);
    Task<int> AddProduct(ProductRequestDto productRequest);
}

public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProducts(ProductQuery query)
    {
        IQueryable<Product> products = dbContext.Products
            .AsNoTracking()
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

        return await products.ToListAsync();
    }

    public async Task<Product> GetProduct(int id)
    {
        var detalhe = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (detalhe == null) throw new InvalidOperationException();

        return detalhe;
    }

    public async Task<int> AddProduct(ProductRequestDto requestDto)
    {
        var product = new Product
        {
            SupplierId = requestDto.SupplierId,
            Name = requestDto.Name,
            Description = requestDto.Description,
            Price = requestDto.Price,
            FinalPrice = requestDto.FinalPrice,
            Stock = requestDto.Stock
        };

        if (requestDto.CategoryIds is { Count: > 0 })
        {
            foreach (var catId in requestDto.CategoryIds.Distinct())
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    CategoryId = catId
                });
            }
        }

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        return product.Id;
    }
    
}