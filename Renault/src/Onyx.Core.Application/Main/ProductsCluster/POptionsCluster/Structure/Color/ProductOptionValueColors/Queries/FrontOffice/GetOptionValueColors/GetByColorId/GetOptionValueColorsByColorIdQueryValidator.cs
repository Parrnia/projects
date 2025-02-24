using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.FrontOffice.GetOptionValueColors.GetByColorId;
public class GetOptionValueColorsByColorIdQueryValidator : AbstractValidator<GetOptionValueColorsByColorIdQuery>
{
    public GetOptionValueColorsByColorIdQueryValidator()
    {
        RuleFor(x => x.ColorId)
            .NotEmpty().WithMessage("شناسه رنگ اجباریست");
    }
}