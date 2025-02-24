using Onyx.Application.Common.Interfaces;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Services;

public class CurrentUserService : ICurrentUserService
{
    /*
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    */

    public string? UserId => null; //_httpContextAccessor.HttpContext?.User?.FindFirstValue("userid  }
}
