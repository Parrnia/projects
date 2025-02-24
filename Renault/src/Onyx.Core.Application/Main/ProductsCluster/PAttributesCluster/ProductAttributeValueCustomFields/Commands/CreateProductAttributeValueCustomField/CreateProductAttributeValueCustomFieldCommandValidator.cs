using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeValueCustomFields.Commands.CreateProductAttributeValueCustomField;
public class CreateProductAttributeValueCustomFieldCommandValidator : AbstractValidator<CreateProductAttributeValueCustomFieldCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeValueCustomFieldCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductAttributeId)
            .NotEmpty().WithMessage("شناسه مقدار ویژگی محصول اجباریست");
    }
}
