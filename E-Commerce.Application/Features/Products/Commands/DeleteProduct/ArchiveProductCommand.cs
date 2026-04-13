using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Command.DeleteProduct;

public record ArchiveProductCommand(Guid ProductId, Guid VendorId) : IRequest<Result<bool>>;