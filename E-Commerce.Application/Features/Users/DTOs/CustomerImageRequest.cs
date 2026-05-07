using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Users.DTOs;

public record CustomerImageRequest(IFormFile? Image);