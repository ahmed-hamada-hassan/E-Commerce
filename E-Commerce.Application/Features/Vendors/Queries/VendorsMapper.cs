using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Vendors.Queries;

internal static class VendorsMapper
{
    public static AdminVendorResponse ToAdminVendorResponse(this Vendor vendor)
    {
        var user = vendor.User;
        return new AdminVendorResponse(
            UserId: user.Id,
            VendorId: vendor.Id,
            Name: user.FullName,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber!,
            ImageUrl: user.ImageUrl,
            StoreName: vendor.StoreName,
            CommercialRegistrationNumber: vendor.CommercialRegistrationNumber,
            IsActive: vendor.IsActive,
            DateOfBirth: user.DateOfBirth,
            IsDeleted: user.IsDeleted,
            DeletedAt: user.DeleteOn,
            IsLockout: user.LockoutEnabled,
            LockoutEnd: user.LockoutEnd,
            Status: (user.IsDeleted || vendor.IsDeleted) ? "Deleted" :
                (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) ? "Blocked" :
                !vendor.IsActive ? "PendingApproval" : "Active",
            Addresses: user.Addresses.Select(ToVendorAddressInfo).ToList());
    }

    private static VendorAddressInfo ToVendorAddressInfo(this Address address)
    {
        return new VendorAddressInfo(
            AddressId: address.Id,
            AddressLine1: address.AddressLine1,
            AddressLine2: address.AddressLine2,
            City: address.City,
            StateOrProvince: address.StateOrProvince,
            Country: address.Country,
            PostalCode: address.PostalCode,
            AddressType: address.AddressType);
    }

    public static VendorProfileResponse ToVendorProfileResponse(this Vendor vendor)
    {
        return new VendorProfileResponse(
            vendor.UserId,
            vendor.Id,
            vendor.User.FullName,
            vendor.User.UserName!,
            vendor.User.DateOfBirth,
            vendor.User.Email!,
            vendor.User.PhoneNumber!,
            vendor.User.ImageUrl,
            vendor.StoreName,
            vendor.CommercialRegistrationNumber,
            vendor.IsActive,
            vendor.User.Addresses.Select(ToVendorAddressInfo).ToList());
    }
}
