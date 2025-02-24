using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.Export.ExportExcelOrderTotals;
public class ExportExcelOrderTotalsQueryValidator : AbstractValidator<ExportExcelOrderTotalsQuery>
{
    public ExportExcelOrderTotalsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}