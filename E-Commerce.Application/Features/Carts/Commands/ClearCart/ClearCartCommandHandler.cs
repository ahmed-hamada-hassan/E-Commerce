using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.ClearCart;

internal sealed class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;

    public ClearCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        await _cartRepository.DeleteAsync(request.CartId, cancellationToken);

        return Result<bool>.Success(true);
    }
}
