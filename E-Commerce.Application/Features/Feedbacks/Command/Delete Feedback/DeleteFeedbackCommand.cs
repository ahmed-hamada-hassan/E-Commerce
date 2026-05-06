using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;

public record DeleteFeedbackCommand(Guid UserId, Guid FeedbackId) : IRequest<Result<bool>>;