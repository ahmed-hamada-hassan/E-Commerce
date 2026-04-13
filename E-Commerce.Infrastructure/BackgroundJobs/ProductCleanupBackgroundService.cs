using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Infrastructure.BackgroundJobs;

public class ProductCleanupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProductCleanupBackgroundService> _logger;

    public ProductCleanupBackgroundService(IServiceProvider serviceProvider, ILogger<ProductCleanupBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Product Cleanup Background Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Product Cleanup Job started at: {StartTime}", DateTimeOffset.Now);

            try
            {
                await CleanupOldProductsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the product cleanup process at {Time}", DateTimeOffset.Now);
            }

            _logger.LogInformation("Product Cleanup Job finished. Waiting 24 hours for the next run...");

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation("Product Cleanup Background Service is stopping.");
    }

    private async Task CleanupOldProductsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var productImageRepository = scope.ServiceProvider.GetRequiredService<IProductImageRepository>();
        var imageService = scope.ServiceProvider.GetRequiredService<IFileService>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var cutoffDate = DateTime.UtcNow.AddDays(-30);
        var expiredProducts = (await productRepository.GetExpiredDeletedProductsAsync(cutoffDate, stoppingToken)).ToList();

        if (!expiredProducts.Any())
        {
            _logger.LogInformation("No expired products found to clean up at this time.");
            return;
        }

        _logger.LogInformation("Found {ExpiredCount} expired products to be permanently deleted.", expiredProducts.Count);

        foreach (var product in expiredProducts)
        {
            try
            {
                var images = await productImageRepository.GetAllByProductIdAsync(product.Id, stoppingToken);

                foreach (var image in images)
                {
                    if (!string.IsNullOrWhiteSpace(image.ImageUrl))
                    {
                        await imageService.DeleteImageAsync(image.ImageUrl);
                        _logger.LogDebug("Deleted image from Cloudinary: {ImageUrl}", image.ImageUrl);
                    }
                }

                await productImageRepository.RemoveByProductIdAsync(product.Id, stoppingToken);

                await productRepository.HardDeleteAsync(product, stoppingToken);

                _logger.LogInformation("Successfully hard deleted product: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to clean up product {ProductId}. It will be retried in the next run.", product.Id);
            }
        }

        await unitOfWork.SaveChangesAsync(stoppingToken);

        _logger.LogInformation("Completed cleanup for {Count} products and their associated images.", expiredProducts.Count);
    }
}
