using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelOrders;
public class ExportExcelOrdersQueryValidator : AbstractValidator<ExportExcelOrdersQuery>
{
    public ExportExcelOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}