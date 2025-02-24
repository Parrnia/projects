using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.CreateProductOptionColor;
public class CreateProductOptionColorCommandValidator : AbstractValidator<CreateProductOptionColorCommand>
{
    public CreateProductOptionColorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست");
    }
}
