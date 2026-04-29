using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Users.Queries;

public static class UsersMapper
{
    public static CustomerProfileResponse ToCustomerProfileResponse(ApplicationUser user) =>
        new(
            UserId: user.Id,
            Name: user.FullName,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber,
            ImageUrl: user.ImageUrl,
            DateOfBirth: user.DateOfBirth,
            DefaultShippingAddressId: user.DefaultShippingAddressId,
            Addresses: user.Addresses.Select(ToCustomerAddressInfo).ToList());

    public static AdminCustomerResponse ToAdminCustomerResponse(ApplicationUser user) =>
        new(
            UserId: user.Id,
            Name: user.FullName,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber!,
            ImageUrl: user.ImageUrl,
            DateOfBirth: user.DateOfBirth,
            IsDeleted: user.IsDeleted,
            DeletedAt: user.DeleteOn,
            IsLockout: user.LockoutEnabled,
            LockoutEnd: user.LockoutEnd,
            Status: user.IsDeleted ? "Deleted" :
                (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) ? "Locked" : "Active",
            Addresses: user.Addresses.Select(ToCustomerAddressInfo).ToList());

    private static CustomerAddressInfo ToCustomerAddressInfo(Address address) =>
        new(
            AddressId: address.Id,
            AddressLine1: address.AddressLine1,
            AddressLine2: address.AddressLine2,
            City: address.City,
            StateOrProvince: address.StateOrProvince,
            Country: address.Country,
            PostalCode: address.PostalCode,
            AddressType: address.AddressType);
}
