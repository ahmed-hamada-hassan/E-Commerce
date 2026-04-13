using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Commands.RestoreCategory;

public record RestoreCategoryCommand(Guid Id) : IRequest<Result<bool>>;
