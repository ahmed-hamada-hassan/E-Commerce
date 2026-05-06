using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
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

        // 1. Seed Roles
        foreach (var roleName in AppRoles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName, NormalizedName = roleName.ToUpper() });
            }
        }

        // 2. Seed Users & Addresses
        // Admin
        var adminId = await CreateUserIfNotExists(userManager, "Super", "Admin", "admin@ecommerce.com", "admin", "0123456789", "Admin@123", AppRoles.SuperAdmin);
        await SeedAddressIfEmpty(context, adminId, "Admin Office", "Main St", "Cairo", "Cairo", "11511", "Egypt");

        // Representative
        var repId = await CreateUserIfNotExists(userManager, "Test", "Representative", "rep@ecommerce.com", "representative", "0112233446", "Rep@123", AppRoles.Representative);
        await SeedAddressIfEmpty(context, repId, "Rep Hub", "Logistics Lane", "Alexandria", "Alex", "21500", "Egypt");

        // Customer
        var customerId = await CreateUserIfNotExists(userManager, "Test", "Customer", "customer@ecommerce.com", "customer", "0112233445", "Customer@123", AppRoles.Customer);
        await SeedAddressIfEmpty(context, customerId, "123 Home St", "Apartment 4B", "Giza", "Giza", "12345", "Egypt");

        // Vendor
        var vendorUserId = await CreateUserIfNotExists(userManager, "Test", "Vendor", "vendor@ecommerce.com", "vendor", "0987654321", "Vendor@123", AppRoles.Vendor);
        if (vendorUserId != Guid.Empty)
        {
            await SeedAddressIfEmpty(context, vendorUserId, "Vendor Warehouse", "Industrial Zone", "6th October", "Giza", "54321", "Egypt");

            if (!await context.Vendors.AnyAsync(v => v.UserId == vendorUserId))
            {
                var vendor = Vendor.Create("Test Store", "CR123456", vendorUserId);
                context.Vendors.Add(vendor.Value!);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task<Guid> CreateUserIfNotExists(UserManager<ApplicationUser> userManager,
        string fName, string lName, string email, string userName, string phone, string password, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            var userResult = ApplicationUser.Create(fName, null, lName, email, userName, phone, null, new DateOnly(1990, 1, 1));
            if (userResult.IsFailure) return Guid.Empty;

            user = userResult.Value;
            user!.EmailConfirmed = true;

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded) return Guid.Empty;
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private static async Task SeedAddressIfEmpty(AppDbContext context, Guid userId, string line1, string line2, string city, string state, string zip, string country)
    {
        if (userId == Guid.Empty) return;

        if (!await context.Addresses.AnyAsync(a => a.UserId == userId))
        {
            var addressResult = Address.Create(userId, line1, line2, city, state, zip, country, AddressType.Shipping);
            if (addressResult.IsSuccess)
            {
                context.Addresses.Add(addressResult.Value!);
                await context.SaveChangesAsync();
            }
        }
    }
}