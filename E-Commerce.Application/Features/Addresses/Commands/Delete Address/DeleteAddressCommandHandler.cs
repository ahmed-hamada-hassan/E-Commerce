using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Addresses.Commands.Delete_Address;

internal sealed class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAddressCommandHandler> _logger;

    public DeleteAddressCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ILogger<DeleteAddressCommandHandler> logger)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Addresses.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found when attempting to delete address.", request.UserId);
            return Result<bool>.Failure(ApplicationUserErrors.NotFound);
        }

        var address = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId);

        if (address is null)
        {
            _logger.LogWarning("Address with ID {AddressId} not found for user with ID {UserId} when attempting to update address.",
                request.AddressId, request.UserId);
            return Result<bool>.Failure(AddressErrors.NotFound);
        }

        if(user.DefaultShippingAddressId == address.Id)
            user.SetDefaultShippingAddress(null);

        address.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
