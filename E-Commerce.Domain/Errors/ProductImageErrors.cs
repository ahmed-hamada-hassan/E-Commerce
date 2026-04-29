using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class ProductImageErrors
{
    public static readonly Error EmptyProductId = new("ProductImage.EmptyProductId", "Product ID cannot be empty.");
    public static readonly Error EmptyImageUrl = new("ProductImage.EmptyImageUrl", "Image URL cannot be empty.");
    public static readonly Error DisplayOrderBetween1to254 = new("ProductImage.DisplayOrderBetween1to254", "Display order must be between 1 and 254.");
    public static readonly Error AccessDenied = new("Product.AccessDenied", "You do not have permission to perform this action on the product image.");
    public static readonly Error NotFound = new("ProductImage.NotFound", "The specified product image was not found.");
    public static readonly Error UploadFaild = new("ProductImage.UploadFailed", "Failed to upload the image. Please try again.");
    public static readonly Error DeleteFaild = new("ProductImage.DeleteFailed", "Failed to delete the image. Please try again.");
    public static readonly Error CannotDeleteLastImage = new("ProductImage.CannotDeleteLastImage", "Cannot delete the last image of a product. A product must have at least one image.");
    public static readonly Error SetPrimaryFailed = new("ProductImage.SetPrimaryFailed", "Failed to set the product image as primary.");
    public static readonly Error AddFaild = new("ProductImage.AddFailed", "Failed to add the image to the product. Please try again.");
}
