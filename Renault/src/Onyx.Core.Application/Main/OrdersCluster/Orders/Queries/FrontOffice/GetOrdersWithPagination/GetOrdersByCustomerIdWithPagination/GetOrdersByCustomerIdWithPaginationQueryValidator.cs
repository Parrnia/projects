using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrdersWithPagination.GetOrdersByCustomerIdWithPagination;
public class GetOrdersByCustomerIdWithPaginationQueryValidator : AbstractValidator<GetOrdersByCustomerIdWithPaginationQuery>
{
    public GetOrdersByCustomerIdWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}