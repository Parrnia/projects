using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<Provider> ApplySearch(this IQueryable<Provider> query, string searchTerm)
    {
        query = query.Where(o => o.LocalizedName.Contains(searchTerm)
                                   || o.Code.ToString().Contains(searchTerm)
                                   || o.Name.ToString().Contains(searchTerm)
                                   || (o.LocalizedCode != null && o.LocalizedCode.ToString().Contains(searchTerm))
                                   || (o.Description != null && o.Description.ToString().Contains(searchTerm))
        );

        return query;
    }
    public static IQueryable<Provider> ApplySorting(this IQueryable<Provider> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "code":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Code) : query.OrderByDescending(o => o.Code);
                break;
            case "name":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                break;
            case "localizedName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.LocalizedName) : query.OrderByDescending(o => o.LocalizedName);
                break;
            case "localizedCode":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.LocalizedCode) : query.OrderByDescending(o => o.LocalizedCode);
                break;
            case "description":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Description) : query.OrderByDescending(o => o.Description);
                break;
            default:
                query = query.OrderBy(o => o.Created);
                break;
        }

        return query;
    }
}
