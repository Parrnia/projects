using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.UpdateProductOptionValueColor;
public class UpdateProductOptionValueColorCommandValidator : AbstractValidator<UpdateProductOptionValueColorCommand>
{
    public UpdateProductOptionValueColorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.Color)
            .NotEmpty().WithMessage("رنگ اجباریست");
        RuleFor(v => v.ProductOptionColorId)
            .NotEmpty().WithMessage("شناسه آپشن رنگ محصول اجباریست");
    }
}