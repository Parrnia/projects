using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.Export.ExportExcelCustomerTickets;
public class ExportExcelSocialLinksQueryValidator : AbstractValidator<ExportExcelSocialLinksQuery>
{
    public ExportExcelSocialLinksQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}