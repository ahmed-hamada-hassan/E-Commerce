using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;

public record CompleteReturnRequestCommand(Guid ReturnRequestId, Guid RepresentativeId, ReturnStatus Status, string Reason) : IRequest<Result<bool>>;