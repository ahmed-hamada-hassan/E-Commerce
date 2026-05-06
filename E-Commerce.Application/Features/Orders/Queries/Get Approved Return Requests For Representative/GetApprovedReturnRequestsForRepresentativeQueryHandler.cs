using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Orders.Queries.Get_Approved_Return_Requests_For_Representative;

internal sealed class GetApprovedReturnRequestsForRepresentativeQueryHandler :
    IRequestHandler<GetApprovedReturnRequestsForRepresentativeQuery, Result<CursorPagedResult<ApprovedReturnRequestResponse, Guid>>>
{
    private readonly IAppDbContext _dbContext;

    public GetApprovedReturnRequestsForRepresentativeQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CursorPagedResult<ApprovedReturnRequestResponse, Guid>>> Handle(
        GetApprovedReturnRequestsForRepresentativeQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.ReturnRequests
            .AsNoTracking()
            .Where(rr => rr.Status == ReturnStatus.Approved);

        if (request.Cursor.HasValue && request.Cursor.Value != Guid.Empty)
        {
            var cursorDate = await _dbContext.ReturnRequests
                .AsNoTracking()
                .Where(rr => rr.Id == request.Cursor.Value)
                .Select(rr => rr.RequestedDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorDate != default)
                query = query.Where(rr => rr.RequestedDate < cursorDate);
        }

        var returnRequests = await query
            .Join(_dbContext.Orders.AsNoTracking(), rr => rr.OrderId, o => o.Id, (rr, o) => new { rr, o })
            .Join(_dbContext.Users.AsNoTracking(), ro => ro.o.UserId, u => u.Id, (ro, u) => new { ro, u })
            .Join(_dbContext.OrderItems.AsNoTracking(),
                  r => new { r.ro.rr.OrderId, r.ro.rr.ProductId },
                  oi => new { oi.OrderId, oi.ProductId },
                  (r, oi) => new { r, oi })
            .Join(_dbContext.Products.AsNoTracking(), p => p.r.ro.rr.ProductId, prod => prod.Id, (r_oi, p) => new { r_oi, p })
            .OrderByDescending(x => x.r_oi.r.ro.rr.RequestedDate)
            .Take(request.Size + 1) 
            .Select(x => new ApprovedReturnRequestResponse(
                x.r_oi.r.ro.rr.Id,
                x.r_oi.r.ro.o.Id,
                x.r_oi.r.u.FullName, 
                x.p.Name,
                x.r_oi.r.ro.rr.Quantity,
                x.r_oi.oi.UnitPrice,
                x.r_oi.r.ro.rr.Quantity * x.r_oi.oi.UnitPrice, 
                x.r_oi.r.ro.rr.Reason,
                x.r_oi.r.ro.rr.RequestedDate
            ))
            .ToListAsync(cancellationToken);

        bool hasNextPage = returnRequests.Count > request.Size;
        Guid? nextCursor = null;

        if (hasNextPage)
        {
            nextCursor = returnRequests[request.Size - 1].ReturnRequestId;
            returnRequests.RemoveAt(request.Size);
        }

        var result = new CursorPagedResult<ApprovedReturnRequestResponse, Guid>(returnRequests, nextCursor);
        return Result<CursorPagedResult<ApprovedReturnRequestResponse, Guid>>.Success(result);
    }
}
