using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class ProductImage
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public string ImageUrl { get; private set; } = null!;
    public bool IsPrimary { get; private set; } = false;
    public byte DisplayOrder { get; private set; }

    private ProductImage(Guid id, Guid productId, string imageUrl, bool isPrimary, byte displayOrder)
    {
        Id = id;
        ProductId = productId;
        ImageUrl = imageUrl;
        IsPrimary = isPrimary;
        DisplayOrder = displayOrder;
    }

    protected ProductImage() { }

    public static Result<ProductImage> Create(Guid productId, string imageUrl, bool isPrimary, byte displayOrder)
    {
        if (productId == Guid.Empty)
            return Result<ProductImage>.Failure(ProductImageErrors.EmptyProductId);
        if (string.IsNullOrWhiteSpace(imageUrl))
            return Result<ProductImage>.Failure(ProductImageErrors.EmptyImageUrl);
        if (displayOrder < 1 || displayOrder > 254)
            return Result<ProductImage>.Failure(ProductImageErrors.DisplayOrderBetween1to254);

        var productImage = new ProductImage(Guid.NewGuid(), productId, imageUrl, isPrimary, displayOrder);
        return Result<ProductImage>.Success(productImage);
    }

    internal void SetPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
    }

    public void UpdateUrl(string newUrl)
    {
        ImageUrl = newUrl;
    }

    public void SetDisplayOrder(byte displayOrder)
    {
        DisplayOrder = displayOrder; 
    }
}
