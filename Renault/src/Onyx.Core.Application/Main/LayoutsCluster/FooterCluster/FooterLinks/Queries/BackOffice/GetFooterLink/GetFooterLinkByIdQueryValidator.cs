using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLink;
public class GetFooterLinkByIdQueryValidator : AbstractValidator<GetFooterLinkByIdQuery>
{
    public GetFooterLinkByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}