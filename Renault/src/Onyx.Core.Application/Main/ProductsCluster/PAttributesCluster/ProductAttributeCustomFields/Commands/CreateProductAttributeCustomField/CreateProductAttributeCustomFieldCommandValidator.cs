using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeCustomFields.Commands.CreateProductAttributeCustomField;
public class CreateProductCustomFieldCommandValidator : AbstractValidator<CreateProductAttributeCustomFieldCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCustomFieldCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductAttributeId)
            .NotEmpty().WithMessage("شناسه ویژگی محصول اجباریست");
    }
}
