using Microsoft.AspNetCore.Identity;

namespace MyMedia.Infrastructure;

public static class Initialization
{
    public static async Task CreateInitialData(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        // Add default Roles
        string[] roles = ["Admin", "Gestor", "Cliente"];

        foreach (var role in roles)
        {
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