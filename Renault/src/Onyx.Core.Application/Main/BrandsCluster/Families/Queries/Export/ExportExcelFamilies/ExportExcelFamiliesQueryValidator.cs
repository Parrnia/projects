using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.Export.ExportExcelFamilies;
public class ExportExcelFamiliesQueryValidator : AbstractValidator<ExportExcelFamiliesQuery>
{
    public ExportExcelFamiliesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}