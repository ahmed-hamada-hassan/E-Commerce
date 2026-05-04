using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Orders.DTOs;

public record CompleteRejectReturnRequest(ReturnStatus Status, string Reason);