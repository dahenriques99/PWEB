using MyMedia.Infrastructure;

namespace WebAPI.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser> FindUserByEmail(string email); 
}

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    Task<ApplicationUser> IUserRepository.FindUserByEmail(string email)
    {
        return null;
    }
}