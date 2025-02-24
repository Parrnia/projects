
using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModel.GetModelById;
using Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetAllModels;
using Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetModelsByFamilyId;
using Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModelsWithPagination.GetModelsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;


public class ModelsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ModelWithPaginationDto>>> GetModelsWithPagination([FromQuery] GetModelsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllModelDto>>> GetAllModels()
    {
        return await Mediator.Send(new GetAllModelsQuery());
    }

    [HttpGet("family{familyId}")]
    public async Task<ActionResult<List<ModelByFamilyIdDto>>> GetModelsByFamilyId(int familyId)
    {
        return await Mediator.Send(new GetModelsByFamilyIdQuery(familyId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ModelByIdDto?>> GetModelById(int id)
    {
        return await Mediator.Send(new GetModelByIdQuery(id));
    }
}
