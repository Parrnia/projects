using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinks;
public class GetFooterLinksByFooterLinkContainerIdQueryValidator : AbstractValidator<GetFooterLinksByFooterLinkContainerIdQuery>
{
    public GetFooterLinksByFooterLinkContainerIdQueryValidator()
    {
        RuleFor(x => x.FooterLinkContainerId)
            .NotEmpty().WithMessage("شناسه دسته بندی لینک های فوتر اجباریست");
    }
}
