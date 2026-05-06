namespace E_Commerce.Application.Features.Feedbacks.DTOs;

public record UpdateProductFeedbackRequest(byte Rating, string? Comment);