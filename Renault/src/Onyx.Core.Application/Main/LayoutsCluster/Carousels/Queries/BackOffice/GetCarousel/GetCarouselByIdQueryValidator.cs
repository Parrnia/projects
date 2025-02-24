using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarousel;
public class GetCarouselByIdQueryValidator : AbstractValidator<GetCarouselByIdQuery>
{
    public GetCarouselByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}