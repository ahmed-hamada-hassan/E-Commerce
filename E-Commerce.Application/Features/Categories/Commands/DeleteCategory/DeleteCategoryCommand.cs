using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result<bool>>;
