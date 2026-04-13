using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class CategoryErrors
{
    public static readonly Error EmptyCategoryName = new("Category.EmptyName", "Category name cannot be empty.");
    public static readonly Error NotFound = new("Category.NotFound", "The category you are trying to delete does not exist.");
    public static readonly Error HasRelatedProducts = new("Category.HasRelatedProducts", "Cannot delete category because it contains products.");
    public static readonly Error DeleteFailed = new("Category.DeleteFailure", "An error occurred while deleting the category.");
    public static readonly Error InvalidParentCategory = new("Category.InvalidParentCategory", "A category cannot be its own parent.");
    public static readonly Error UploadImageFailed = new("Category.UploadImageFailed", "An error occurred while uploading the category image.");
}
