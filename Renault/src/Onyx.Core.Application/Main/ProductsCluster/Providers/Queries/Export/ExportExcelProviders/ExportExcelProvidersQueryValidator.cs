using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.Export.ExportExcelProviders;
public class ExportExcelProvidersQueryValidator : AbstractValidator<ExportExcelProvidersQuery>
{
    public ExportExcelProvidersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}