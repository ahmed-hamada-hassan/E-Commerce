namespace E_Commerce.Application.Features.Users.DTOs;

public record UpdateCustomerInfoRequest(string? FirstName, string? MiddleName, string? LastName, string? Email, string? UserName,
    string? PhoneNumber, DateOnly? DateOfBirth);