using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditsWithPagination;
public static class PaginationQueryHelper
{
    public static IQueryable<MaxCredit> ApplySearch(this IQueryable<MaxCredit> query, string searchTerm)
    {
        query = query.Where(o => o.Value.ToString().Contains(searchTerm)
                                   || o.ModifierUserName.Contains(searchTerm)
                                   || o.ModifierUserId.ToString().Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<MaxCredit> ApplySorting(this IQueryable<MaxCredit> query, string sortColumn, string sortDirection)
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
        }

        return query;
    }
}
