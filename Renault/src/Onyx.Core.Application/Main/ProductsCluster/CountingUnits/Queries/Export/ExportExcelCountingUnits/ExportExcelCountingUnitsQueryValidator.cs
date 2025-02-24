using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.Export.ExportExcelCountingUnits;
public class ExportExcelCountingUnitsQueryValidator : AbstractValidator<ExportExcelCountingUnitsQuery>
{
    public ExportExcelCountingUnitsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}