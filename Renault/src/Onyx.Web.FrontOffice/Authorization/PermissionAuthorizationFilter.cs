using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Onyx.Application;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Extensions;

namespace Onyx.Web.FrontOffice.Authorization;

public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly HttpClient _client;

    public PermissionAuthorizationFilter(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient(AppConstants.AuthenticationHttpClient);
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            // در صورتی که منبع درخواست شده نیاز به تایید هویت نداشته باشد
            if (ResourceIsAnonymous(context.ActionDescriptor))
                return;

            // در صورتی که مشخصات کاربر قابل استخراج نباشد، کاربر لاگین نباشد یا توکن وی منقضی شده باشد
            var userInfo = context.HttpContext.User?.Claims?.ToList().ToUserInfo();
            if (userInfo?.Roles == null || !userInfo.Roles.Any())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // در صورتی که برای منبع درخواستی حداقل یک پرمیژن ثبت شده باشد و کاربر ادمین نباشد
            var permissions = GetAppliedPermissions(context.ActionDescriptor);
            if (permissions.Any() && !UserIsSuperAdmin(userInfo))
            {
                // در صورتی که کاربر حداقل یکی از پرمیژن های منبع درخواست شده را داشته باشد
                foreach (var permission in permissions)
                {
                    if ((permission.Role == Roles.Any || UserHasRole(permission.Role, userInfo)) &&
                        (await UserHasPermission(permission.PermissionSlug, userInfo, context.HttpContext)))
                        return;
                }

                // در صورتی که کاربر پرمیژن مد نظر را نداشته باشد.
                context.Result = new ForbidResult();
            }
            else
            {
                // do more checks if needed
                return;
            }
        }
        catch
        {
            context.Result = new UnauthorizedResult();
        }
    }


    private async Task<bool> UserHasPermission(string permissionSlug, UserInfo userInfo, HttpContext context)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"users/self/hasPermission?permissionSlug={permissionSlug}");
        request.Headers.TryAddWithoutValidation("Authorization", context.Request.Headers["Authorization"].ToString());
        request.Headers.TryAddWithoutValidation("X-Forwarded-For", GetRoutedClientIp(context) ?? string.Empty);
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;

        var result = JsonSerializer.Deserialize<ResponseMessage<bool>>(await response.Content.ReadAsStringAsync(), JsonOptions);
        return result?.Content ?? false;
    }

    private static bool UserHasRole(string role, UserInfo userInfo)
    {
        return userInfo.Roles.Contains(role);
    }

    private static bool ResourceIsAnonymous(ActionDescriptor descriptor)
    {
        if (!(descriptor is ControllerActionDescriptor actionDescriptor))
            return false; // worse case
        var attributes = actionDescriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>();
        if (!attributes.Any())
        {
            return actionDescriptor.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>().Any() &&
                   !actionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>().Any();
        }

        return true;
    }


    private static bool UserIsSuperAdmin(UserInfo userInfo)
    {
        if (userInfo?.Roles == null || !userInfo.Roles.Any())
            return false;
        return userInfo.Roles.Contains(Roles.SysAdmin);
    }


    private List<(string Role, string PermissionSlug)> GetAppliedPermissions(ActionDescriptor descriptor)
    {
        if (!(descriptor is ControllerActionDescriptor actionDescriptor))
            return new List<(string Role, string PermissionSlug)>();

        var permissions = actionDescriptor.MethodInfo.GetCustomAttributes<CheckPermissionAttribute>()
            .Concat(actionDescriptor.ControllerTypeInfo.GetCustomAttributes<CheckPermissionAttribute>()).ToList();

        var results = new List<(string Role, string PermissionSlug)>();
        foreach (var permission in permissions)
        {
            if (permission.Permission is not Enum)
                continue;

            var permissionDefinition = permission.Permission.GetType()
                .GetCustomAttributes<PermissionsDefinitionAttribute>().FirstOrDefault();

            results.Add((permission.Role, $"{permissionDefinition?.Name}-{permission.Permission}"));
        }
        return results;
    }


    private string? GetRoutedClientIp(HttpContext context)
    {
        // a.ammari:
        // در صورتی که سرویس پشت یک ریورس پروکسی باشد سعی می کنیم اطلاعات آی پی مبدا را استخراج کنیم
        // ریورس پروکسی باید اطلاعات آی پی مبدا را توسط یکی از هدر های استاندارد ارسال کند.

        var forwardedFor = context.Request.Headers["X-Forwarded-For"];
        var forwarded = context.Request.Headers["Forwarded"];

        if (forwardedFor != StringValues.Empty && !string.IsNullOrEmpty(forwardedFor))
            return forwardedFor.ToString();

        if (forwarded != StringValues.Empty && !string.IsNullOrEmpty(forwarded))
        {
            return forwarded.ToString()
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(i => i.Trim()
                    .StartsWith("for=", StringComparison.OrdinalIgnoreCase))?
                .Split('=', StringSplitOptions.RemoveEmptyEntries)
                .Last().Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

}