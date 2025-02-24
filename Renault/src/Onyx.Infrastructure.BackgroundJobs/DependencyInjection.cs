using Hangfire;
using Hangfire.SqlServer;
using Onyx.Application.Services;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManageFiles;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManagePayments;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SendRequest;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData;

namespace Onyx.Infrastructure.BackgroundJobs;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services,
      string connectionString,
      string serverName,
      int workerCount, IConfiguration configuration)
    {
        services.AddScoped<SharedService>();
        services.AddScoped<SevenSoftService>();
        services.AddScoped<SmsService>();
        services.AddScoped<DataMigrationHandler>();
        services.AddScoped<SendRequestHandler>();
        services.AddScoped<ManageFilesHandler>();
        services.AddScoped<ManagePaymentsHandler>();
        services.AddScoped<SyncDataJob>();
        services.AddScoped<SendRequestJob>();
        services.AddScoped<ManageFilesJob>();
        services.AddHttpClient();

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 14, DelaysInSeconds = new int[] { 3600 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail });
        
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));



        services.AddHangfireServer(x =>
        {
            x.ServerName = serverName;
            x.WorkerCount = workerCount;
        });


        return services;
    }

    //public static IServiceCollection AddApplication(this IServiceCollection services)
    //{
    //    //services.AddAutoMapper(Assembly.GetExecutingAssembly());
    //    services.AddMediatR(i => i.RegisterServicesFromAssemblyContaining<Kind>());


    //    return services;
    //}

    //public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    //{
    //    if (configuration.GetValue<bool>("UseInMemoryOnyxDB"))
    //    {
    //        services.AddDbContext<ApplicationDbContext>(options =>
    //            options.UseInMemoryDatabase("OnyxDB"));
    //    }
    //    else
    //    {
    //        services.AddDbContext<ApplicationDbContext>(options =>
    //            options.UseSqlServer(
    //                configuration.GetConnectionString("DefaultConnection"),
    //                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    //    }



    //    return services;
    //}
}
