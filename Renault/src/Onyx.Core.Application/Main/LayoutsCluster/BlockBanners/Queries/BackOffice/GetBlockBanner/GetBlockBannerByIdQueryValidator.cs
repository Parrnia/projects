using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBanner;
public class GetBlockBannerByIdQueryValidator : AbstractValidator<GetBlockBannerByIdQuery>
{
    public GetBlockBannerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}