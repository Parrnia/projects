using FluentValidation;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.Export.ExportExcelCredits;
public class ExportExcelCreditsQueryValidator : AbstractValidator<ExportExcelCreditsQuery>
{
    public ExportExcelCreditsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}