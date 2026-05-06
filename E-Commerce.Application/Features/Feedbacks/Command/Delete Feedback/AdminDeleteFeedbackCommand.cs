using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;

public record AdminDeleteFeedbackCommand(Guid AdminId, Guid FeedbackId) : IRequest<Result<bool>>;