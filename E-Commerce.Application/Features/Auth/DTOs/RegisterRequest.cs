using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Auth.DTOs;

public record RegisterRequest(string Email, string Password, string FirstName, string? MiddleName, string LastName, string UserName,
    IFormFile? Image, string PhoneNumber, DateOnly DateOfBirth, string Role);