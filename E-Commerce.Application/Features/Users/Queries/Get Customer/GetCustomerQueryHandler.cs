using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Users.Queries.Get_Customer;

internal sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerProfileResponse>>
{
    private readonly IAppDbContext _dbContext;
    public GetCustomerQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CustomerProfileResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == request.UserId)
            .Select(u => new CustomerProfileResponse
            (
                UserId: u.Id,
                Name: u.FullName,
                UserName: u.UserName!,
                Email: u.Email!,
                PhoneNumber: u.PhoneNumber!,
                ImageUrl: u.ImageUrl,
                DateOfBirth: u.DateOfBirth,
                DefaultShippingAddressId: u.DefaultShippingAddressId,
                Addresses: u.Addresses.Select(a => new CustomerAddressInfo
                (
                    AddressId: a.Id,
                    AddressLine1: a.AddressLine1,
                    AddressLine2: a.AddressLine2,
                    City: a.City,
                    StateOrProvince: a.StateOrProvince,
                    Country: a.Country,
                    PostalCode: a.PostalCode,
                    AddressType: a.AddressType
                )).ToList()
            )).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<CustomerProfileResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<CustomerProfileResponse>.Success(user);
    }
}
