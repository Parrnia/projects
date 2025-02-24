using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.SevenCluster.SyncData.Count.Commands.UpdateCount;
public class UpdateSevenCountCommandValidator : AbstractValidator<UpdateSevenCountCommand>
{
    public UpdateSevenCountCommandValidator()
    {
        RuleFor(v => v.Related7SoftProductId)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Count)
            .NotEmpty().WithMessage("تعداد اجباریست")
            .GreaterThanOrEqualTo(0).WithMessage("تعداد باید بزرگتر یا مساوی صفر باشد");
    }
}