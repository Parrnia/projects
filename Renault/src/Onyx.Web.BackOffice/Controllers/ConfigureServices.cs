using System.Reflection;
using FluentValidation;
using MediatR;
using Onyx.Application.Common.Behaviours;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPostsWithPagination;

namespace Onyx.Web.BackOffice.Controllers;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(i => i.RegisterServicesFromAssemblyContaining<GetPostsWithPaginationQuery>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        return services;
    }
}
