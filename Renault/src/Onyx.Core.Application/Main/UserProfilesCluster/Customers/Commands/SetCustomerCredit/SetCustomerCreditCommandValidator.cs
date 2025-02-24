using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerCredit;
public class SetCustomerCreditCommandValidator : AbstractValidator<SetCustomerCreditCommand>
{
    private readonly IApplicationDbContext _context;
    public SetCustomerCreditCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Credit)
            .GreaterThanOrEqualTo(0).WithMessage("اعتبار مشتری اجباریست");
        RuleFor(v => v.ModifierUserId)
            .NotEmpty().WithMessage("شناسه کاربر تغییردهنده اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.ModifierUserName)
            .NotEmpty().WithMessage("نام کاربر تغییردهنده اجباریست");
    }
}