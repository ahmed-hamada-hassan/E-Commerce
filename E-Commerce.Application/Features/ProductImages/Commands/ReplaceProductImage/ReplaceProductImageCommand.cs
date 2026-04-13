using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReplaceProductImage;

public record ReplaceProductImageCommand(Guid ProductId, Guid VendorId, Guid ImageId, IFormFile NewImage) : IRequest<Result<bool>>;