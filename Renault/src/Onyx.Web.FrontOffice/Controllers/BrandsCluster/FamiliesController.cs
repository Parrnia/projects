using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetAllFamilies;
using Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetFamiliesByBrandId;
using Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamiliesWithPagination.GetAllFamiliesWithPagination;
using Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamily.GetFamilyById;

namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;


public class FamiliesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<AllFamilyWithPaginationDto>>> GetAllFamiliesWithPagination([FromQuery] GetAllFamiliesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllFamilyDto>>> GetAllFamilies()
    {
        return await Mediator.Send(new GetAllFamiliesQuery());
    }

    [HttpGet("brand{brandId}")]
    public async Task<ActionResult<List<FamilyByBrandIdDto>>> GetFamiliesByBrandId(int brandId)
    {
        return await Mediator.Send(new GetFamiliesByBrandIdQuery(brandId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FamilyByIdDto?>> GetFamilyById(int id)
    {
        return await Mediator.Send(new GetFamilyByIdQuery(id));
    }
}
