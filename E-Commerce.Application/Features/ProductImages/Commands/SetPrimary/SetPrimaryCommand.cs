using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.SetPrimary;

public record SetPrimaryCommand(Guid ProductId, Guid VendorId, Guid ImageId) : IRequest<Result<bool>>;