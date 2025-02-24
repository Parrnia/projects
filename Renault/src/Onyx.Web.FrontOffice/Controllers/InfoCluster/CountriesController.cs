using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountries;
using Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountriesWithPagination;
using Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountry;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class CountriesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CountryWithPaginationDto>>> GetCountriesWithPagination([FromQuery] GetCountriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllCountryDto>>> GetAllCountries()
    {
        return await Mediator.Send(new GetAllCountriesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountryByIdDto?>> GetCountryById(int id)
    {
        return await Mediator.Send(new GetCountryByIdQuery(id));
    }
}
