using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.Export.ExportExcelFooterLinks;
public class ExportExcelFooterLinksQueryValidator : AbstractValidator<ExportExcelFooterLinksQuery>
{
    public ExportExcelFooterLinksQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}