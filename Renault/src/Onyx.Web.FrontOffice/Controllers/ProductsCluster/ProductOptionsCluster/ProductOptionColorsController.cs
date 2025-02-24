using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColor;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColors;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.FrontOffice.GetOptionValueColors.GetAllOptionValueColors;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster.ProductOptionsCluster;


public class ProductOptionColorsController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductOptionColorDto>>> GetAllProductOptionColors()
    {
        return await Mediator.Send(new GetAllProductOptionColorsQuery());
    }
    [HttpGet("allValue")]
    public async Task<ActionResult<List<AllProductOptionValueColorDto>>> GetAllProductOptionValueColors()
    {
        return await Mediator.Send(new GetAllProductOptionValueColorsQuery());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductOptionColorByIdDto?>> GetProductOptionColorById(int id)
    {
        return await Mediator.Send(new GetProductOptionColorByIdQuery(id));
    }
}
