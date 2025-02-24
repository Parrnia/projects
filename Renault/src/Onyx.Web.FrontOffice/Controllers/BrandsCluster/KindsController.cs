using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKind.GetKindById;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetAllKinds;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetKindsByModelId;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKindsWithPagination.GetKindsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;


public class KindsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<KindWithPaginationDto>>> GetKindsWithPagination([FromQuery] GetKindsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllKindDto>>> GetAllKinds()
    {
        return await Mediator.Send(new GetAllKindsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<KindByIdDto?>> GetKindById(int id)
    {
        return await Mediator.Send(new GetKindByIdQuery(id));
    }

    [HttpGet("model{modelId}")]
    public async Task<ActionResult<List<KindByModelIdDto>>> GetKindsByModelId(int modelId)
    {
        return await Mediator.Send(new GetKindsByModelIdQuery(modelId));
    }
}
