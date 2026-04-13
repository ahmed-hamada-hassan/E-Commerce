using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Data;
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
        var query = from user in _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
                    where user.Id == request.UserId
                       && (user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow)
                    join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId into ur
                    from userRole in ur.DefaultIfEmpty()
                    join role in _dbContext.Roles on userRole.RoleId equals role.Id into r
                    from role in r.DefaultIfEmpty()
                    where role.Name == "Customer"
                    select new AdminCustomerResponse
                    (
                        UserId: user.Id,
                        Name: user.FullName,
                        UserName: user.UserName!,
                        Email: user.Email!,
                        PhoneNumber: user.PhoneNumber!,
                        ImageUrl: user.ImageUrl!,
                        DateOfBirth: user.DateOfBirth,
                        IsDeleted: user.IsDeleted,
                        DeletedAt: user.DeleteOn,
                        IsLockout: user.LockoutEnabled,
                        LockoutEnd: user.LockoutEnd,
                        Status: user.IsDeleted ? "Deleted" : 
                        (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) ? "Locked" : "Active",

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
                        )).ToList()
                    );

        var response = await query.FirstOrDefaultAsync(cancellationToken);
        if( response is null)
            return Result<AdminCustomerResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<AdminCustomerResponse>.Success(response);
    }
}
