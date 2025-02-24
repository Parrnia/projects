using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.Export.ExportExcelProductTypeAttributeGroupAttributes;
public class ExportExcelProductTypeAttributeGroupAttributesQueryValidator : AbstractValidator<ExportExcelProductTypeAttributeGroupAttributesQuery>
{
    public ExportExcelProductTypeAttributeGroupAttributesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}