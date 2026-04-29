using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Queries.Get_Vendor;

public record GetVendorQuery(Guid VendorId) : IRequest<Result<VendorProfileResponse>>;