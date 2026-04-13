using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Command.RestoreProduct;

public record UnSuspendProductCommand(Guid ProductId) : IRequest<Result<bool>>;
