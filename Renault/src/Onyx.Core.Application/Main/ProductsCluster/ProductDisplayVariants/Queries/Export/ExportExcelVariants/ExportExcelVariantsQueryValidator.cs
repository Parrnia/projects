using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.Export.ExportExcelVariants;
public class ExportExcelVariantsQueryValidator : AbstractValidator<ExportExcelVariantsQuery>
{
    public ExportExcelVariantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}