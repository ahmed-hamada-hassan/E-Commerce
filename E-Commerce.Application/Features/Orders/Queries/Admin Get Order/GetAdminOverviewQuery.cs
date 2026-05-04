using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;

public record GetAdminOverviewQuery(DateTime? FromDate = null, DateTime? ToDate = null) : IRequest<Result<AdminOverviewResponse>>;