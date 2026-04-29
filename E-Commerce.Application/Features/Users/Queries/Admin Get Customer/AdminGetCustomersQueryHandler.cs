using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

internal sealed class AdminGetCustomersQueryHandler :
    IRequestHandler<AdminGetCustomersQuery, Result<CursorPagedResult<AdminCustomersResponse, Guid>>>
{
    private readonly IAppDbContext _dbContext;

    public AdminGetCustomersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CursorPagedResult<AdminCustomersResponse, Guid>>> Handle(AdminGetCustomersQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
            .Where(u => _dbContext.UserRoles
                .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => new { ur.UserId, role.Name })
                .Any(x => x.UserId == u.Id && x.Name == "Customer"));

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(u =>
                u.Email!.ToLower().Contains(search) ||
                (u.FullName).ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(u => !u.IsDeleted && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow));
            else if (request.Status.Equals("Blocked", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(u => u.LockoutEnd > DateTimeOffset.UtcNow);
            else if (request.Status.Equals("Deleted", StringComparison.OrdinalIgnoreCase))
                baseQuery = baseQuery.Where(u => u.IsDeleted);
        }

        baseQuery = baseQuery.OrderBy(u => u.Id);

        if (request.Cursor.HasValue)
            baseQuery = baseQuery.Where(u => u.Id.CompareTo(request.Cursor.Value) > 0);

        var itemsToFetch = request.Size + 1;

        var resultList = await baseQuery.Take(itemsToFetch)
            .Select(u => new AdminCustomersResponse(
                u.Id,
                u.FullName,
                u.UserName!,
                u.Email!,
                u.PhoneNumber!,
                u.ImageUrl,
                u.DateOfBirth,
                u.IsDeleted ? "Deleted" :
                (u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow) ? "Blocked" : "Active"
            )).ToListAsync(cancellationToken);

        bool hasMoreData = resultList.Count == itemsToFetch;

        if(hasMoreData) resultList.RemoveAt(resultList.Count - 1);

        var nextCursor = hasMoreData ? resultList.Last().UserId : (Guid?)null;

        var pagedResult = new CursorPagedResult<AdminCustomersResponse, Guid>(
            items: resultList,
            nextCursor: nextCursor
        );

        return Result<CursorPagedResult<AdminCustomersResponse, Guid>>.Success(pagedResult);
    }
}
