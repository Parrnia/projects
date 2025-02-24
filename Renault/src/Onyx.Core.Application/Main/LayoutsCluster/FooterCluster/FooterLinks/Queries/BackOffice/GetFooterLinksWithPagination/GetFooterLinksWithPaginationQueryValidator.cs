using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinksWithPagination;
public class GetFooterLinksWithPaginationQueryValidator : AbstractValidator<GetFooterLinksWithPaginationQuery>
{
    public GetFooterLinksWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}