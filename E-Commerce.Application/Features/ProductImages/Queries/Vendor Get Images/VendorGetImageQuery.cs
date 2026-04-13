using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Queries.Vendor_Get_Images;

public record VendorGetImageQuery(Guid ImageId, Guid VendorId, Guid ProductId) : IRequest<Result<VendorImageDetailsResponse>>;