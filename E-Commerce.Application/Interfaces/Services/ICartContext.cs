using E_Commerce.Application.Interfaces.Dependency_Injection;

namespace E_Commerce.Application.Interfaces.Services;

public interface ICartContext : IScopedService
{
    Guid CartId();
}
