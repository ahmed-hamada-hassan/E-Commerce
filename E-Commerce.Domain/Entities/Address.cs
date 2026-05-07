using E_Commerce.Domain.Common;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Address : SoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; } = null!;
    public string AddressLine1 { get; private set; } = null!;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = null!;
    public string? StateOrProvince { get; private set; }
    public string PostalCode { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public AddressType AddressType { get; private set; }

    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

   private Address(
       Guid id, Guid userId, string addressLine1, string? addressLine2, string city, string? stateOrProvince, 
       string postalCode, string country, AddressType addressType)
    {
        Id = id;
        UserId = userId;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        StateOrProvince = stateOrProvince;
        PostalCode = postalCode;
        Country = country;
        AddressType = addressType;
    }
    protected Address() { } // Parameterless constructor for EF Core

    public static Result<Address> Create(Guid userId, string addressLine1, string? addressLine2, string city, string? stateOrProvince, 
        string postalCode, string country, AddressType addressType)
    {
        if (userId == Guid.Empty)
            return Result<Address>.Failure(AddressErrors.EmptyUserId);
        if (string.IsNullOrWhiteSpace(addressLine1))
            return Result<Address>.Failure(AddressErrors.MissingAddressLine1);
        if (string.IsNullOrWhiteSpace(city))
            return Result<Address>.Failure(AddressErrors.MissingCity);
        if (string.IsNullOrWhiteSpace(postalCode))
            return Result<Address>.Failure(AddressErrors.MissingPostalCode);
        if (string.IsNullOrWhiteSpace(country))
            return Result<Address>.Failure(AddressErrors.MissingCountry);
        if(Enum.IsDefined(typeof(AddressType), addressType) == false)
            return Result<Address>.Failure(AddressErrors.InvalidAddressType);

        var address = new Address(
            Guid.NewGuid(), userId, addressLine1, addressLine2, city, stateOrProvince, 
            postalCode, country, addressType);

        return Result<Address>.Success(address);
    }

    public Result<bool> Update(string? addressLine1, string? addressLine2, string? city, string? stateOrProvince,
        string? postalCode, string? country, AddressType? addressType)
    {
        if(!string.IsNullOrWhiteSpace(addressLine1))
            AddressLine1 = addressLine1;
        if(!string.IsNullOrWhiteSpace(addressLine2))
            AddressLine2 = addressLine2;
        if(!string.IsNullOrWhiteSpace(city))
            City = city;
        if(!string.IsNullOrWhiteSpace(stateOrProvince))
            StateOrProvince = stateOrProvince;
        if(!string.IsNullOrWhiteSpace(postalCode))
            PostalCode = postalCode;
        if(!string.IsNullOrWhiteSpace(country))
            Country = country;
        if(addressType.HasValue && Enum.IsDefined(typeof(AddressType), addressType.Value))
            AddressType = addressType.Value;

        return Result<bool>.Success(true);
    }
}