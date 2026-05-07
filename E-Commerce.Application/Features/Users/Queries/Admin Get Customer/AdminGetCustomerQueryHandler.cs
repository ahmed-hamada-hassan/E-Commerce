using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Users.Queries;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

internal sealed class AdminGetCustomerQueryHandler : IRequestHandler<AdminGetCustomerQuery, Result<AdminCustomerResponse>>
{
    private readonly IAppDbContext _dbContext;

    public AdminGetCustomerQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AdminCustomerResponse>> Handle(AdminGetCustomerQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(u => u.Id == request.UserId)
            .Select(u => new AdminCustomerResponse
            (
                UserId : u.Id,
                Name : u.FullName,
                UserName : u.UserName!,
                Email : u.Email!,
                PhoneNumber : u.PhoneNumber!,
                ImageUrl : u.ImageUrl,
                DateOfBirth : u.DateOfBirth,
                DefaultShippingAddressId: u.DefaultShippingAddressId,
                IsDeleted : u.IsDeleted,
                DeletedAt : u.DeleteOn,
                IsBlocked : !u.LockoutEnabled,
                BlockedAt : u.LockoutEnd,
                Status : u.IsDeleted ? "Deleted" : u.LockoutEnabled ? "Locked" : "Active",
                Addresses : u.Addresses.Select(a => new AdminCustomerAddressInfo
                (
                    AddressId : a.Id,
                    AddressLine1 : a.AddressLine1,
                    AddressLine2 : a.AddressLine2,
                    City : a.City,
                    StateOrProvince : a.StateOrProvince,
                    Country : a.Country,
                    PostalCode : a.PostalCode,
                    AddressType : a.AddressType,
                    Status : a.IsDeleted ? "Deleted" : "Active"
                )).ToList()
            )).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<AdminCustomerResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<AdminCustomerResponse>.Success(user);
    }
}
