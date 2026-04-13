using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileService _fileService;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IFileService fileService, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category is null)
            return Result<bool>.Failure(CategoryErrors.NotFound);

        string? imageUploadUrl = null;
        if (request.Image is not null)
        {
            imageUploadUrl = await _fileService.UploadImageAsync(request.Image);
            if (string.IsNullOrEmpty(imageUploadUrl))
            {
                _logger.LogError("INFRASTRUCTURE ERROR: Image upload failed during category updating for Category Name: {CategoryName}", request.Name);
                return Result<bool>.Failure(CategoryErrors.UploadImageFailed);
            }
            else
            {
                if(!string.IsNullOrEmpty(category.ImageUrl))
                    await _fileService.DeleteImageAsync(category.ImageUrl);
            }
        }

        var result = category.Update(request.Name, request.Description, request.ParentId, imageUploadUrl);
        if (result.IsFailure) return Result<bool>.Failure(result.Error);

        await _categoryRepository.UpdateAsync(category);

        return result;
    }
}