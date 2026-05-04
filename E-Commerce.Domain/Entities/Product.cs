using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Product : SoftDeletable
{
    public Guid Id { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid VendorId { get; private set; }
    public Vendor Vendor { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string SKU { get; private set; } = null!;
    public string? Barcode { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public int StockQuantity { get; private set; }
    public bool DeletedByAdmin { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    public string? MainImageUrl => Images.FirstOrDefault(p => p.IsPrimary)?.ImageUrl ?? Images.FirstOrDefault()?.ImageUrl;

    // This will load feedbacks and cart items lazily when accessed, which can help with performance if they are not always needed.
    //private readonly List<Feedback> _feedbacks = new();
    //private readonly List<CartItem> _cartItems = new();
    //public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks.AsReadOnly();
    //public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private Product(Guid id, Guid categoryId, Guid vendorId, string name, string? description, decimal price, 
        string sku, string? barcode, int stockQuantity, DateTimeOffset createdOn)
    {
        Id = id;
        CategoryId = categoryId;
        VendorId = vendorId;
        Name = name;
        Description = description;
        Price = price;
        SKU = sku;
        Barcode = barcode;
        StockQuantity = stockQuantity;
        CreatedOn = createdOn;
    }

    protected Product() { }

    public static Result<Product> Create(Guid categoryId, Guid vendorId, string name, string? description, 
        decimal price, string sku, string? barcode, int stockQuantity)
    {
        if (categoryId == Guid.Empty)
            return Result<Product>.Failure(ProductErrors.EmptyCategoryId);

        if (vendorId == Guid.Empty)
            return Result<Product>.Failure(ProductErrors.EmptyVendorId);

        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure(ProductErrors.EmptyProductName);

        if (price <= 0)
            return Result<Product>.Failure(ProductErrors.PriceMustBeGreaterThanZero);

        if (string.IsNullOrWhiteSpace(sku))
            return Result<Product>.Failure(ProductErrors.EmptySKU);

        if (stockQuantity < 0)
            return Result<Product>.Failure(ProductErrors.StockQuantityCannotBeNegative);

        var product = new Product(Guid.NewGuid(), categoryId, vendorId, name, description, 
            price, sku, barcode, stockQuantity, DateTimeOffset.UtcNow);
        return Result<Product>.Success(product);
    }

    public Result<bool> Update(Guid categoryId, string? name, string? description, decimal? price, string? sku, string? barcode, int? stockQuantity)
    {
        if(categoryId != Guid.Empty)
            CategoryId = categoryId;

        if(!string.IsNullOrWhiteSpace(name))
            Name = name;

        if(!string.IsNullOrWhiteSpace(description))
            Description = description;

        if(price.HasValue && !(price <= 0))
            Price = price.Value;

        if (stockQuantity.HasValue && !(stockQuantity < 0))
            StockQuantity = stockQuantity.Value;

        if(!string.IsNullOrWhiteSpace(sku))
            SKU = sku;

        if(!string.IsNullOrWhiteSpace(barcode))
            Barcode = barcode;

        return Result<bool>.Success(true);
    }
    public Result<bool> DeductStock(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure(ProductErrors.InvalidQuantity);
        if (quantity > StockQuantity)
            return Result<bool>.Failure(ProductErrors.InsufficientStock);
        StockQuantity -= quantity;
        return Result<bool>.Success(true);
    }
    public Result<bool> AddStock(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure(ProductErrors.InvalidQuantity);
        StockQuantity += quantity;
        return Result<bool>.Success(true);
    }
    public Result<bool> AddImage(string imageUrl, bool isPrimary, byte displayOrder)
    {
        if (isPrimary)
        {
            // If the new image is set as primary, unset the current primary image
            var currentPrimary = Images.FirstOrDefault(i => i.IsPrimary);
            if (currentPrimary != null)
            {
                _images.ForEach(img => img.SetPrimary(false));
            }
        }
        var productImage = ProductImage.Create(Id, imageUrl, isPrimary, displayOrder);
        if (productImage.IsFailure) return Result<bool>.Failure(productImage.Error);

        _images.Add(productImage.Value!);
        return Result<bool>.Success(true);
    }

    public void ClearImages()
    {
        _images.Clear(); 
    }

    public void SuspendByAdmin()
    {
        DeletedByAdmin = true;
        IsDeleted = true;
        DeleteOn = DateTimeOffset.UtcNow;
    }
    public void UnSuspendByAdmin()
    {
        DeletedByAdmin = false;
        IsDeleted = false;
        DeleteOn = null;
    }

    public void ArchiveByVendor()
    {
        IsDeleted = true;
        DeleteOn = DateTimeOffset.UtcNow;
    }
    public void RestoreByVendor()
    {
        IsDeleted = false;
        DeleteOn = null;
    }
    public Result<bool> Restock(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure(ProductErrors.InvalidQuantity);

        StockQuantity += quantity;
        return Result<bool>.Success(true);
    }
}
