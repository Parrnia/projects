using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelShippedOrders;
public class ExportExcelShippedOrdersQueryValidator : AbstractValidator<ExportExcelShippedOrdersQuery>
{
    public ExportExcelShippedOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}