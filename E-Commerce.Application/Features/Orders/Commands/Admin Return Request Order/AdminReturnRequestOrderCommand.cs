using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Admin_Return_Request_Order;

public record AdminReturnRequestOrderCommand(Guid ReturnRequestId, ReturnStatus Status) : IRequest<Result<bool>>;