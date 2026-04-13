using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Vendor;

internal sealed class AdminGetVendorQueryHandler : IRequestHandler<AdminGetVendorQuery, Result<AdminVendorResponse>>
{
    private readonly IAppDbContext _dbContext;

    public AdminGetVendorQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AdminVendorResponse>> Handle(AdminGetVendorQuery request, CancellationToken cancellationToken)
    {
        var query = from vendor in _dbContext.Vendors.IgnoreQueryFilters().AsNoTracking()
                    where vendor.Id == request.VendorId
                    join user in _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
                        on vendor.UserId equals user.Id
                    select new AdminVendorResponse(
                        user.Id,
                        vendor.Id,
                        user.FullName,
                        user.UserName!,
                        user.Email!,
                        user.PhoneNumber!,
                        user.ImageUrl,
                        vendor.StoreName,
                        vendor.CommercialRegistrationNumber,
                        vendor.IsActive,
                        user.DateOfBirth,
                        user.IsDeleted,
                        user.DeleteOn,
                        user.LockoutEnabled,
                        user.LockoutEnd,
                        (user.IsDeleted || vendor.IsDeleted) ? "Deleted" :
                        (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) ? "Blocked" :
                        (!vendor.IsActive) ? "PendingApproval" : "Active",
                        user.Addresses.Select(a => new CustomerAddressInfo
                        (
                            AddressId: a.Id,
                            AddressLine1: a.AddressLine1,
                            AddressLine2: a.AddressLine2,
                            City: a.City,
                            StateOrProvince: a.StateOrProvince,
                            Country: a.Country,
                            PostalCode: a.PostalCode,
                            AddressType: a.AddressType
                        )).ToList()); 

        var response = await query.FirstOrDefaultAsync(cancellationToken);
        if(response is null)
            return Result<AdminVendorResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<AdminVendorResponse>.Success(response);
    }
}