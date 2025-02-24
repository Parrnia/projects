using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductCustomFields.Commands.UpdateProductCustomField;
public class UpdateProductCustomFieldCommandValidator : AbstractValidator<UpdateProductCustomFieldCommand>
{
    public UpdateProductCustomFieldCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FieldName)
            .NotEmpty().WithMessage("نام فیلد اجباریست");
        RuleFor(v => v.Value)
            .NotEmpty().WithMessage("مقدار اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}