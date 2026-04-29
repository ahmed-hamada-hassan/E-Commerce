using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Vendors.DTOs;

public record VendorAddressInfo(
    Guid AddressId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string? StateOrProvince,
    string Country,
    string PostalCode,
    AddressType AddressType);