using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Extensions;

namespace Onyx.Web.BackOffice.Controllers;

[ApiController]
//[PermissionAuthorize]
[AllowAnonymous]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private UserInfo? _userInfo;
    private ISender? _mediator;

    protected UserInfo? UserInfo => _userInfo ??= User?.Claims?.ToList().ToUserInfo();
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
