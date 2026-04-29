using E_Commerce.Application.Features.ProductImages.Commands.ReplaceProductImage;
using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Features.Products.Command.CreateProduct;
using E_Commerce.Application.Features.Products.Command.UpdateProduct;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Features.Products.Queries.GetProduct;
using E_Commerce.Application.Features.Products.Queries.GetProducts;

namespace E_Commerce.API.Contracts;

public static class ProductMappingExtensions
{
    public static GetProductsQuery ToGetProductQuery (this CustomerProductsRequest request)
    {
        return new GetProductsQuery(
            SearchTerm: request.SearchTerm,
            MinPrice: request.MinPrice,
            MaxPrice: request.MaxPrice,
            SortBy: request.SortBy,
            Page: request.Page,
            Size: request.Size
        );
    }

    public static CreateProductCommand ToCreateProductCommand(this AddProductRequest request, Guid vendorId)
    {
        return new CreateProductCommand(
            VendorId: vendorId,
            Name: request.Name,
            CategoryId: request.CategoryId,
            Description: request.Description,
            Price: request.Price,
            SKU: request.SKU,
            Barcode: request.Barcode,
            StockQuantity: request.StockQuantity
        );
    }

    public static UpdateProductCommand ToUpdateProductCommand(this UpdateProductRequest request, Guid productId, Guid vendorId)
    {
        return new UpdateProductCommand(
            vendorId,
            productId,
            request.CategoryId,
            string.IsNullOrWhiteSpace(request.Name) ? null : request.Name,
            request.Description,
            request.Price,
            string.IsNullOrWhiteSpace(request.SKU) ? null : request.SKU,
            string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode,
            request.Quantity
        );
    }

    public static ReorderImage ToReorderImage(this ReorderImageRequest request)
    {
        return new ReorderImage(
            imageId: request.imageId,
            displayOrder: request.displayOrder
        );
    }
    public static ImageDTO ToImageDTO(this ImageRequest request)
    {
        return new ImageDTO(
            Image: request.Image,
            IsPrimary: request.IsPrimary,
            DisplayOrder: request.DisplayOrder
        );
    }
}
