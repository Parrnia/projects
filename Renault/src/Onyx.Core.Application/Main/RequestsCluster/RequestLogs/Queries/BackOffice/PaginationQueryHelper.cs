using System.Net;
using Onyx.Application.Services;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<RequestLog> ApplySearch(this IQueryable<RequestLog> query, string searchTerm)
    {
        query = query.Where(o => (o.ApiAddress.Contains(searchTerm)
                                  || (o.RequestBody != null && o.RequestBody.Contains(searchTerm))
                                  || (o.ErrorMessage != null && o.ErrorMessage.Contains(searchTerm))
                                  || EnumHelper<HttpStatusCode>.GetDisplayValue(o.ResponseStatus).Contains(searchTerm)
                                  || EnumHelper<RequestType>.GetDisplayValue(o.RequestType).Contains(searchTerm)
                                  || EnumHelper<ApiType>.GetDisplayValue(o.ApiType).Contains(searchTerm)
                                  || o.Created.ToPersianDate().Contains(searchTerm)));

        return query;
    }
    public static IQueryable<RequestLog> ApplySorting(this IQueryable<RequestLog> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "apiAddress":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ApiAddress) : query.OrderByDescending(o => o.ApiAddress);
                break;
            case "requestBody":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.RequestBody) : query.OrderByDescending(o => o.RequestBody);
                break;
            case "errorMessage":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ErrorMessage) : query.OrderByDescending(o => o.ErrorMessage);
                break;
            case "httpStatusCode":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ResponseStatus) : query.OrderByDescending(o => o.ResponseStatus);
                break;
            case "created":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Created) : query.OrderByDescending(o => o.Created);
                break;
            case "requestTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.RequestType) : query.OrderByDescending(o => o.RequestType);
                break;
            case "apiTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ApiType) : query.OrderByDescending(o => o.ApiType);
                break;
            default:
                query = query.OrderBy(o => o.Created);
                break;
        }

        return query;
    }
}
