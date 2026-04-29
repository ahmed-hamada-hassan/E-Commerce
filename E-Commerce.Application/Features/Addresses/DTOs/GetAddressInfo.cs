using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Addresses.DTOs;

public record GetAddressInfo(
    Guid AddressId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string? StateOrProvince,
    string PostalCode,
    string Country,
    AddressType AddressType,
    bool IsDefault
);