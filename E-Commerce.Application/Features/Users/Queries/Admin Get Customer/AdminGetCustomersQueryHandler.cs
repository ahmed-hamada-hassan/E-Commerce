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
        var baseQuery = _dbContext.Users.IgnoreQueryFilters().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(u =>
                u.Email!.ToLower().Contains(search) ||
                (u.FirstName + " " + u.LastName).ToLower().Contains(search));
        }

        var query = from user in baseQuery
                    join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId into ur
                    from userRole in ur.DefaultIfEmpty()
                    join role in _dbContext.Roles on userRole.RoleId equals role.Id into r
                    from role in r.DefaultIfEmpty()
                    select new { User = user, RoleName = role != null ? role.Name : "User" };

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(x => x.RoleName == request.Role);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                query = query.Where(x => !x.User.IsDeleted && (x.User.LockoutEnd == null || x.User.LockoutEnd <= DateTimeOffset.UtcNow));
            else if (request.Status.Equals("Blocked", StringComparison.OrdinalIgnoreCase))
                query = query.Where(x => x.User.LockoutEnd > DateTimeOffset.UtcNow);
            else if (request.Status.Equals("Deleted", StringComparison.OrdinalIgnoreCase))
                query = query.Where(x => x.User.IsDeleted);
        }

        query = query.OrderBy(x => x.User.Id);

        if (request.Cursor.HasValue)
        {
            query = query.Where(x => x.User.Id.CompareTo(request.Cursor.Value) > 0);
        }

        var itemsToFetch = request.Size + 1;

        var resultList = await query.Take(itemsToFetch)
            .Select(x => new AdminCustomersResponse(
                x.User.Id,
                x.User.FullName,
                x.User.UserName!,
                x.User.Email!,
                x.User.PhoneNumber!,
                x.User.ImageUrl,
                x.User.DateOfBirth,
                x.User.IsDeleted ? "Deleted" : 
                (x.User.LockoutEnd != null && x.User.LockoutEnd > DateTimeOffset.UtcNow) ? "Blocked" : "Active"
            )).ToListAsync(cancellationToken);

 
        var nextCursor = resultList.Count > 0 ? resultList.Last().UserId : (Guid?)null;

        var pagedResult = new CursorPagedResult<AdminCustomersResponse, Guid>(
            items: resultList,
            nextCursor: nextCursor
        );

        return Result<CursorPagedResult<AdminCustomersResponse, Guid>>.Success(pagedResult);
    }
}
