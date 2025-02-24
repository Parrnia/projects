using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColor;
public class GetProductOptionColorByIdQueryValidator : AbstractValidator<GetProductOptionColorByIdQuery>
{
    public GetProductOptionColorByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}