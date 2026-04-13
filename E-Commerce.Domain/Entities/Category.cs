using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Category : SoftDeletable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid? ParentCategoryId {  get; private set; }
    public string? ImageUrl { get; private set; }

    public Category? ParentCategory { get; private set; }
    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    
    //private readonly List<Product> _products = new();
    //public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category(Guid id, string name, string? description, Guid? parentCategoryId, string? imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        ParentCategoryId = parentCategoryId;
        ImageUrl = imageUrl;
    }

    protected Category() { }

    public static Result<Category> Create(string name, string? description, Guid? parentCategoryId, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Category>.Failure(CategoryErrors.EmptyCategoryName);

        var category = new Category(Guid.NewGuid(), name, description, parentCategoryId, imageUrl);
        return Result<Category>.Success(category);
    }

    public Result<bool> Update(string name, string? description, Guid? parentCategoryId, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<bool>.Failure(CategoryErrors.EmptyCategoryName);

        if (Id == parentCategoryId)
            return Result<bool>.Failure(CategoryErrors.InvalidParentCategory);

        Name = name;
        Description = description;
        ParentCategoryId = parentCategoryId;
        ImageUrl = imageUrl;

        return Result<bool>.Success(true);
    }

    
}
