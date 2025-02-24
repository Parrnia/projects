using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariants;
public class GetAllVariantsByProductIdQueryValidator : AbstractValidator<GetAllVariantsByProductIdQuery>
{
    public GetAllVariantsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}