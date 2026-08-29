using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, 
        IFileService fileService, ILogger<CreateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        string? imageUploadUrl = null;
        if(request.Image is not null)
        {
            imageUploadUrl = await _fileService.UploadImageAsync(request.Image);
            if (string.IsNullOrEmpty(imageUploadUrl))
            {
                _logger.LogError("INFRASTRUCTURE ERROR: Image upload failed during category creation for Category Name: {CategoryName}", request.Name);
                return Result<Guid>.Failure(CategoryErrors.UploadImageFailed);
            }
        }
        var category = Category.Create(request.Name, request.Description, request.ParentCategoryId, imageUploadUrl);

        if (category.IsFailure)
            return Result<Guid>.Failure(category.Error);

        var newCategory = category.Value!;
        var newCategoryId = await _categoryRepository.AddAsync(newCategory, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(newCategoryId);
    }
}
