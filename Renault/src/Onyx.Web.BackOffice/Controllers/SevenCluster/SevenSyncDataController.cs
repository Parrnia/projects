using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.SevenCluster.SyncData.Count.Commands.UpdateCount;
using Onyx.Application.Main.SevenCluster.SyncData.CountAndPrice.Commands.UpdateCountAndPrice;
using Onyx.Application.Main.SevenCluster.SyncData.Price.Commands.UpdatePrice;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.SevenCluster;


public class SevenSyncDataController : ApiControllerBase
{
    [HttpPut("prices")]
    [CheckPermission(Roles.Employee, UserPermissions.Seven)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> UpdateSevenPrices(UpdateSevenPricesCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }


    [HttpPut("countsAndPrices")]
    [CheckPermission(Roles.Employee, UserPermissions.Seven)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> UpdateSevenCountsAndPrices(UpdateSevenCountsAndPricesCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }


    [HttpPut("counts")]
    [CheckPermission(Roles.Employee, UserPermissions.Seven)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> UpdateSevenCounts(UpdateSevenCountsCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
