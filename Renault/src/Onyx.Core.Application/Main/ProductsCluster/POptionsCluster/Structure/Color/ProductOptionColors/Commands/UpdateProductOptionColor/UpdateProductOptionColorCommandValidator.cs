using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.UpdateProductOptionColor;
public class UpdateProductOptionColorCommandValidator : AbstractValidator<UpdateProductOptionColorCommand>
{
    public UpdateProductOptionColorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
    }
}