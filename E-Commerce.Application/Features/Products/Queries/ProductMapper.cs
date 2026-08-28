using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Products.Queries;

public static class ProductMapper
{
    public static CustomerProductDetailsResponse ToCustomerProductDetailsResponse(this Product request, double averageRating, int totalReviews)
    {
        return new CustomerProductDetailsResponse(
            Id: request.Id,
            categoryName: request.Category.Name,
            categoryId: request.CategoryId,
            name: request.Name,
            description: request.Description,
            price: request.Price,
            sku: request.SKU,
            barcode: request.Barcode,
            isInStock: request.StockQuantity > 0,
            lowStockQuantity: request.StockQuantity <= 5 ? (byte)request.StockQuantity : null,
            maxAllowedPerOrder: (byte)Math.Min(10, request.StockQuantity),
            AverageRating: averageRating,
            TotalReviews: totalReviews,
            images: request.Images.Select(i => new ProductImageResponse
            (
                ImageUrl: i.ImageUrl,
                IsPrimary: i.IsPrimary,
                DisplayOrder: i.DisplayOrder
            )).ToList()
        );
    }
    public static VendorProductResponse ToVendorProductResponse(this Product request)
    {
        return new VendorProductResponse(
            ProductId: request.Id,
            CategoryId: request.CategoryId,
            CategoryName: request.Category.Name,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            Quantity: request.StockQuantity,
            Images: request.Images.Select(i => new ProductImageResponse
            (
                ImageUrl: i.ImageUrl,
                IsPrimary: i.IsPrimary,
                DisplayOrder: i.DisplayOrder
            )).ToList()
        );
    }
    public static AdminProductResponse ToAdminProductResponse(this Product request)
    {
        return new AdminProductResponse(
            ProductId: request.Id,
            CategoryId: request.CategoryId,
            VendorId: request.VendorId,
            CategoryName: request.Category.Name,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            Quantity: request.StockQuantity,
            PrimaryImageURL: request.MainImageUrl
        );
    }
    public static VendorArchivedProductResponse ToVendorArchivedProductResponse(this Product request)
    {
        return new VendorArchivedProductResponse(
            ProductId: request.Id,
            CategoryId: request.CategoryId,
            CategoryName: request.Category.Name,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            Quantity: request.StockQuantity,
            Images: request.Images.Select(i => new ProductImageResponse
            (
                ImageUrl: i.ImageUrl,
                IsPrimary: i.IsPrimary,
                DisplayOrder: i.DisplayOrder
            )).ToList(),
            IsDeleted: request.IsDeleted,
            DeletedOn: request.DeleteOn
        );
    }
    public static AdminArchivedProductResponse ToAdminArchivedProductResponse(this Product request)
    {
        return new AdminArchivedProductResponse(
            ProductId: request.Id,
            VendorId: request.VendorId,
            CategoryId: request.CategoryId,
            CategoryName: request.Category.Name,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            Quantity: request.StockQuantity,
            PrimaryImageURL: request.MainImageUrl,
            IsDeleted: request.IsDeleted,
            DeletedOn: request.DeleteOn
        );
    }
    public static AdminSuspendProductResponse ToAdminSuspendProductResponse(this Product request)
    {
        return new AdminSuspendProductResponse(
            ProductId: request.Id,
            VendorId: request.VendorId,
            CategoryId: request.CategoryId,
            CategoryName: request.Category.Name,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            Quantity: request.StockQuantity,
            PrimaryImageURL: request.MainImageUrl,
            IsDeleted: request.IsDeleted,
            IsDeletedByAdmin: request.DeletedByAdmin,
            DeletedOn: request.DeleteOn
        );
    }
}
