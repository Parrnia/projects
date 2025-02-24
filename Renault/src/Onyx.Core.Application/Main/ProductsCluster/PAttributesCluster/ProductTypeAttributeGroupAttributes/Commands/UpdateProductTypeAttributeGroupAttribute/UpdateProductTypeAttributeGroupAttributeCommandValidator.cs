using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.UpdateProductTypeAttributeGroupAttribute;
public class UpdateProductTypeAttributeGroupAttributeCommandValidator : AbstractValidator<UpdateProductTypeAttributeGroupAttributeCommand>
{

    public UpdateProductTypeAttributeGroupAttributeCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductTypeAttributeGroupId)
            .NotEmpty().WithMessage("شناسه گروه بندی نوع ویژگی محصول اجباریست");
    }
}