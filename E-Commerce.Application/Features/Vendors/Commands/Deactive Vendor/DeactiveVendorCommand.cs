using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Commands.Deactive_Vendor;

public record DeactiveVendorCommand(Guid VendorId) : IRequest<Result<bool>>;