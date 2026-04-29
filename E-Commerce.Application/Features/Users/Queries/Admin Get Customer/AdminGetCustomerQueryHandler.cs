using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Users.Queries;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

internal sealed class AdminGetCustomerQueryHandler : IRequestHandler<AdminGetCustomerQuery, Result<AdminCustomerResponse>>
{
    private readonly IUserRepository _userRepository;

    public AdminGetCustomerQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<AdminCustomerResponse>> Handle(AdminGetCustomerQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetCustomerForAdminByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<AdminCustomerResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<AdminCustomerResponse>.Success(UsersMapper.ToAdminCustomerResponse(user));
    }
}
