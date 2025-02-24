using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;
using Onyx.Domain.Enums;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;

namespace Onyx.Web.FrontOffice.Controllers.UserProfilesCluster;


public class CustomerTypesController : ApiControllerBase
{
    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<CustomerTypeDto?>> GetCustomerTypeByCustomerTypeEnum()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetCustomerTypeByCustomerTypeEnumQuery((CustomerTypeEnum)UserInfo.CustomerType));
    }
}
