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

        // 1. هات الـ Context وطبق أي Migration ناقصة (دي أهم خطوة)
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        // 2. استخدم الـ RoleManager بنفس الـ Type اللي عرفته في الـ Identity
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (var roleName in AppRoles.AllRoles)
        {
            // 3. تأكد إن الدور مش موجود قبل الإضافة
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper() // الـ Identity بيعتمد على الـ NormalizedName في البحث
                });

                if (!result.Succeeded)
                {
                    // لو فشل لسبب ما (زي إن الـ Database مش مستجيبة)
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role {roleName}: {errors}");
                }
            }
        }
    }
}