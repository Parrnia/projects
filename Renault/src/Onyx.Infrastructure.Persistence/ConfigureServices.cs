using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;
using Onyx.Application.Services;
using Onyx.Application.Services.ExportServices;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Infrastructure.Persistence;
using Onyx.Infrastructure.Persistence.Interceptors;
using Onyx.Infrastructure.Services;

namespace Onyx.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("OnyxDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IFileStore, FileStore>();
        services.AddScoped<ISharedProductQueryHelperMethods, SharedProductQueryHelperMethods>();
        services.AddScoped<IExportServices, ExportServices>();
        services.AddScoped<ISevenSoftService, SevenSoftService>();
        services.AddScoped<ICreateOrderHelper, CreateOrderHelper>();
        services.AddScoped<ICreateReturnOrderHelper, ReturnOrderHelper>();
        services.AddScoped<ISmsService,SmsService>();

        return services;
    }
}
