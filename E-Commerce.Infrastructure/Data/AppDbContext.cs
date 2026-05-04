using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Common;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace E_Commerce.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region Identity DbSets
    public DbSet<Address> Addresses { get; set; }
    #endregion

    #region Catalog DbSets
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Feedback> Reviews { get; set; }
    #endregion

    #region CheckOut DbSets
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cancellation> Cancellations { get; set; }
    #endregion

    #region Finance DbSets
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Refund> Refunds { get; set; }
    public DbSet<ReturnRequest> ReturnRequests { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetDynamicIsDeletedFilter(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GetDynamicIsDeletedFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "it");
        var prop = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }
}


//ApplicationUser, Address => These tables and the ASP.NET Identity tables[Role, UserRole, UserClaim, UserLogin, RoleClaim, UserToken]
                            //their schema will be in the Identity schema
//Category, Product, Vendor, ProductImage, Feedback => These tables will be in the Catalog schema
//Cart, CartItem => These tables will be in the Cart schema
//Order, OrderItem, Cancellation => These tables will be in the CheckOut schema
//Payment, Refund => These tables will be in the Finance schema