using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrand.GetProductBrandById;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetAllProductBrands;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetProductBrandsForBlock;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrandsWithPagination.GetProductBrandsWithPagination;


namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;


public class ProductBrandsController : ApiControllerBase
{
    [HttpGet("allProduct")]
    public async Task<ActionResult<List<AllProductBrandDto>>> GetAllProductBrands()
    {
        return await Mediator.Send(new GetAllProductBrandsQuery());
    }

    [HttpGet("allProductWithPagination")]
    public async Task<ActionResult<PaginatedList<ProductBrandWithPaginationDto>>> GetProductBrandsWithPagination([FromQuery] GetProductBrandsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("allForBlock{limit}")]
    public async Task<ActionResult<PaginatedList<ProductBrandForBlockDto>>> GetProductBrandsForBlock(int limit)
    {
        return await Mediator.Send(new GetProductBrandsForBlockQuery(limit));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductBrandByIdDto?>> GetProductBrandById(int id)
    {
        return await Mediator.Send(new GetProductBrandByIdQuery(id));
    }

}
