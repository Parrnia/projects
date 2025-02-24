using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.UpdatePrice;
public class UpdatePriceCommandValidator : AbstractValidator<UpdatePriceCommand>
{
    public UpdatePriceCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Date)
            .NotEmpty().WithMessage("تاریخ اجباریست");
        RuleFor(v => v.MainPrice)
            .NotEmpty().WithMessage("مقدار قیمت اجباریست");
        RuleFor(v => v.ProductAttributeOptionId)
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");
    }
}