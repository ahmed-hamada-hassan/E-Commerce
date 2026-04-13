using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

public record AdminRemoveImageCommand(Guid ProductId, Guid ImgaeId) : IRequest<Result<bool>>;