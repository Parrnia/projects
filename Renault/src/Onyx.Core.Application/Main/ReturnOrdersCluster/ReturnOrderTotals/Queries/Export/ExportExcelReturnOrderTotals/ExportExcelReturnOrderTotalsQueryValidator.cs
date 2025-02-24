using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.Export.ExportExcelReturnOrderTotals;
public class ExportExcelReturnOrderTotalsQueryValidator : AbstractValidator<ExportExcelReturnOrderTotalsQuery>
{
    public ExportExcelReturnOrderTotalsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}