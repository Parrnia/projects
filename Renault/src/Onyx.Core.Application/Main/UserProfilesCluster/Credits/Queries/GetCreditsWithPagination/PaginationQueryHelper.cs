using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditsWithPagination;
public static class PaginationQueryHelper
{
    public static IQueryable<Credit> ApplySearch(this IQueryable<Credit> query, string searchTerm)
    {
        query = query.Where(o => o.Value.ToString().Contains(searchTerm)
                                   || o.ModifierUserName.Contains(searchTerm)
                                   || o.ModifierUserId.ToString().Contains(searchTerm)
                                   || o.OrderToken != null && o.OrderToken.ToString().Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<Credit> ApplySorting(this IQueryable<Credit> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "value":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Value) : query.OrderByDescending(o => o.Value);
                break;
            case "modifierUserName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ModifierUserName) : query.OrderByDescending(o => o.ModifierUserName);
                break;
            case "modifierUserId":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ModifierUserId) : query.OrderByDescending(o => o.ModifierUserId);
                break;
            case "orderToken":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.OrderToken) : query.OrderByDescending(o => o.OrderToken);
                break;
        }

        return query;
    }
}
