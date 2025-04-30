using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.AdminSetup;

public static class DataSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUserEntity>>();

        var adminEmail = "admin@admin.com";
        var adminPassword = "Admin123!";

        string[] roles = { "Administrator", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existingUser == null)
        {
            var adminUser = new AppUserEntity
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
                Console.WriteLine("Admin user created and assigned to 'Administrator' role.");
            }
            else
            {
                Console.WriteLine("Failed to create admin user:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($" - {error.Description}");
                }
            }
        }
    }
}