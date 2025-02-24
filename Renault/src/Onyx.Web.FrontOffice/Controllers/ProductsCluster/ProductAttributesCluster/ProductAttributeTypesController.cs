using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice.GetProductAttributeType;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice.GetProductAttributeTypes;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductAttributeTypesController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductAttributeTypeDto>>> GetAllProductAttributeTypes()
    {
        return await Mediator.Send(new GetAllProductAttributeTypesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductAttributeTypeByIdDto?>> GetProductAttributeTypeById(int id)
    {
        return await Mediator.Send(new GetProductAttributeTypeByIdQuery(id));
    }
}
