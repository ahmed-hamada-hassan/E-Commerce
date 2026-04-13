namespace E_Commerce.Application.Common;

public sealed class RedisSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public int CartExpirationDays { get; set; }

    public string ConnectionString => $"{Host}:{Port}";
}
