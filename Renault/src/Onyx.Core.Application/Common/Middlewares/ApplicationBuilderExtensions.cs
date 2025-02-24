using Microsoft.AspNetCore.Builder;

namespace Onyx.Application.Common.Middlewares;
public static class ApplicationBuilderExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
