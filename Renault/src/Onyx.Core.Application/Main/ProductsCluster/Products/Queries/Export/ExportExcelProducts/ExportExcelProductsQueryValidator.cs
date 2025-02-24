using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.Export.ExportExcelProducts;
public class ExportExcelProductsQueryValidator : AbstractValidator<ExportExcelProductsQuery>
{
    public ExportExcelProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}