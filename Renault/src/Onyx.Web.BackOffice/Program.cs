using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onyx.Application;
using Onyx.Application.Common.Middlewares;
using Onyx.Application.Services;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Settings;
using Onyx.Infrastructure;
using Onyx.Infrastructure.Persistence;
using Onyx.Web.BackOffice;
using Onyx.Web.BackOffice.Authorization;
using OnyxAuth.Shared;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

ConfigureAppSettings(builder);
ConfigureLogger(builder);

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = AppConstants.MaxRequestSize;
});
builder.WebHost.UseIIS();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
ConfigureAuthentication(builder);
ConfigurePayment(builder);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initializer.InitialiseAsync();
    //await initializer.SeedAsync();
}

app.UseCustomExceptionHandler();
app.UseHealthChecks("/health");
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();

static void ConfigureLogger(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, provider, logger) =>
    {
        var settings = provider.GetService<IOptions<LogSettings>>()?.Value;
        if (settings is null)
            return;

        logger = logger.Enrich.FromLogContext()
            .Enrich.WithProperty("service", settings.ServiceName)
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(settings.LogLevel));

        if (settings.Overrides != null)
        {
            logger = settings.Overrides
                .Aggregate(logger, (current, @override) =>
                    current.MinimumLevel.Override(@override.Key,
                        Enum.Parse<LogEventLevel>(settings.LogLevel)));
        }

        if (settings.Console is not null && settings.Console.Enabled)
        {
            logger.WriteTo.Console(
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(settings.LogLevel));
        }

        if (settings.Seq is not null && settings.Seq.Enabled)
        {
            logger.WriteTo.Seq(settings.Seq.ServerUrl,
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(settings.LogLevel),
                apiKey: settings.Seq.ApiKey);
        }

        if (settings.File is not null && settings.File.Enabled)
        {
            logger.WriteTo.File(settings.File.Path,
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(settings.LogLevel),
                rollingInterval: Enum.Parse<RollingInterval>(settings.File.RollingInterval),
                fileSizeLimitBytes: settings.File.FilesSizeLimit,
                retainedFileCountLimit: settings.File.FilesCountLimit,
                rollOnFileSizeLimit: true);
        }

    });
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var jwtTokenSettings = builder.Configuration
        .GetSection(nameof(JwtTokenSettings))
        .Get<JwtTokenSettings>()!;

    builder.Services.AddScoped<PermissionAuthorizationFilter>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.ClaimsIssuer = jwtTokenSettings.Issuer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtTokenSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtTokenSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenSettings.ValidationKey)),
            ValidateLifetime = true,
            NameClaimType = AuthConstants.Claims.UserName,
            RoleClaimType = AuthConstants.Claims.UserRole
        };
        options.SaveToken = true;
    });

    builder.Services.AddAuthorization();

    builder.Services.AddHttpClient(AppConstants.AuthenticationHttpClient, h =>
    {
        h.BaseAddress = new Uri(jwtTokenSettings.AuthenticationAddress);
    });
}

void ConfigureAppSettings(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddOptions<JwtTokenSettings>()
        .BindConfiguration(nameof(JwtTokenSettings));

    webApplicationBuilder.Services.AddOptions<ApplicationSettings>()
        .BindConfiguration(nameof(ApplicationSettings));

    webApplicationBuilder.Services.AddOptions<LogSettings>()
        .BindConfiguration(nameof(LogSettings));
}

void ConfigurePayment(WebApplicationBuilder builder)
{
    var paymentSettings = builder.Configuration
        .GetSection(nameof(AsanPardakhtSettings))
        .Get<AsanPardakhtSettings>()!;

    builder.Services.AddHttpClient(AppConstants.AsanPardakhtHttpClient, h =>
    {
        h.BaseAddress = new Uri(paymentSettings.ApiUrl);
    });

    builder.Services.AddScoped<AsanPardakhtPaymentService>();
    builder.Services.AddScoped<ParsianPaymentService>();
    builder.Services.AddScoped<PaymentServiceFactory>();

}