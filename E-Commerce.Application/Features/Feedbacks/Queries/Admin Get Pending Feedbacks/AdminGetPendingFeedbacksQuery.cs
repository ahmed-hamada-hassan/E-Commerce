using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Admin_Get_Pending_Feedbacks;

public record AdminGetPendingFeedbacksQuery(Guid? Cursor, int Size) : IRequest<Result<CursorPagedResult<PendingFeedbackResponse, Guid>>>, ICursorPaginationRequest<Guid>;