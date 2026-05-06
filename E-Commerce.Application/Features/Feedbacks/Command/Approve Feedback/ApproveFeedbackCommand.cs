using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Feedbacks.Command.Approve_Feedback;

public record ApproveFeedbackCommand(Guid AdminId, Guid FeedbackId) : IRequest<Result<bool>>;