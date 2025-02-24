using Microsoft.AspNetCore.Mvc.Filters;

namespace Onyx.Web.FrontOffice.Authorization;

public class PermissionAuthorizeAttribute : Attribute, IFilterFactory
{
    public bool IsReusable { get; } = false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var filter = serviceProvider.GetRequiredService<PermissionAuthorizationFilter>();
        return filter;
    }


}