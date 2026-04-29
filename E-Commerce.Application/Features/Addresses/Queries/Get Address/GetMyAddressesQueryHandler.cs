using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Addresses.Queries.Get_Address;

internal sealed class GetMyAddressesQueryHandler : IRequestHandler<GetMyAddressesQuery, Result<List<GetAddressInfo>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetMyAddressesQueryHandler> _logger;

    public GetMyAddressesQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetMyAddressesQueryHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<List<GetAddressInfo>>> Handle(GetMyAddressesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Addresses.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found when attempting to update the address.", request.UserId);
            return Result<List<GetAddressInfo>>.Failure(ApplicationUserErrors.NotFound);
        }

        return Result<List<GetAddressInfo>>.Success(user.Addresses.Select(a => new GetAddressInfo
        (
            AddressId : a.Id,
            AddressLine1 : a.AddressLine1,
            AddressLine2 : a.AddressLine2,
            City : a.City,
            StateOrProvince : a.StateOrProvince,
            PostalCode : a.PostalCode,
            Country : a.Country,
            AddressType : a.AddressType,
            IsDefault : user.DefaultShippingAddressId == a.Id
        )).ToList());
    }
}
