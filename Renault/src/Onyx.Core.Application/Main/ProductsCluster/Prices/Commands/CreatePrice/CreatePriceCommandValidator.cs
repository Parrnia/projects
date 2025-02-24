using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.CreatePrice;
public class CreatePriceCommandValidator : AbstractValidator<CreatePriceCommand>
{
    private readonly IApplicationDbContext _context;
    public CreatePriceCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Date)
            .NotEmpty().WithMessage("تاریخ اجباریست");
        RuleFor(v => v.MainPrice)
            .NotEmpty().WithMessage("مقدار قیمت اجباریست");
        RuleFor(v => v.ProductAttributeOptionId)
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");
    }
}
