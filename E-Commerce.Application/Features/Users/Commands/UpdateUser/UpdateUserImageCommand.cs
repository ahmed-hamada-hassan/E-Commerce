using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserImageCommand(Guid UserId, IFormFile? Image) : IRequest<Result<bool>>;