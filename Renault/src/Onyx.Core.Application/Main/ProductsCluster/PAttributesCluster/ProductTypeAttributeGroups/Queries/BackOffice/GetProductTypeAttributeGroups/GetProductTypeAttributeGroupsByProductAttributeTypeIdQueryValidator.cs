using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroups;
public class GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryValidator : AbstractValidator<GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery>
{
    public GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryValidator()
    {
        RuleFor(x => x.ProductAttributeTypeId)
            .NotEmpty().WithMessage("شناسه نوع ویژگی محصول اجباریست");
    }
}
