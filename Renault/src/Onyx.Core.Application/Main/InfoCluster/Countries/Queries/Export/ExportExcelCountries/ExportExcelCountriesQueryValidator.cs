using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.Export.ExportExcelCountries;
public class ExportExcelCountriesQueryValidator : AbstractValidator<ExportExcelCountriesQuery>
{
    public ExportExcelCountriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}