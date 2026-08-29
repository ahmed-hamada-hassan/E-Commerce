using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace E_Commerce.API.Extentsions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddAppRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("Login", c =>
            {
                var ip = c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 6,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 6,
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("Signup", c =>
            {
                var ip = c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromHours(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("AdminSignup", c =>
            {
                var userId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownAdmin";
                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromHours(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("RefreshToken", c =>
            {
                var ip = c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("UserActions", c =>
            {
                var partitionKey = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 15,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("AdminManagement", c =>
            {
                var adminId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "UnknownAdmin";
                return RateLimitPartition.GetFixedWindowLimiter(adminId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("GuestCartActions", c =>
            {
                var cartId = c.Request.Headers["X-Cart-Id"].ToString();
                var partitionKey = !string.IsNullOrEmpty(cartId) ? cartId : (c.Connection.RemoteIpAddress?.ToString() ?? "AnonymousCart");
                return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 20,
                    TokensPerPeriod = 5,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                    QueueLimit = 0
                });
            });

            options.AddPolicy("PublicBrowsing", c =>
            {
                var ip = c.Connection.RemoteIpAddress?.ToString() ?? "UnknownPublic";
                return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 120,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("SearchProducts", c =>
            {
                var ip = c.Connection.RemoteIpAddress?.ToString() ?? "UnknownPublic";
                return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 30,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 3,
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("ProfileManagement", c =>
            {
                var userId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("OrderOperations", c =>
            {
                var userId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("FeedbackOperations", c =>
            {
                var userId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("RepresentativeOperations", c =>
            {
                var representativeId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "UnknownRepresentative";
                return RateLimitPartition.GetFixedWindowLimiter(representativeId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 30,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.AddPolicy("VendorManagement", c =>
            {
                var vendorId = c.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? c.Connection.RemoteIpAddress?.ToString() ?? "UnknownVendor";
                return RateLimitPartition.GetFixedWindowLimiter(vendorId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 40,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                });
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int)Math.Ceiling(retryAfter.TotalSeconds))
                        .ToString(CultureInfo.InvariantCulture);
                }

                context.HttpContext.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Title = "Too Many Requests",
                    Status = StatusCodes.Status429TooManyRequests,
                    Detail = "You have exceeded the allowed number of requests."
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, token);
            };
        });

        return services;
    }
}