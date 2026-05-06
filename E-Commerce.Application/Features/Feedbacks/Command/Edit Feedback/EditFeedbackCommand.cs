using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Command.Edit_Feedback;

public record EditFeedbackCommand(Guid UserId, Guid FeedbackId, byte? Rating, string? Comment) : IRequest<Result<bool>>;