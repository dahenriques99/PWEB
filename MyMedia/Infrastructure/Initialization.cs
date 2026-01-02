using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMedia.Domain.Entities;

namespace MyMedia.Infrastructure;

public static class Initialization
{
    public static async Task CreateInitialData(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        string[] roles = ["Admin", "Worker", "Gestor", "Client"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    throw new Exception($"Failed to create role {role}: " +
                                        string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        await EnsureUser(
            userManager,
            email: "admin@localhost.com",
            password: "Is3C..00",
            name: "Administrador",
            surname: "Local",
            role: "Admin");

        await EnsureUser(
            userManager,
            email: "gestor@localhost.com",
            password: "Is3C..00",
            name: "Gestor",
            surname: "Local",
            role: "Gestor");

        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category { Name = "Format" },
                new Category { Name = "Label" }
            );

            await db.SaveChangesAsync();
        }
    }

    private static async Task EnsureUser(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string name,
        string surname,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
                Surname = surname,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                throw new Exception($"Failed to create user {email}: " +
                                    string.Join("; ", createResult.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
                throw new Exception($"Failed to add role {role} to {email}: " +
                                    string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));
        }
    }
}