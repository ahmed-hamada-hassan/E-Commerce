using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Commands.Update_Vendor;

public record UpdateVendorCommand(Guid VendorId, string? StoreName, string? CommercialRegistrationNumber) : IRequest<Result<bool>>;