using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, IOrderRepository orderRepository, ILogger<DeleteUserCommandHandler> logger)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        var hasActiveOrder = await _orderRepository.HasActiveOrdersForUserAsync(user.Id, cancellationToken);
        if (hasActiveOrder)
        {
            _logger.LogWarning("Attempt to delete user with active orders. UserId: {UserId}", user.Id);
            return Result<bool>.Failure(ApplicationUserErrors.HasActiveOrder);
        }

        user.Delete();

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var error = updateResult.Errors.First();
            _logger.LogError("Failed to delete user. UserId: {UserId}, Error: {ErrorCode} - {ErrorDescription}", 
                user.Id, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        return Result<bool>.Success(true);
    }
}