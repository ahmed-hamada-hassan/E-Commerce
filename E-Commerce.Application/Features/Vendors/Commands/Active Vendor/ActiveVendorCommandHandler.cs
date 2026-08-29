using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Commands.Active_Vendor;

internal sealed class ActiveVendorCommandHandler : IRequestHandler<ActiveVendorCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVendorRepository _vendorRepository;

    public ActiveVendorCommandHandler(IUnitOfWork unitOfWork, IVendorRepository vendorRepository)
    {
        _unitOfWork = unitOfWork;
        _vendorRepository = vendorRepository;
    }

    public async Task<Result<bool>> Handle(ActiveVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor is null)
            return Result<bool>.Failure(VendorErrors.NotFound);

        vendor.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
