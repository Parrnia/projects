using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice.GetSocialLink;
public class GetSocialLinkByIdQueryValidator : AbstractValidator<GetSocialLinkByIdQuery>
{
    public GetSocialLinkByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}