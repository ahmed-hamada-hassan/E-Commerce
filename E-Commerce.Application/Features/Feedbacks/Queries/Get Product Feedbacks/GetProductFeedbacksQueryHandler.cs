using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Get_Product_Feedbacks;

internal sealed class GetProductFeedbacksQueryHandler : 
    IRequestHandler<GetProductFeedbacksQuery, Result<CursorPagedResult<ProductFeedbackResponse, Guid>>>
{
    private readonly IAppDbContext _dbContext;

    public GetProductFeedbacksQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CursorPagedResult<ProductFeedbackResponse, Guid>>> Handle(GetProductFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.ProductId == request.ProductId && r.IsApproved && !r.IsDeleted);

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
        .OrderByDescending(x => x.r.CreatedDate)
        .Take(request.Size + 1)
        .Select(x => new ProductFeedbackResponse(
            x.r.Id,
            x.u.FullName,
            x.r.Rating,
            x.r.Comment,
            x.r.CreatedDate,
            x.r.IsVerifiedPurchase))
        .ToListAsync(cancellationToken);

        bool hasNextPage = feedbacks.Count > request.Size;
        Guid? nextCursor = null;

        if (hasNextPage)
        {
            nextCursor = feedbacks[request.Size - 1].FeedbackId;
            feedbacks.RemoveAt(request.Size);
        }

        var result = new CursorPagedResult<ProductFeedbackResponse, Guid>(feedbacks, nextCursor);
        return Result<CursorPagedResult<ProductFeedbackResponse, Guid>>.Success(result);
    }
}
