using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

public record AdminClearImagesCommand(Guid ProductId) : IRequest<Result<bool>>;