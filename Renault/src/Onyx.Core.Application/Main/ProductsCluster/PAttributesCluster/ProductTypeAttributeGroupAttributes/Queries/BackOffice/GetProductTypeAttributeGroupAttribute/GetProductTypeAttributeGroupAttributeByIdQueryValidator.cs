using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttribute;
public class GetProductTypeAttributeGroupAttributeByIdQueryValidator : AbstractValidator<GetProductTypeAttributeGroupAttributeByIdQuery>
{
    public GetProductTypeAttributeGroupAttributeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}