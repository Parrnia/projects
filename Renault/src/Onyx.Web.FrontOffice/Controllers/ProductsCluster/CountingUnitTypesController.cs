using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitType;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitTypes;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitTypesWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class CountingUnitTypesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CountingUnitTypeWithPaginationDto>>> GetCountingUnitTypesWithPagination([FromQuery] GetCountingUnitTypesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllCountingUnitTypeDto>>> GetAllCountingUnitTypes()
    {
        return await Mediator.Send(new GetAllCountingUnitTypesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountingUnitTypeByIdDto?>> GetCountingUnitTypeById(int id)
    {
        return await Mediator.Send(new GetCountingUnitTypeByIdQuery(id));
    }
}
