using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Queries.Admin_Get_Vendor;

public record AdminGetVendorQuery(Guid VendorId) : IRequest<Result<AdminVendorResponse>>;
