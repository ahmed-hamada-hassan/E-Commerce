using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

public record VendorRemoveImageCommand(Guid ProductId, Guid VendorId, Guid ImgaeId) : IRequest<Result<bool>>;
