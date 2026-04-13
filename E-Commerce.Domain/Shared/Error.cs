namespace E_Commerce.Domain.Shared;

public record Error(string Code, string Description) // Primary constructor for record type
{
    public static readonly Error None = new Error(string.Empty, string.Empty); // Null object pattern for no error
}
