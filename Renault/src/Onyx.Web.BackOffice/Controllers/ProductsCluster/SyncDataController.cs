using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.SyncData.Count.Commands.SyncCount;
using Onyx.Application.Main.ProductsCluster.SyncData.CountAndPrice.Commands.SyncCountAndPrice;
using Onyx.Application.Main.ProductsCluster.SyncData.Price.Commands.SyncPrice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class SyncDataController : ApiControllerBase
{
    [HttpPut("prices")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> SyncPrices()
    {
        var result = await Mediator.Send(new SyncPriceCommand());

        return result;
    }
    [HttpPut("countsAndPrices")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> SyncCountsAndPrices()
    {
        var result = await Mediator.Send(new SyncCountAndPriceCommand());

        return result;
    }
    [HttpPut("counts")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> SyncCounts()
    {
        var result = await Mediator.Send(new SyncCountCommand());

        return result;
    }
}
