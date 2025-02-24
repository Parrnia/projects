using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.Export.ExportExcelProductAttributeOptionRoles;
public class ExportExcelProductAttributeOptionRolesQueryValidator : AbstractValidator<ExportExcelProductAttributeOptionRolesQuery>
{
    public ExportExcelProductAttributeOptionRolesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}