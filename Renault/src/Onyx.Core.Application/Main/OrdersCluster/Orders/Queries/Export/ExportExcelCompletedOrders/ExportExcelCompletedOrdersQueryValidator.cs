using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelCompletedOrders;
public class ExportExcelCompletedOrdersQueryValidator : AbstractValidator<ExportExcelCompletedOrdersQuery>
{
    public ExportExcelCompletedOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}