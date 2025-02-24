using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnit;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnits;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnitsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class CountingUnitsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CountingUnitWithPaginationDto>>> GetCountingUnitsWithPagination([FromQuery] GetCountingUnitsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllCountingUnitDto>>> GetAllCountingUnits()
    {
        return await Mediator.Send(new GetAllCountingUnitsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountingUnitByIdDto?>> GetCountingUnitById(int id)
    {
        return await Mediator.Send(new GetCountingUnitByIdQuery(id));
    }
}
