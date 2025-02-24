using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.Export.ExportExcelPrices;
public class ExportExcelPricesQueryValidator : AbstractValidator<ExportExcelPricesQuery>
{
    public ExportExcelPricesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}