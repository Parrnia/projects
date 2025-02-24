using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.ProductImages.Commands.CreateProductImage;
using Onyx.Application.Main.ProductsCluster.ProductImages.Commands.DeleteRangeProductImages;
using Onyx.Application.Main.ProductsCluster.ProductImages.Commands.UpdateProductImage;
using Onyx.Application.Main.ProductsCluster.ProductImages.Queries.BackOffice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;

public class ProductImagesController : ApiControllerBase
{
 
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductImagesDto>>> GetAllProductImages()
    {
        return await Mediator.Send(new GetAllProductImagesQuery());
    }

    
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductImageCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateProductImageCommand command)
    {
        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpGet("product{productId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductImagesDto>>> GetProductImagesByProductId(int productId)
    {
        return await Mediator.Send(new GetProductImagesByProductIdQuery(productId));
    }

    
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductImagesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}

