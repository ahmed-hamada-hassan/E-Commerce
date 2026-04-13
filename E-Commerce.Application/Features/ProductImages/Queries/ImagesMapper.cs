using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.ProductImages.Queries;

internal static class ImagesMapper
{
    public static VendorImageDetailsResponse ToGetVendorImageDetails(this ProductImage productImage)
    {
        return new VendorImageDetailsResponse(
            ImageUrl: productImage.ImageUrl,
            IsPrimary: productImage.IsPrimary,
            DisplayOrder: productImage.DisplayOrder
            );
    }
}