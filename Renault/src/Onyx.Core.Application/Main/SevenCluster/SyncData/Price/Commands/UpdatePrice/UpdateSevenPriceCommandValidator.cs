using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.SevenCluster.SyncData.Price.Commands.UpdatePrice;
public class UpdateSevenPriceCommandValidator : AbstractValidator<UpdateSevenPriceCommand>
{
    public UpdateSevenPriceCommandValidator()
    {
        RuleFor(v => v.Related7SoftProductId)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Price)
            .NotEmpty().WithMessage("قیمت اجباریست")
            .GreaterThan(0).WithMessage("قیمت باید بزرگتر از صفر باشد");
    }
}