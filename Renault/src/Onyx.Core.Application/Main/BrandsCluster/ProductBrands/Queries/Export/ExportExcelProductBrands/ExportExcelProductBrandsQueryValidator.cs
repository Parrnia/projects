using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.Export.ExportExcelProductBrands;
public class ExportExcelProductBrandsQueryValidator : AbstractValidator<ExportExcelProductBrandsQuery>
{
    public ExportExcelProductBrandsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}