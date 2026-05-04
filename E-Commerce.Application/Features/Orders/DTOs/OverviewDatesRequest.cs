namespace E_Commerce.Application.Features.Orders.DTOs;

public record OverviewDatesRequest(DateTime? FromDate, DateTime? ToDate);