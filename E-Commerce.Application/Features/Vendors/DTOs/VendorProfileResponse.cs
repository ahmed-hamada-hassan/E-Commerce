namespace E_Commerce.Application.Features.Vendors.DTOs;

public record VendorProfileResponse(
    Guid UserId,
    Guid VendorId,
    string Name,
    string UserName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string? ImageUrl,
    string StoreName,
    string CommercialRegistrationNumber,
    bool IsActive,
    List<VendorAddressInfo> Addresses);