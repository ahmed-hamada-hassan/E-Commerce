using E_Commerce.Application.Features.Feedbacks.Command.Edit_Feedback;
using E_Commerce.Application.Features.Feedbacks.DTOs;

namespace E_Commerce.API.Contracts;

public static class FeedbackMappingExtensions
{
    public static EditFeedbackCommand ToEditFeedbackCommand(this UpdateProductFeedbackRequest request, Guid userId, Guid feedbackId)
    {
        return new EditFeedbackCommand(userId, feedbackId, request.Rating, request.Comment);
    }
}
