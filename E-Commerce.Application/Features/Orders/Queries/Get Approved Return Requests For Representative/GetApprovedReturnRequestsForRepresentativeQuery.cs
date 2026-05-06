using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Get_Approved_Return_Requests_For_Representative;

public record GetApprovedReturnRequestsForRepresentativeQuery(Guid? Cursor, int Size) :
    IRequest<Result<CursorPagedResult<ApprovedReturnRequestResponse, Guid>>>, ICursorPaginationRequest<Guid>;