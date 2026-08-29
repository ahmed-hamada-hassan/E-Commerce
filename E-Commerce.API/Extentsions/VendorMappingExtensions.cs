using E_Commerce.Application.Features.Vendors.Commands.Update_Vendor;
using E_Commerce.Application.Features.Vendors.DTOs;

namespace E_Commerce.API.Contracts;

internal static class VendorMappingExtensions
{
    public static UpdateVendorCommand ToUpdateVendorStoreCommand(this UpdateVendorStoreRequest request, Guid vendorId)
    {
        return new UpdateVendorCommand(vendorId,
            string.IsNullOrWhiteSpace(request.StoreName) ? null : request.StoreName,
            string.IsNullOrWhiteSpace(request.CommercialRegistrationNumber) ? null : request.CommercialRegistrationNumber);
    }
}
