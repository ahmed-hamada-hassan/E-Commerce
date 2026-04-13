using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Queries.Admin_Get_Images;

public record AdminGetImagesQuery(Guid ProductId, Guid? VendorId) : IRequest<Result<IReadOnlyCollection<AdminImageDetailsResponse>>>;