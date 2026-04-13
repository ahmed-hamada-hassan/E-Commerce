using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Auth.Command.Login;

public record LoginCommand(string Email,
    string Password
) : IRequest<Result<AuthResponse>>;