using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<Review> ApplySearch(this IQueryable<Review> query, string searchTerm)
    {
        query = query.Where(o => o.Content.Contains(searchTerm)
                                   || o.Rating.ToString().Contains(searchTerm)
                                   || o.AuthorName.ToString().Contains(searchTerm)
                                   || o.Product.LocalizedName.ToString().Contains(searchTerm)
                                   || o.AuthorName.ToString().Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<Review> ApplySorting(this IQueryable<Review> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "content":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Content) : query.OrderByDescending(o => o.Content);
                break;
            case "rating":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Rating) : query.OrderByDescending(o => o.Rating);
                break;
            case "authorName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.AuthorName) : query.OrderByDescending(o => o.AuthorName);
                break;
            case "productName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Product.LocalizedName) : query.OrderByDescending(o => o.Product.LocalizedName);
                break;
            case "isActive":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.IsActive) : query.OrderByDescending(o => o.IsActive);
                break;
            default:
                query = query.OrderBy(o => o.Date);
                break;
        }

        return query;
    }
}
