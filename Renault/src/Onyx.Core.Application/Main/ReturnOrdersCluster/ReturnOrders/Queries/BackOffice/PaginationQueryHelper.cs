using Microsoft.EntityFrameworkCore;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<ReturnOrder> ApplySearch(this IQueryable<ReturnOrder> query, string searchTerm)
    {
        query = query.Include(c => c.Order).Where(o =>
                                      (o.Token != null && o.Token.Contains(searchTerm))
                                   || o.Number.ToString().Contains(searchTerm)
                                   || o.Order.Token.ToString().Contains(searchTerm)
                                   || o.Order.PhoneNumber.ToString().Contains(searchTerm)
                                   || o.Order.CustomerFirstName.ToString().Contains(searchTerm)
                                   || o.Order.CustomerLastName.ToString().Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<ReturnOrder> ApplySorting(this IQueryable<ReturnOrder> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "number":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Number) : query.OrderByDescending(o => o.Number);
                break;
            case "quantity":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Quantity) : query.OrderByDescending(o => o.Quantity);
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
            case "costRefundTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CostRefundType) : query.OrderByDescending(o => o.CostRefundType);
                break;
            case "currentReturnOrderStateName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ReturnOrderStateHistory.OrderBy(e => e.Created).Last().ReturnOrderStatus) : query.OrderByDescending(o => o.CostRefundType);
                break;
            case "returnOrderTransportationTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ReturnOrderTransportationType) : query.OrderByDescending(o => o.ReturnOrderTransportationType);
                break;
            case "orderNumber":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Order.Number) : query.OrderByDescending(o => o.Order.Number);
                break;
            case "phoneNumber":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Order.PhoneNumber) : query.OrderByDescending(o => o.Order.PhoneNumber);
                break;
            case "fullCustomerName":
                query = sortDirection.ToLower() == "asc"
                    ? query.OrderBy(o => o.Order.CustomerFirstName).ThenBy(o => o.Order.CustomerLastName)
                    : query.OrderBy(o => o.Order.CustomerFirstName).ThenBy(o => o.Order.CustomerLastName);
                break;
            default:
                query = query.OrderBy(o => o.CreatedAt);
                break;
        }

        return query;
    }
}
