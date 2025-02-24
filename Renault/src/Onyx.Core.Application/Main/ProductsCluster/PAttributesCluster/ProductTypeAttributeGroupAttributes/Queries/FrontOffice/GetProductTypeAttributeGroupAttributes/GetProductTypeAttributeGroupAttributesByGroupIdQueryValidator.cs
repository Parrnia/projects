using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.FrontOffice.GetProductTypeAttributeGroupAttributes;
public class GetProductTypeAttributeGroupAttributesByGroupIdQueryValidator : AbstractValidator<GetProductTypeAttributeGroupAttributesByGroupIdQuery>
{
    public GetProductTypeAttributeGroupAttributesByGroupIdQueryValidator()
    {
        RuleFor(x => x.ProductTypeAttributeGroupId)
            .NotEmpty().WithMessage("شناسه گروه بندی نوع ویژگی محصول اجباریست");
    }
}