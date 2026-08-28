using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class CartRepo : ICartRepository
{
    private readonly IDistributedCache _cache;
    private readonly RedisSettings _redisSettings;
    private const string CartCachePrefix = "cart:";
    public CartRepo(IDistributedCache cache, IOptions<RedisSettings> redisSettings)
    {
        _cache = cache;
        _redisSettings = redisSettings.Value;
    }

    public async Task<bool> DeleteAsync(Guid CartId, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{CartId}";
        await _cache.RemoveAsync(cacheKey, ct);
        return true;
    }

    public async Task<bool> DeleteBuyNowCartAsync(Guid CartId, CancellationToken ct)
    {
        var cacheKey = $"buy_now_cart:{CartId}";
        await _cache.RemoveAsync(cacheKey, ct);
        return true;
    }

    public async Task<Cart?> GetAsync(Guid CartId, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{CartId}";
        var stringData = await _cache.GetStringAsync(cacheKey, ct);
        return string.IsNullOrEmpty(stringData) ? default : JsonSerializer.Deserialize<Cart?>(stringData);
    }

    public async Task<bool> UpdateAsync(Cart cart, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{cart.Id}";
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(_redisSettings.ExpirationDays));

        var jsonData = JsonSerializer.Serialize(cart);
        await _cache.SetStringAsync(cacheKey, jsonData, options, ct);

        return true;
    }

    public async Task<bool> SetBuyNowCartAsync(Cart cart, CancellationToken ct)
    {
        var cacheKey = $"buy_now_cart:{cart.Id}";
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

        var jsonData = JsonSerializer.Serialize(cart);
        await _cache.SetStringAsync(cacheKey, jsonData, options, ct);
        return true;
    }

    public async Task<Cart?> GetBuyNowCartAsync(Guid CartId, CancellationToken ct)
    {
        var cacheKey = $"buy_now_cart:{CartId}";
        var stringData = await _cache.GetStringAsync(cacheKey, ct);
        return string.IsNullOrEmpty(stringData) ? default : JsonSerializer.Deserialize<Cart?>(stringData);
    }
}
