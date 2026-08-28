using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class AddressErrors
{
    public static readonly Error EmptyUserId = new ("Address.EmptyUserId", "The User ID cannot be empty.");
    public static readonly Error MissingAddressLine1 = new ("Address.MissingAddressLine1", "The Address Line 1 is required.");
    public static readonly Error MissingCity = new ("Address.MissingCity", "The City is required.");
    public static readonly Error MissingPostalCode = new ("Address.MissingPostalCode", "The Postal Code is required.");
    public static readonly Error MissingCountry = new ("Address.MissingCountry", "The Country is required.");
    public static readonly Error InvalidAddressType = new("Address.InvalidAddressType", "The address type must be either 'Shipping' or 'Billing'.");
    public static readonly Error NotFound = new("Address.NotFound", "The specified address was not found.");
    public static readonly Error MaxActiveAddressesReached = new("Address.MaxActiveAddressesReached", "The user has reached the maximum limit of 5 active addresses.");
}
