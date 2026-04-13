using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Auth.DTOs;

public record RegisterVendorRequest(string Email, string Password, string FirstName, string? MiddleName, string LastName, string UserName,
    string StoreName, string CommercialRegistrationNumber, IFormFile? Image, string PhoneNumber, DateOnly DateOfBirth);