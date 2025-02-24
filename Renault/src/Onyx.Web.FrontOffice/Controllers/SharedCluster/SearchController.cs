using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetFilterResults;
using Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetSearchSuggestions;
using Onyx.Domain.Enums;

namespace Onyx.Web.FrontOffice.Controllers.SharedCluster;


public class SearchController : ApiControllerBase
{
    [HttpGet("getSearchSuggestions{productLimit},{productCategoryLimit},{query},{kindId}")]
    public async Task<ActionResult<SearchSuggestionDto>> GetSearchSuggestions(int productLimit, int productCategoryLimit,string? query, int? kindId)
    {
        var personType = CustomerTypeEnum.Personal;
        if (UserInfo != null)
        {
            personType = (CustomerTypeEnum)UserInfo.PersonType;
        }

        return await Mediator.Send(new GetSearchSuggestionsQuery(productLimit,productCategoryLimit,personType,query,kindId));
    }

    [HttpGet("getProductsByFilter")]
    public async Task<ActionResult<FilterViewModel>> GetProductsByFilter([FromQuery] GetFilterResultsQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerType = (CustomerTypeEnum)UserInfo.PersonType;
        }
        else
        {
            query.CustomerType = CustomerTypeEnum.Personal;
        }

        return await Mediator.Send(query);
    }
}
