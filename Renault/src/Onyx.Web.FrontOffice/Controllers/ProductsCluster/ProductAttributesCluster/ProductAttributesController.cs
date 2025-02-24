using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttribute.GetProductAttributeById;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttributes;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductAttributesController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductAttributeDto>>> GetAllProductAttributes()
    {
        return await Mediator.Send(new GetAllProductAttributesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductAttributeByIdDto?>> GetProductAttributeById(int id)
    {
        return await Mediator.Send(new GetProductAttributeByIdQuery(id));
    }
}
