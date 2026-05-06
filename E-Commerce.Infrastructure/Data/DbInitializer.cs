using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. Seed Roles First
        foreach (var roleName in AppRoles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role {roleName}: {errors}");
                }
            }
        }

        // 2. Seed Super Admin User
        var adminEmail = "admin@ecommerce.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            var userResult = ApplicationUser.Create("Super", null, "Admin", adminEmail, "admin", "0123456789", null, new DateOnly(1990, 1, 1));
            
            if (userResult.IsSuccess)
            {
                var user = userResult.Value;
                user!.EmailConfirmed = true;
                
                var createResult = await userManager.CreateAsync(user, "Admin@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AppRoles.SuperAdmin);
                }
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(adminUser, AppRoles.SuperAdmin))
            {
                await userManager.AddToRoleAsync(adminUser, AppRoles.SuperAdmin);
            }
        }

        // 3. Seed Vendor & Customer
        await SeedUserAsync(userManager, "Test", "Vendor", "vendor@ecommerce.com", "vendor", "0987654321", "Vendor@123", AppRoles.Vendor);
        await SeedUserAsync(userManager, "Test", "Customer", "customer@ecommerce.com", "customer", "0112233445", "Customer@123", AppRoles.Customer);
    }

    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string firstName, string lastName, string email, string userName, string phoneNumber, string password, string role)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is null)
        {
            var userResult = ApplicationUser.Create(firstName, null, lastName, email, userName, phoneNumber, null, new DateOnly(1990, 1, 1));
            if (userResult.IsSuccess)
            {
                var user = userResult.Value;
                user!.EmailConfirmed = true;
                var createResult = await userManager.CreateAsync(user, password);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(existingUser, role))
            {
                await userManager.AddToRoleAsync(existingUser, role);
            }
        }
    }
}