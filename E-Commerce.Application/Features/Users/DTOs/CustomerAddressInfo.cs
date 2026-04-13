using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Users.DTOs;

public record CustomerAddressInfo(
    Guid AddressId, 
    string AddressLine1, 
    string? AddressLine2, 
    string City,
    string? StateOrProvince, 
    string Country, 
    string PostalCode,
    AddressType AddressType);