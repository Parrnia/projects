using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductCustomFields.Commands.CreateProductCustomField;
public class CreateProductCustomFieldCommandValidator : AbstractValidator<CreateProductCustomFieldCommand>
{
    public CreateProductCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}
