using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Interfaces.Data;

public interface IAppDbContext : IScopedService
{
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<IdentityRole<Guid>> Roles { get; set; }
    DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }

    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Vendor> Vendors { get; set; }
    DbSet<ProductImage> ProductImages { get; set; }
    DbSet<Feedback> Reviews { get; set; }

    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Cancellation> Cancellations { get; set; }

    DbSet<Payment> Payments { get; set; }
    DbSet<Refund> Refunds { get; set; }
    DbSet<ReturnRequest> ReturnRequests { get; set; }

}
