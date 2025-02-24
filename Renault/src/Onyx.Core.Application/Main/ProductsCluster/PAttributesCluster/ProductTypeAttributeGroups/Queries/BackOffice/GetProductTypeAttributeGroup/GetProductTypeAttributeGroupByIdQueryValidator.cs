using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroup;
public class GetProductTypeAttributeGroupByIdQueryValidator : AbstractValidator<GetProductTypeAttributeGroupByIdQuery>
{
    public GetProductTypeAttributeGroupByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}