using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Get_Product_Feedbacks;

public record GetProductFeedbacksQuery(Guid ProductId, Guid? Cursor, int Size) : 
    IRequest<Result<CursorPagedResult<ProductFeedbackResponse, Guid>>>, ICursorPaginationRequest<Guid>;