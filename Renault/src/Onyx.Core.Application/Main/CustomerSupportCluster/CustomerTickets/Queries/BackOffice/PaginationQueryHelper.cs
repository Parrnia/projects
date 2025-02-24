using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<CustomerTicket> ApplySearch(this IQueryable<CustomerTicket> query, string searchTerm)
    {
        query = query.Where(o => o.Subject.Contains(searchTerm)
                                   || o.Message.Contains(searchTerm)
                                   || o.CustomerPhoneNumber.Contains(searchTerm)
                                   || o.CustomerName.Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<CustomerTicket> ApplySorting(this IQueryable<CustomerTicket> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "subject":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Subject) : query.OrderByDescending(o => o.Subject);
                break;
            case "message":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Message) : query.OrderByDescending(o => o.Message);
                break;
            case "customerPhoneNumber":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CustomerPhoneNumber) : query.OrderByDescending(o => o.CustomerPhoneNumber);
                break;
            case "customerName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CustomerName) : query.OrderByDescending(o => o.CustomerName);
                break;
            case "isActive":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.IsActive) : query.OrderByDescending(o => o.IsActive);
                break;
            case "date":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Date) : query.OrderByDescending(o => o.Date);
                break;
            default:
                query = query.OrderBy(o => o.Subject);
                break;
        }

        return query;
    }
}
