using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadges;
public class GetAllBadgesByOptionIdQueryValidator : AbstractValidator<GetAllBadgesByOptionIdQuery>
{
    public GetAllBadgesByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}