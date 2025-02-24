using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Commands.CreateProductImage;
public class CreateProductImageCommandValidator : AbstractValidator<CreateProductImageCommand>
{
    public CreateProductImageCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("شناسه تصویر محصول اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Order)
            .NotEmpty().WithMessage("ترتیب اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}
