using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.Export.ExportExcelCountingUnitTypes;
public class ExportExcelCountingUnitTypesQueryValidator : AbstractValidator<ExportExcelCountingUnitTypesQuery>
{
    public ExportExcelCountingUnitTypesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}