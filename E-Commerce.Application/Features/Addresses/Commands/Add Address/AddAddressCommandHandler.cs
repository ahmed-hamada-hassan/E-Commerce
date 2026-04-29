using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Addresses.Commands;

internal sealed class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Result<List<Guid>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddAddressCommandHandler> _logger;

    public AddAddressCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ILogger<AddAddressCommandHandler> logger)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<Guid>>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Addresses.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found when attempting to add address.", request.UserId);
            return Result<List<Guid>>.Failure(ApplicationUserErrors.NotFound);
        }

        var currentAddresses = user.Addresses.Count;
        var newAddressesCount = request.Addresses.Count;
        var addressesIDs = new List<Guid>();
        if((currentAddresses < 5 && currentAddresses >= 0) && (newAddressesCount <= 5 && newAddressesCount >= 1))
        {
            if (currentAddresses + newAddressesCount <= 5)
            {
                foreach (var addressInfo in request.Addresses)
                {
                    var addressResult = Address.Create(
                        request.UserId,
                        addressInfo.AddressLine1,
                        addressInfo.AddressLine2,
                        addressInfo.City,
                        addressInfo.StateOrProvince,
                        addressInfo.PostalCode,
                        addressInfo.Country,
                        addressInfo.AddressType
                        );
                    if (addressResult.IsFailure)
                    {
                        _logger.LogWarning("Failed to create address for user with ID {UserId}. Error: {Error}", request.UserId, addressResult.Error);
                        return Result<List<Guid>>.Failure(addressResult.Error);
                    }
                    var newAddress = addressResult.Value!;
                    user.AddAddress(newAddress);

                    if (user.DefaultShippingAddressId is null)
                        user.SetDefaultShippingAddress(newAddress.Id);

                    addressesIDs.Add(newAddress.Id);
                }
            }
            else
            {
                return Result<List<Guid>>.Failure(new Error("Address.LimitExceeded", "Maximum 5 addresses allowed."));
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<List<Guid>>.Success(addressesIDs);
    }
}