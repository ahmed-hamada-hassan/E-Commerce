namespace E_Commerce.Application.Features.Users.DTOs;

public record AdminCustomerResponse(
    Guid UserId,
    string Name,
    string UserName,
    string Email,
    string PhoneNumber,
    string? ImageUrl,
    DateOnly DateOfBirth,
    Guid? DefaultShippingAddressId,
    bool IsDeleted,
    DateTimeOffset? DeletedAt,
    bool IsBlocked,
    DateTimeOffset? BlockedAt,
    string Status,
    List<AdminCustomerAddressInfo> Addresses);