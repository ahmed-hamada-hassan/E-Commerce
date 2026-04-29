using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Queries.Get_Vendor;

internal class GetVendorQueryHandler : IRequestHandler<GetVendorQuery, Result<VendorProfileResponse>>
{
    private readonly IVendorRepository _vendorRepository;

    public GetVendorQueryHandler(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<Result<VendorProfileResponse>> Handle(GetVendorQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if(vendor is null)
            return Result<VendorProfileResponse>.Failure(VendorErrors.NotFound);
        return Result<VendorProfileResponse>.Success(vendor.ToVendorProfileResponse());
    }
}
