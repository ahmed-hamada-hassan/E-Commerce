using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.ClearCart;

internal sealed class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<bool>>
{
    private readonly IUserContext _userContext;
    private readonly ICartRepository _cartRepository;

    public ClearCartCommandHandler(IUserContext userContext, ICartRepository cartRepository)
    {
        _userContext = userContext;
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        await _cartRepository.DeleteAsync(userId, cancellationToken);

        return Result<bool>.Success(true);
    }
}
