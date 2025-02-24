using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductType;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductTypes;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductTypesWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class ProductTypesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductTypeWithPaginationDto>>> GetProductTypesWithPagination([FromQuery] GetProductTypesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductTypeDto>>> GetAllProductTypes()
    {
        return await Mediator.Send(new GetAllProductTypesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductTypeByIdDto?>> GetProductTypeById(int id)
    {
        return await Mediator.Send(new GetProductTypeByIdQuery(id));
    }
}
