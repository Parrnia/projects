using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProvider;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProviders;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProvidersWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class ProvidersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProviderWithPaginationDto>>> GetProvidersWithPagination([FromQuery] GetProvidersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllProviderDto>>> GetAllProviders()
    {
        return await Mediator.Send(new GetAllProvidersQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProviderByIdDto?>> GetProviderById(int id)
    {
        return await Mediator.Send(new GetProviderByIdQuery(id));
    }
}
