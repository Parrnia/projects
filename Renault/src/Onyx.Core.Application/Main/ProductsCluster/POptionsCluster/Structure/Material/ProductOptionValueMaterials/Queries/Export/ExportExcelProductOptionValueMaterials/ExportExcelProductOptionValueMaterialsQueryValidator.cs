using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.Export.ExportExcelProductOptionValueMaterials;
public class ExportExcelProductOptionValueMaterialsQueryValidator : AbstractValidator<ExportExcelProductOptionValueMaterialsQuery>
{
    public ExportExcelProductOptionValueMaterialsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}