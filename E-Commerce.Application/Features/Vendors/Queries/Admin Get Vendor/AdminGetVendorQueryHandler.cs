using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Features.Vendors.Queries;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Queries.Admin_Get_Vendor;

internal sealed class AdminGetVendorQueryHandler : IRequestHandler<AdminGetVendorQuery, Result<AdminVendorResponse>>
{
    private readonly IVendorRepository _vendorRepository;

    public AdminGetVendorQueryHandler(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<Result<AdminVendorResponse>> Handle(AdminGetVendorQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetVendorForAdminByIdAsync(request.VendorId, cancellationToken);
        if (vendor is null)
            return Result<AdminVendorResponse>.Failure(ApplicationUserErrors.NotFound);

        return Result<AdminVendorResponse>.Success(vendor.ToAdminVendorResponse());
    }
}
