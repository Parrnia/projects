using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice.GetProductOptionColor;
public class GetAllProductOptionValueColorsByColorIdQueryValidator : AbstractValidator<GetProductOptionColorByIdQuery>
{
    public GetAllProductOptionValueColorsByColorIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}