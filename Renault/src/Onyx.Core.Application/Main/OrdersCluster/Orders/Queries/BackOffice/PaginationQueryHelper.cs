using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<Order> ApplySearch(this IQueryable<Order> query, string searchTerm)
    {
        query = query.Where(o => o.Token.Contains(searchTerm)
                                   || o.Number.ToString().Contains(searchTerm)
                                   || o.PhoneNumber.ToString().Contains(searchTerm)
                                   || o.CustomerFirstName.ToString().Contains(searchTerm)
                                   || o.CustomerLastName.ToString().Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<Order> ApplySorting(this IQueryable<Order> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "number":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Number) : query.OrderByDescending(o => o.Number);
                break;
            case "quantity":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Quantity) : query.OrderByDescending(o => o.Quantity);
                break;
            case "orderPaymentTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.OrderPaymentType) : query.OrderByDescending(o => o.OrderPaymentType);
                break;
            case "isPayed":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.IsPayed) : query.OrderByDescending(o => o.IsPayed);
                break;
            case "customerTypeEnumName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CustomerTypeEnum) : query.OrderByDescending(o => o.CustomerTypeEnum);
                break;
            case "phoneNumber":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.PhoneNumber) : query.OrderByDescending(o => o.PhoneNumber);
                break;
            case "fullCustomerName":
                query = sortDirection.ToLower() == "asc"
                    ? query.OrderBy(o => o.CustomerFirstName).ThenBy(o => o.CustomerLastName)
                    : query.OrderBy(o => o.CustomerFirstName).ThenBy(o => o.CustomerLastName);
                break;
            case "subtotal":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Subtotal) : query.OrderByDescending(o => o.Subtotal);
                break;
            case "total":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Total) : query.OrderByDescending(o => o.Total);
                break;
            case "createdAt":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CreatedAt) : query.OrderByDescending(o => o.CreatedAt);
                break;
            case "discountPercentForRole":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.DiscountPercentForRole) : query.OrderByDescending(o => o.DiscountPercentForRole);
                break;
            default:
                query = query.OrderBy(o => o.CreatedAt);
                break;
        }

        return query;
    }
}
