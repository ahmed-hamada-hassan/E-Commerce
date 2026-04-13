using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class ProductErrors
{
    public static readonly Error EmptyCategoryId = new("Product.EmptyCategoryId", "CategoryId cannot be empty.");
    public static readonly Error EmptyVendorId = new("Product.EmptyVendorId", "VendorId cannot be empty.");
    public static readonly Error EmptyProductName = new("Product.EmptyProductName", "Product name cannot be empty.");
    public static readonly Error PriceMustBeGreaterThanZero = new("Product.PriceMustBeGreaterThanZero", "Price must be greater than zero.");
    public static readonly Error EmptySKU = new("Product.EmptySKU", "SKU cannot be empty.");
    public static readonly Error StockQuantityCannotBeNegative = new("Product.StockQuantityCannotBeNegative", "Stock quantity cannot be negative.");
    public static readonly Error ProductNotFound = new("Product.ProductNotFound", "Product not found.");
    public static readonly Error DeletedFailed = new("Product.DeletedFailed", "An error occurred while deleting the product.");
    public static readonly Error Ordered = new("Product.Ordered", "Cannot delete product because it has been ordered.");
    public static readonly Error HasActiveOrder =new("Product.ActiveOrders", "Cannot delete product because it has active/pending orders.");
    public static readonly Error InsufficientStock = new("Product.InsufficientStock", "Insufficient stock for the requested quantity.");
    public static readonly Error InvalidQuantity = new("Product.InvalidQuantity", "Quantity must be greater than zero.");
    public static readonly Error AccessDenied = new("Product.AccessDenied", "You do not have permission to perform this action on the product.");
    public static readonly Error DuplicateSKU = new("Product.DuplicateSKU", "The SKU already exists for another product.");
}