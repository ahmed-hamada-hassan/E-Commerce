using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.Active_Vendor;

public record ActiveVendorCommand(Guid VendorId):IRequest<Result<bool>>;