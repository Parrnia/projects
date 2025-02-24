using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariant;
public class GetVariantByIdQueryValidator : AbstractValidator<GetVariantByIdQuery>
{
    public GetVariantByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}