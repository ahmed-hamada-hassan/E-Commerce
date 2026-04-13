using E_Commerce.Application.Interfaces.Dependency_Injection;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Interfaces.Services;

public interface IFileService : IScopedService
{
    Task<string?> UploadImageAsync(IFormFile file);
    Task<bool> DeleteImageAsync(string imageUrl);
}