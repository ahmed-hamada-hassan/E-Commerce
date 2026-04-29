using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Vendors.Queries.Admin_Get_Vendor;

internal sealed class AdminGetVendorsQueryHandler : IRequestHandler<AdminGetVendorsQuery, Result<CursorPagedResult<AdminVendorsResponse, Guid>>>
{
    private readonly IAppDbContext _dbContext;

    public AdminGetVendorsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CursorPagedResult<AdminVendorsResponse, Guid>>> Handle(AdminGetVendorsQuery request, CancellationToken cancellationToken)
    {
        var baseQuery =
            from vendor in _dbContext.Vendors.IgnoreQueryFilters().AsNoTracking()
            join user in _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
                on vendor.UserId equals user.Id
            select new { Vendor = vendor, User = user };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                x.User.Email!.ToLower().Contains(search) ||
                (x.User.FullName).ToLower().Contains(search) ||
                x.Vendor.StoreName.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(x => !x.User.IsDeleted && !x.Vendor.IsDeleted && x.Vendor.IsActive && (x.User.LockoutEnd == null || x.User.LockoutEnd <= DateTimeOffset.UtcNow));
            else if (request.Status.Equals("Blocked", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(x => x.User.LockoutEnd > DateTimeOffset.UtcNow);
            else if (request.Status.Equals("Deleted", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(x => x.User.IsDeleted || x.Vendor.IsDeleted);
            else if (request.Status.Equals("PendingApproval", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(x => !x.Vendor.IsActive && !x.User.IsDeleted && !x.Vendor.IsDeleted);
        }

        baseQuery = baseQuery.OrderBy(x => x.Vendor.Id);

        if (request.Cursor.HasValue)
            baseQuery = baseQuery.Where(x => x.Vendor.Id.CompareTo(request.Cursor.Value) > 0);

        var itemsToFetch = request.Size + 1;

        var resultList = await baseQuery.Take(itemsToFetch)
            .Select(x => new AdminVendorsResponse(
                x.User.Id,
                x.Vendor.Id,
                x.User.FullName,
                x.User.UserName!,
                x.User.Email!,
                x.User.PhoneNumber!,
                x.User.ImageUrl,
                x.Vendor.StoreName,
                x.Vendor.CommercialRegistrationNumber,
                x.Vendor.IsActive,
                x.User.DateOfBirth,
                x.User.IsDeleted || x.Vendor.IsDeleted,
                x.User.DeleteOn,
                x.User.LockoutEnabled,
                x.User.LockoutEnd,
                (x.User.IsDeleted || x.Vendor.IsDeleted) ? "Deleted" :
                (x.User.LockoutEnd != null && x.User.LockoutEnd > DateTimeOffset.UtcNow) ? "Blocked" :
                (!x.Vendor.IsActive) ? "PendingApproval" : "Active"
            )).ToListAsync(cancellationToken);

        bool hasMoreData = resultList.Count == itemsToFetch;
        if (hasMoreData)
            resultList.RemoveAt(resultList.Count - 1);

        var nextCursor = hasMoreData ? resultList.Last().VendorId : (Guid?)null;

        var pagedResult = new CursorPagedResult<AdminVendorsResponse, Guid>(
            items: resultList,
            nextCursor: nextCursor
        );

        return Result<CursorPagedResult<AdminVendorsResponse, Guid>>.Success(pagedResult);
    }
}
