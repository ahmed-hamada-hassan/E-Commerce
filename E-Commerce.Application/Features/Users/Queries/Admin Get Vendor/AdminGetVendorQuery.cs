using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Vendor;

public record AdminGetVendorQuery(Guid VendorId) : IRequest<Result<AdminVendorResponse>>;
