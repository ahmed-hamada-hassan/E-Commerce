namespace E_Commerce.Application.Common;

public sealed class RedisSettings
{
    public string ConnectionString { get; set; } = null!;
    public int ExpirationDays { get; set; }
}
