using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<Tag> ApplySearch(this IQueryable<Tag> query, string searchTerm)
    {
        query = query.Where(o => o.EnTitle.Contains(searchTerm)
                                   || o.FaTitle.Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<Tag> ApplySorting(this IQueryable<Tag> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "enTitle":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.EnTitle) : query.OrderByDescending(o => o.EnTitle);
                break;
            case "faTitle":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.FaTitle) : query.OrderByDescending(o => o.FaTitle);
                break;
            default:
                query = query.OrderBy(o => o.Created);
                break;
        }

        return query;
    }
}
