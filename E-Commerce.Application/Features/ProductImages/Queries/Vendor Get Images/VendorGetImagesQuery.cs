using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Queries.Vendor_Get_Images;

public record VendorGetImagesQuery(Guid ProductId, Guid VendorId) : IRequest<Result<IReadOnlyCollection<VendorImageDetailsResponse>>>;