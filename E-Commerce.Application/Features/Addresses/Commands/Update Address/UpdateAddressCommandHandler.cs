using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Addresses.Commands.Update_Address;

internal sealed class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateAddressCommandHandler> _logger;

    public UpdateAddressCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UpdateAddressCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Addresses.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found when attempting to update the address.", request.UserId);
            return Result<bool>.Failure(ApplicationUserErrors.NotFound);
        }

        var address = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId);

        if (address is null)
        {
            _logger.LogWarning("Address with ID {AddressId} not found for user with ID {UserId} when attempting to update address.",
                request.AddressId, request.UserId);
            return Result<bool>.Failure(AddressErrors.NotFound);
        }

        address.Update(
            request.AddressInfo.AddressLine1,
            request.AddressInfo.AddressLine2,
            request.AddressInfo.City,
            request.AddressInfo.StateOrProvince,
            request.AddressInfo.PostalCode,
            request.AddressInfo.Country,
            request.AddressInfo.AddressType
        );

        await _userManager.UpdateAsync(user);

        return Result<bool>.Success(true);
    }
}
