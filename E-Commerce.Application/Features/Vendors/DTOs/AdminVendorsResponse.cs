namespace E_Commerce.Application.Features.Vendors.DTOs;

public record AdminVendorsResponse(
    Guid UserId,
    Guid VendorId,
    string Name,
    string UserName,
    string Email,
    string PhoneNumber,
    string? ImageUrl,
    string StoreName,
    string CommercialRegistrationNumber,
    bool IsActive,
    DateOnly DateOfBirth,
    bool IsDeleted,
    DateTimeOffset? DeletedAt,
    bool IsLockout,
    DateTimeOffset? LockoutEnd,
    string Status);