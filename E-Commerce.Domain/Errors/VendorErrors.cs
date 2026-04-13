using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class VendorErrors
{
    public static readonly Error EmptyStoreName = new("Vendor.StoreName.Empty", "Store name cannot be empty.");
    public static readonly Error EmptyCommercialRegistrationNumber = new("Vendor.CommercialRegistrationNumber.Empty", 
        "Commercial registration number cannot be empty.");
    public static readonly Error NotFound = new("Vendor.NotFound", "Vendor not found.");
    public static readonly Error NotActive = new("Vendor.NotActive", "Vendor is not active.");
    public static readonly Error DuplicateStoreName = new("Vendor.DuplicateStoreName", "Store name must be unique.");
    public static readonly Error DuplicateCommercialRegistrationNumber = new("Vendor.DuplicateCommercialRegistrationNumber", 
        "Commercial registration number must be unique.");
}
