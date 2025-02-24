using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.Export.ExportExcelOptionValues;
public class ExportExcelReturnOrderItemGroupOptionValuesQueryValidator : AbstractValidator<ExportExcelReturnOrderItemGroupOptionValuesQuery>
{
    public ExportExcelReturnOrderItemGroupOptionValuesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}