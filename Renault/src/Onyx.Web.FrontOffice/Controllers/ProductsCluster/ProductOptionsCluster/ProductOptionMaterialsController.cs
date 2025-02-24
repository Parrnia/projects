using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.FrontOffice.GetProductOptionMaterial;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.FrontOffice.GetProductOptionMaterials;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.FrontOffice.GetOptionValueMaterials.GetAllOptionValueMaterials;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster.ProductOptionsCluster;


public class ProductOptionMaterialsController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductOptionMaterialDto>>> GetAllProductOptionMaterials()
    {
        return await Mediator.Send(new GetAllProductOptionMaterialsQuery());
    }
    [HttpGet("allValue")]
    public async Task<ActionResult<List<AllProductOptionValueMaterialDto>>> GetAllProductOptionValueMaterials()
    {
        return await Mediator.Send(new GetAllProductOptionValueMaterialsQuery());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductOptionMaterialByIdDto?>> GetProductOptionMaterialById(int id)
    {
        return await Mediator.Send(new GetProductOptionMaterialByIdQuery(id));
    }
}
