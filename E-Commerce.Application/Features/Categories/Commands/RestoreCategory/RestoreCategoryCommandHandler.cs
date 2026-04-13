using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Categories.Commands.RestoreCategory;

internal sealed class RestoreCategoryCommandHandler : IRequestHandler<RestoreCategoryCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public RestoreCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<bool>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetDeletedByIdAsync(request.Id, cancellationToken);

        if(category is null) return Result<bool>.Failure(CategoryErrors.NotFound);

        category.Restore();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
