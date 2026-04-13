using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class ApplicationUserErrors
{
    public static readonly Error EmptyFirstName = new("ApplicationUser.EmptyFirstName", "The first name cannot be empty.");
    public static readonly Error EmptyLastName = new("ApplicationUser.EmptyLastName", "The last name cannot be empty.");
    public static readonly Error EmptyUsername = new("ApplicationUser.EmptyUsername", "The username cannot be empty.");
    public static readonly Error EmptyEmail = new("ApplicationUser.EmptyEmail", "The email cannot be empty.");
    public static readonly Error EmptyPhone = new("ApplicationUser.EmptyPhone", "The phone number cannot be empty.");
    public static readonly Error NotFound = new("ApplicationUser.NotFound", "The user was not found.");
    public static readonly Error HasActiveOrder = new("ApplicationUser.HasActiveOrder", "The user has an active order and cannot be deleted.");
    public static readonly Error InvalidCredentails = new("ApplicationUser.InvalidCredentials", "The provided credentials are invalid.");
    public static readonly Error InvalidToken = new("ApplicationUser.InvalidToken", "The provided token is invalid.");
    public static readonly Error InvalidRefreshToken = new("ApplicationUser.InvalidRefreshToken", "The provided refresh token is invalid.");
    public static readonly Error EmailAlreadyExists = new("ApplicationUser.EmailAlreadyExists", "A user with the same email already exists.");
    public static readonly Error UploadImageFaild = new("ApplicationUser.UploadImageFailed", "Failed to upload the profile image. Please try again.");
    public static readonly Error UserNameAlreadyExists = new("ApplicationUser.UserNameAlreadyExists", "A user with the same username already exists.");
    public static readonly Error VendorNotActive = new("ApplicationUser.VendorNotActive", "The associated vendor is not active. Please contact support.");
    public static readonly Error AccountLocked = new("ApplicationUser.AccountLocked", "Your account has been locked due to multiple failed login attempts. Please try again later or contact support.");
    public static readonly Error AccessDenied = new("ApplicationUser.AccessDenied", "You do not have permission to perform this action.");
}
