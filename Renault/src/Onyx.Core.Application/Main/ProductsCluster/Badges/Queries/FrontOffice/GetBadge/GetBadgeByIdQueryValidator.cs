using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadge;
public class GetBadgeByIdQueryValidator : AbstractValidator<GetBadgeByIdQuery>
{
    public GetBadgeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}