using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Command.CreateFeedback;

public record CreateFeedbackCommand(Guid UserId, Guid ProductId, byte Rating, string? Comment): IRequest<Result<Guid>>;