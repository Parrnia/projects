using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainer;
public class GetFooterLinkContainerByIdQueryValidator : AbstractValidator<GetFooterLinkContainerByIdQuery>
{
    public GetFooterLinkContainerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}