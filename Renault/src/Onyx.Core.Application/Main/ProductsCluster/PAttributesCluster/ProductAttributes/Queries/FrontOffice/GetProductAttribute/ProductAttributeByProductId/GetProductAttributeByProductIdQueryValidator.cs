using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttribute.ProductAttributeByProductId;
public class GetProductAttributeByProductIdQueryValidator : AbstractValidator<GetProductAttributeByProductIdQuery>
{
    public GetProductAttributeByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}