using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Admin_Get_Pending_Feedbacks;

internal sealed class AdminGetPendingFeedbacksQueryHandler :
    IRequestHandler<AdminGetPendingFeedbacksQuery, Result<CursorPagedResult<PendingFeedbackResponse, Guid>>>
{
    private readonly IAppDbContext _dbContext;

    public AdminGetPendingFeedbacksQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CursorPagedResult<PendingFeedbackResponse, Guid>>> Handle(AdminGetPendingFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Reviews
            .AsNoTracking()
            .Where(r => !r.IsApproved && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedDate)
            .AsQueryable();

        if (request.Cursor.HasValue && request.Cursor.Value != Guid.Empty)
        {
            var cursorReviewDate = await _dbContext.Reviews
                .Where(r => r.Id == request.Cursor)
                .Select(r => r.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorReviewDate != default)
                query = query.Where(r => r.CreatedDate < cursorReviewDate);
        }

        var feedbacks = await query
        .Join(_dbContext.Users, r => r.UserId, u => u.Id, (r, u) => new { r, u })
        .Join(_dbContext.Products, ru => ru.r.ProductId, p => p.Id, (ru, p) => new { ru.r, ru.u, p })
        .OrderByDescending(x => x.r.CreatedDate)
        .Take(request.Size + 1)
        .Select(x => new PendingFeedbackResponse(
            x.u.Id,
            x.r.Id,
            x.r.ProductId,
            x.p.Name,
            x.u.FullName!,
            x.r.Rating,
            x.r.Comment,
            x.r.CreatedDate))
        .ToListAsync(cancellationToken);

        bool hasNextPage = feedbacks.Count > request.Size;
        Guid? nextCursor = null;

        if (hasNextPage)
        {
            nextCursor = feedbacks[request.Size - 1].FeedbackId;
            feedbacks.RemoveAt(request.Size);
        }

        var result = new CursorPagedResult<PendingFeedbackResponse, Guid>(feedbacks, nextCursor);
        return Result<CursorPagedResult<PendingFeedbackResponse, Guid>>.Success(result);
    }
}
