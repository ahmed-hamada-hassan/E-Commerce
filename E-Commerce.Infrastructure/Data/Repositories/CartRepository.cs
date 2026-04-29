using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class CartRepository : ICartRepository
{
    private readonly IDistributedCache _cache;
    private readonly RedisSettings _redisSettings;
    private const string CartCachePrefix = "cart:";
    public CartRepository(IDistributedCache cache, IOptions<RedisSettings> redisSettings)
    {
        _cache = cache;
        _redisSettings = redisSettings.Value;
    }

    public async Task<bool> DeleteAsync(Guid UserId, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{UserId}";
        await _cache.RemoveAsync(cacheKey, ct);
        return true;
    }

    public async Task<Cart?> GetAsync(Guid UserId, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{UserId}";
        var stringData = await _cache.GetStringAsync(cacheKey, ct);
        return string.IsNullOrEmpty(stringData) ? default : JsonSerializer.Deserialize<Cart?>(stringData);
    }

    public async Task<Cart?> UpdateAsync(Cart cart, Guid UserId, CancellationToken ct)
    {
        var cacheKey = $"{CartCachePrefix}{UserId}";
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(_redisSettings.ExpirationDays));

        var jsonData = JsonSerializer.Serialize(cart);
        await _cache.SetStringAsync(cacheKey, jsonData, options, ct);

        return cart;
    }
}
