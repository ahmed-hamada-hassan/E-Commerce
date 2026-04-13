using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Commands.AddImage;

public record AddImageCommand(Guid ProductId, Guid VendorId, IEnumerable<ImageResponse> Images) : IRequest<Result<List<Guid>>>;