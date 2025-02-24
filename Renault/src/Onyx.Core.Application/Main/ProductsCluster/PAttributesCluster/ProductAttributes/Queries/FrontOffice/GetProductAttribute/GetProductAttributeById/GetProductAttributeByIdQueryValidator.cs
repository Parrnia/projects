using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttribute.GetProductAttributeById;
public class GetProductAttributeByIdQueryValidator : AbstractValidator<GetProductAttributeByIdQuery>
{
    public GetProductAttributeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}