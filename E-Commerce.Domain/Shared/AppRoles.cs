namespace E_Commerce.Domain.Shared;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";
    public const string Vendor = "Vendor";
    public const string SuperAdmin = "Super Admin";

    public static readonly HashSet<string> AllRoles = new HashSet<string>
    {
        Admin, Customer, Vendor, SuperAdmin
    };
}
