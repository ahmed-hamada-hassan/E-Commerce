using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Vendor : SoftDeletable
{
    public Guid Id { get; private set; }
    public string StoreName { get; private set; } = null!;
    public string CommercialRegistrationNumber { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public Guid UserId { get; private set; }

    public ApplicationUser User { get; private set; } = null!;

    protected Vendor() { } 
    private Vendor(Guid Id, string storeName, string commercialRegistrationNumber, Guid userId)
    {
        StoreName = storeName;
        CommercialRegistrationNumber = commercialRegistrationNumber;
        IsActive = false;
        UserId = userId;
    }

    public static Result<Vendor> Create(string storeName, string CrNumber, Guid userId)
    {
        if(string.IsNullOrWhiteSpace(storeName))
            return Result<Vendor>.Failure(VendorErrors.EmptyStoreName);
        if(string.IsNullOrWhiteSpace(CrNumber))
            return Result<Vendor>.Failure(VendorErrors.EmptyCommercialRegistrationNumber);

        var vendor = new Vendor(Guid.NewGuid(), storeName, CrNumber, userId);

        return Result<Vendor>.Success(vendor);
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void UpdateStoreName(string storeName)
    {
        if (!string.IsNullOrWhiteSpace(storeName))
            StoreName = storeName;
    }
    public void UpdateCommercialRegistrationNumber(string crNumber)
    {
        if (!string.IsNullOrWhiteSpace(crNumber))
            CommercialRegistrationNumber = crNumber;
    }
}
