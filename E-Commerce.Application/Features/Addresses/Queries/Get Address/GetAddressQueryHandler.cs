using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Addresses.Queries.Get_Address;

internal class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, Result<GetAddressInfo>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetAddressQueryHandler> _logger;

    public GetAddressQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetAddressQueryHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<GetAddressInfo>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Addresses.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found when attempting to update the address.", request.UserId);
            return Result<GetAddressInfo>.Failure(ApplicationUserErrors.NotFound);
        }

        var address = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId);

        if(address is null)
        {
            _logger.LogWarning("Address with ID {AddressId} not found for user with ID {UserId}.", request.AddressId, request.UserId);
            return Result<GetAddressInfo>.Failure(AddressErrors.NotFound);
        }

        return Result<GetAddressInfo>.Success(new GetAddressInfo
        (
            AddressId : address.Id,
            AddressLine1 : address.AddressLine1,
            AddressLine2 : address.AddressLine2,
            City : address.City,
            StateOrProvince : address.StateOrProvince,
            PostalCode : address.PostalCode,
            Country : address.Country,
            AddressType : address.AddressType,
            IsDefault : user.DefaultShippingAddressId == address.Id
        ));
    }
}
