using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, ISoftDeletable
{
    public string FirstName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = null!;
    public string? ImageUrl { get; private set; }

    //public Cart Cart { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }

    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    public Guid? DefaultShippingAddressId { get; private set; }
    public Address? DefaultShippingAddress { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; private set; }

    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeleteOn { get; set; }

    public string FullName => $"{FirstName} {(string.IsNullOrWhiteSpace(MiddleName) ? "" : MiddleName + " ")}{LastName}".Trim();

    // This is a Fat Agregate trap
    //private readonly List<Order> _orders = new();
    //private readonly List<Feedback> _feedbacks = new();
    //private readonly List<Cancellation> _cancellations = new();
    //public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
    //public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks.AsReadOnly();
    //public IReadOnlyCollection<Cancellation> Cancellations => _cancellations.AsReadOnly();

    private ApplicationUser(string firstName, string? middleName, string lastName, string email, string userName, string phoneNumber,
        string? imageUrl, DateOnly dateOfBirth) : base(userName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        ImageUrl = imageUrl;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
    }
    protected ApplicationUser() { } // Parameterless constructor for EF Core

    public static Result<ApplicationUser> Create(string firstName, string? middleName, string lastName, string email, string userName, string phoneNumber,
        string? imageUrl, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result<ApplicationUser>.Failure(ApplicationUserErrors.EmptyFirstName);
        if (string.IsNullOrWhiteSpace(lastName))
            return Result<ApplicationUser>.Failure(ApplicationUserErrors.EmptyLastName);
        if (string.IsNullOrWhiteSpace(email))
            return Result<ApplicationUser>.Failure(ApplicationUserErrors.EmptyEmail);
        if (string.IsNullOrWhiteSpace(userName))
            return Result<ApplicationUser>.Failure(ApplicationUserErrors.EmptyUsername);

        var user = new ApplicationUser(firstName, middleName, lastName, email, userName, phoneNumber, imageUrl, dateOfBirth);
        return Result<ApplicationUser>.Success(user);
    }

    public Result<bool> Update(string? firstName, string? middleName, string? lastName, string? email, string? userName, string? phoneNumber,
        DateOnly? dateOfBirth)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
        if (!string.IsNullOrWhiteSpace(userName))
            UserName = userName;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            PhoneNumber = phoneNumber;
        if (dateOfBirth.HasValue)
            DateOfBirth = dateOfBirth.Value;

        return Result<bool>.Success(true);
    }

    public Result<bool> UpdateImage(string? imageUrl)
    {
        ImageUrl = imageUrl;
        return Result<bool>.Success(true);
    }

    public void Restore()
    {
        IsDeleted = false;
        DeleteOn = null;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeleteOn = DateTimeOffset.UtcNow;
    }

    public void SetDefaultShippingAddress(Guid? addressId)
    {
        DefaultShippingAddressId = addressId;
    }

    public void UpdateRefreshToken(string refreshToken, DateTimeOffset expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public Result<bool> AddAddress(Address address)
    {
        if(Addresses.Count >= 5)
        {
            return Result<bool>.Failure(AddressErrors.MaxActiveAddressesReached);
        }
        _addresses.Add(address);
        return Result<bool>.Success(true);
    }
}
