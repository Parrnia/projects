using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditByCustomerId;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCredits;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;

namespace Onyx.Web.FrontOffice.Controllers.UserProfilesCluster;

public class CreditsController : ApiControllerBase
{
    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<PaginatedList<CreditDto>>> GetCreditsByCustomerIdWithPagination([FromQuery] GetCreditsByCustomerIdWithPaginationQuery query)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }

    [HttpGet("lastCredit")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<decimal?>> GetCreditByCustomerId([FromQuery] GetCreditByCustomerIdQuery query)
    {
        if (UserInfo == null || query.CustomerId != UserInfo.UserId)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }
}
