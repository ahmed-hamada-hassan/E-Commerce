namespace E_Commerce.Application.Features.Users.DTOs;

public record AdminCustomersResponse(Guid UserId, string Name, string UserName,
    string Email, string PhoneNumber, string? ImageUrl, DateOnly DateOfBirth, string Status);