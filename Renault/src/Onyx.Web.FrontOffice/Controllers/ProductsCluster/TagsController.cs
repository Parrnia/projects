using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTag;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTags;


namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class TagsController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllTagDto>>> GetAllTags()
    {
        return await Mediator.Send(new GetAllTagsQuery());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TagByIdDto?>> GetTagById(int id)
    {
        return await Mediator.Send(new GetTagByIdQuery(id));
    }
}
