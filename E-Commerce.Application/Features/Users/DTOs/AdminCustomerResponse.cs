namespace E_Commerce.Application.Features.Users.DTOs;

public record AdminCustomerResponse(
    Guid UserId,
    string Name,
    string UserName,
    string Email,
    string PhoneNumber,
    string? ImageUrl,
    DateOnly DateOfBirth,
    bool IsDeleted,
    DateTimeOffset? DeletedAt,
    bool IsLockout,
    DateTimeOffset? LockoutEnd,
    string Status,
    List<CustomerAddressInfo> Addresses);