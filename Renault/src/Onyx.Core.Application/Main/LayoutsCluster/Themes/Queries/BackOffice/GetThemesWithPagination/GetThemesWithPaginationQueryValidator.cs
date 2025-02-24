using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetThemesWithPagination;
public class GetThemesWithPaginationQueryValidator : AbstractValidator<GetThemesWithPaginationQuery>
{
    public GetThemesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}