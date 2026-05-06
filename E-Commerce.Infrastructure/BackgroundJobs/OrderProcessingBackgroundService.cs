using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Infrastructure.BackgroundJobs;

public class OrderProcessingBackgroundService : BackgroundService
{
    private readonly ILogger<OrderProcessingBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const int CheckIntervalInMinutes = 60;

    public OrderProcessingBackgroundService(ILogger<OrderProcessingBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order Processing Job is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var pendingOrders = await orderRepo.GetPendingOrdersOver24HoursAsync(stoppingToken);

                    if (pendingOrders.Any())
                    {
                        _logger.LogInformation("Found {Count} pending orders over 24 hours. Processing cancellations.", pendingOrders.Count);
                        foreach (var order in pendingOrders)
                        {
                            order.Confirm();
                            _logger.LogInformation("Order with ID {OrderId} has been cancelled due to inactivity.", order.Id);
                        }
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation("No pending orders over 24 hours found.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing orders in background job.");
            }

            await Task.Delay(TimeSpan.FromMinutes(CheckIntervalInMinutes), stoppingToken);
        }
    }
}
