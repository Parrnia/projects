using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice.GetProductAttributeType;
public class GetProductAttributeTypeByIdQueryValidator : AbstractValidator<GetProductAttributeTypeByIdQuery>
{
    public GetProductAttributeTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}