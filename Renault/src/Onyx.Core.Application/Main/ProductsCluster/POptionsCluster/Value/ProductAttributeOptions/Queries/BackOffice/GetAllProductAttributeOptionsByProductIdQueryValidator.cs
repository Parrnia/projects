using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;
public class GetAllProductAttributeOptionsByProductIdQueryValidator : AbstractValidator<FrontOffice.GetProductAttributeOptions.GetAllProductAttributeOptionsByProductIdQuery>
{
    public GetAllProductAttributeOptionsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}