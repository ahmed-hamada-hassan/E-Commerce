namespace E_Commerce.Application.Features.Users.DTOs;

public record AdminCustomerProfileResponse(
    Guid UserId,
    string Name,
    string UserName,
    string Email,
    string? PhoneNumber,
    string? ImageUrl,
    DateOnly DateOfBirth,
    Guid? DefaultShippingAddressId,
    IReadOnlyList<AdminCustomerAddressInfo> Addresses);
