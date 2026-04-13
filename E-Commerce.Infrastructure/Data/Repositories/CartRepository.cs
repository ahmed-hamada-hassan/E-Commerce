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

    public CartRepository(IDistributedCache cache, IOptions<RedisSettings> redisSettings)
    {
        _cache = cache;
        _redisSettings = redisSettings.Value;
    }

    public async Task<bool> DeleteAsync(Guid UserId, CancellationToken ct)
    {
        await _cache.RemoveAsync(UserId.ToString(), ct);
        return true;
    }

    public async Task<Cart?> GetAsync(Guid UserId, CancellationToken ct)
    {
        var stringData = await _cache.GetStringAsync(UserId.ToString(), ct);
        return string.IsNullOrEmpty(stringData) ? null : JsonSerializer.Deserialize<Cart?>(stringData);
    }

    public async Task<Cart?> UpdateAsync(Cart cart, CancellationToken ct)
    {
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(_redisSettings.CartExpirationDays));

        var jsonData = JsonSerializer.Serialize(cart);
        await _cache.SetStringAsync(cart.UserId.ToString(), jsonData, options, ct);

        return cart;
    }
}
