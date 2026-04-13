using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Command.RestoreProduct;

public record RestoreProductCommand(Guid ProductId, Guid VendorId) : IRequest<Result<bool>>;