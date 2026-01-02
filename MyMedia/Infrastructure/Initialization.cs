using Microsoft.AspNetCore.Identity;
using MyMedia.Domain.Entities.enums;

namespace MyMedia.Infrastructure;

public static class Initialization
{
    public static async Task CreateInitialData(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        foreach (UserRole userRole in Enum.GetValues(typeof(UserRole)))
        {
            var role = userRole.ToString();
            if (await roleManager.RoleExistsAsync(role)) continue;
            var roleRole = new IdentityRole(role);
            await roleManager.CreateAsync(roleRole);
        }

        // Add Default User - Admin
        var defaultUser = new ApplicationUser
        {
            UserName = "admin@localhost.com", // Ele Valida o email
            Email = "admin@localhost.com",
            Name = "Administrador",
            Surname = "Local",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Status = UserStatus.Active,
            ClientType = UserType.Client
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Is3C..00"); // Ele valida se é Strong
                await userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }

    }
}