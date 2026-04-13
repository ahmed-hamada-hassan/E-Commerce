using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReorderProductImage;

public record ReorderProductImageCommand(Guid ProductId, Guid VendorId, IEnumerable<ReorderImage> Images) : IRequest<Result<bool>>;