using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities;

namespace WebAPI.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories();
    Task<Category> GetCategory(int id);
}

public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await dbContext.Categories
            .OrderBy(o => o.Name)
            .ToListAsync();
    }
    
    public async Task<Category> GetCategory(int id)
    {
        var detalhe = await dbContext.Categories
            .FirstOrDefaultAsync(p => p.Id == id);

        if (detalhe == null) throw new InvalidOperationException();

        return detalhe;
    }

}