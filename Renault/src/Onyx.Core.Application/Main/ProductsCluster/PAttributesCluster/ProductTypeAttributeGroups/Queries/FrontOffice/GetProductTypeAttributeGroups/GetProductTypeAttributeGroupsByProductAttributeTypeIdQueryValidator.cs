using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.FrontOffice.GetProductTypeAttributeGroups;
public class GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryValidator : AbstractValidator<BackOffice.GetProductTypeAttributeGroups.GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery>
{
    public GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryValidator()
    {
        RuleFor(x => x.ProductAttributeTypeId)
            .NotEmpty().WithMessage("شناسه نوع ویژگی محصول اجباریست");
    }
}
