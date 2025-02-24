using Hangfire;
using Microsoft.Extensions.Options;
using Onyx.Infrastructure.BackgroundJobs;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Middlewares;
using Onyx.Application.Services;
using Onyx.Infrastructure.BackgroundJobs.Features.Services;
using Onyx.Infrastructure.Persistence;
using Onyx.Infrastructure.Persistence.Interceptors;
using Onyx.Infrastructure.Services;
using Onyx.Application.Settings;
using Onyx.Infrastructure.BackgroundJobs.Jobs;
using Serilog;
using Serilog.Events;
using Onyx.Infrastructure;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApplicationSettings>()
    .BindConfiguration(nameof(ApplicationSettings));

builder.Services.AddOptions<HangfireSettings>()
    .BindConfiguration(nameof(HangfireSettings));

builder.Services.AddOptions<LogSettings>()
    .BindConfiguration(nameof(LogSettings));

ConfigureLogger(builder);
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDateTime, DateTimeService>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

ConfigurePayment(builder);

var hangfireSettings = builder.Configuration.GetSection(nameof(HangfireSettings))
    .Get<HangfireSettings>()!;

//Hangfire
builder.Services.AddBackgroundJobs(builder.Configuration.GetConnectionString("DefaultConnection")!,
    hangfireSettings.ServerName, hangfireSettings.WorkerCount, builder.Configuration);

builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initializer.InitialiseAsync();
}

app.UseCustomExceptionHandler();
app.UseHangfireDashboard(hangfireSettings.DashboardUrl);

app.UseAuthorization();
app.MapControllers();
JobManager.Initialize(hangfireSettings);

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