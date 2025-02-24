using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.FrontOffice.GetProductStatus;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.FrontOffice.GetProductStatuses;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.FrontOffice.GetProductStatusesWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class ProductStatusesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductStatusWithPaginationDto>>> GetProductStatusesWithPagination([FromQuery] GetProductStatusesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductStatusDto>>> GetAllProductStatuses()
    {
        return await Mediator.Send(new GetAllProductStatusesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductStatusByIdDto?>> GetProductStatusById(int id)
    {
        return await Mediator.Send(new GetProductStatusByIdQuery(id));
    }
}
