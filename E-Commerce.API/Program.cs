using E_Commerce.API.Middlewares;
using E_Commerce.Application;
using E_Commerce.Application.Behaviors;
using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.BackgroundJobs;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Data.Interceptors;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, service, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service)
    .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<SoftDeleteInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

builder.Services.AddOptions<PaginationSettings>()
    .Bind(builder.Configuration.GetSection("PaginationSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<CloudinarySettings>()
    .Bind(builder.Configuration.GetSection("CloudinarySettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
var connectionString = builder.Configuration.GetValue<string>("RedisSettings:ConnectionString");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connectionString;
    options.InstanceName = "E-Commerce_";
});

builder.Services.AddHttpContextAccessor();

// TODO: Configure persistence storge for production
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    //options.Password.RequiredUniqueChars = 3;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //options.SignIn.RequireConfirmedEmail = true;
    //options.SignIn.RequireConfirmedPhoneNumber = false;
    //options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole<Guid>>() // this line adds support for roles with a Guid as the key type
    .AddEntityFrameworkStores<AppDbContext>() // this line tells Identity Use Entity Framework Core to store users, roles, claims, tokens, etc.
    .AddSignInManager<SignInManager<ApplicationUser>>() // this line adds the SignInManager service for handling user sign-in operations
    .AddDefaultTokenProviders(); // this line adds default token providers for generating tokens for password reset, email confirmation, etc.

builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(IApplicationAssemblyMarker).Assembly,
        typeof(IInfrastructureAssemblyMarker).Assembly)

    .AddClasses(classes => classes.AssignableTo<ISingletonService>(), publicOnly: false)
        .AsImplementedInterfaces(type => type != typeof(ISingletonService))
        .AsSelf()
        .WithSingletonLifetime()

    .AddClasses(classes => classes.AssignableTo<IScopedService>(), publicOnly: false)
        .AsImplementedInterfaces(type => type != typeof(IScopedService))
        .AsSelf()
        .WithScopedLifetime()

    .AddClasses(classes => classes.AssignableTo<ITransientService>(), publicOnly: false)
        .AsImplementedInterfaces(type => type != typeof(ITransientService))
        .AsSelf()
        .WithTransientLifetime()
);

builder.Services.AddHostedService<ProductCleanupBackgroundService>();
builder.Services.AddHostedService<OrderProcessingBackgroundService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(IApplicationAssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidateBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(IApplicationAssemblyMarker).Assembly, includeInternalTypes: true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddOptions<JWTSettings>()
    .Bind(builder.Configuration.GetSection("JWT"))
    .Validate(options => options.SecretKey.Length >= 32, "SecretKey must be at least 32 characters long.")
    .ValidateOnStart();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        var issuer = builder.Configuration["JWT:Issuer"];
        var audience = builder.Configuration["JWT:Audience"];
        var secretKey = builder.Configuration["JWT:SecretKey"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin-SuperAdmin-Only", policy =>
        policy.RequireRole(AppRoles.Admin, AppRoles.SuperAdmin));

    options.AddPolicy("SuperAdmin-Vendor", policy =>
        policy.RequireRole(AppRoles.SuperAdmin, AppRoles.Vendor));

    options.AddPolicy("Customer-Only", policy =>
        policy.RequireRole(AppRoles.Customer));

    options.AddPolicy("Vendor-Only", policy =>
        policy.RequireRole(AppRoles.Vendor));

    options.AddPolicy("SuperAdmin-Only", policy =>
        policy.RequireRole(AppRoles.SuperAdmin));

    options.AddPolicy("Representative-Only", policy =>
        policy.RequireRole(AppRoles.Representative));

    options.AddPolicy("Representative-SuperAdmin-Only", policy =>
        policy.RequireRole(AppRoles.Representative, AppRoles.SuperAdmin));
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds)
                .ToString(CultureInfo.InvariantCulture);
        }

        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
            """{"error": "Too many requests. Please check the Retry-After header."}""",
            token
        );
    };

    options.AddPolicy("IpRateLimit", httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6
            }));

    options.AddPolicy("UserRateLimit", httpContext =>
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";

        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: userId,
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 40,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6
            });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseSerilogRequestLogging();
app.MapControllers();
await DbInitializer.SeedAsync(app.Services);
app.Run();
