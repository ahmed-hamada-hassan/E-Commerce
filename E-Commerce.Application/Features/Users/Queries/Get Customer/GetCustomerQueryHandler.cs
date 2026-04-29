using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Get_Customer;

internal sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerProfileResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetCustomerQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<CustomerProfileResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetActiveCustomerWithAddressesAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<CustomerProfileResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<CustomerProfileResponse>.Success(UsersMapper.ToCustomerProfileResponse(user));
    }
}
