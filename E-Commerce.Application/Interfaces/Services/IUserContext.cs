using E_Commerce.Application.Interfaces.Dependency_Injection;

namespace E_Commerce.Application.Interfaces.Services;

public interface IUserContext : IScopedService
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
    string? Email { get; }
    Guid? VendorId { get; }
    bool IsInRole(string role);
}
