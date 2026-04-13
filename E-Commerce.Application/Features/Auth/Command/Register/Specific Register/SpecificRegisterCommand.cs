using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Auth.Command.Register;

public record SpecificRegisterCommand(string Password, string FirstName, string? MiddleName, string LastName, string Email,
    string UserName, IFormFile? Image, string PhoneNumber, DateOnly DateOfBirth, string Role) 
    : IRequest<Result<AuthResponse>>;