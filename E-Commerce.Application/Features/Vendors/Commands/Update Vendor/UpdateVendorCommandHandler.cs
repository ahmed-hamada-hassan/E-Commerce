using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Vendors.Commands.Update_Vendor;

internal sealed class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, Result<bool>>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfwork;
    private readonly ILogger<UpdateVendorCommandHandler> _logger;

    public UpdateVendorCommandHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfwork, ILogger<UpdateVendorCommandHandler> logger)
    {
        _vendorRepository = vendorRepository;
        _unitOfwork = unitOfwork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if(vendor is null) return Result<bool>.Failure(VendorErrors.NotFound);

        if(!string.IsNullOrWhiteSpace(request.StoreName))
            vendor.UpdateStoreName(request.StoreName);
        if(!string.IsNullOrWhiteSpace(request.CommercialRegistrationNumber) && 
            vendor.CommercialRegistrationNumber != request.CommercialRegistrationNumber)
        {
            var isCrnUnique = await _vendorRepository.IsCommercialRegistrationNumberUniquenessAsync(request.CommercialRegistrationNumber, 
                cancellationToken);
            if(!isCrnUnique) return Result<bool>.Failure(VendorErrors.DuplicateCommercialRegistrationNumber);
            vendor.UpdateCommercialRegistrationNumber(request.CommercialRegistrationNumber);

            vendor.Deactivate();
        }

        await _unitOfwork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
