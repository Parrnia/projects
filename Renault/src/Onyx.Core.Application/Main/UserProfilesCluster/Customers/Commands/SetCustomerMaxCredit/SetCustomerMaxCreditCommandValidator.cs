using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerMaxCredit;
public class SetCustomerMaxCreditCommandValidator : AbstractValidator<SetCustomerMaxCreditCommand>
{
    private readonly IApplicationDbContext _context;
    public SetCustomerMaxCreditCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.MaxCredit)
            .GreaterThanOrEqualTo(0).WithMessage("سقف اعتبار مشتری اجباریست");
        RuleFor(v => v.ModifierUserId)
            .NotEmpty().WithMessage("شناسه کاربر تغییردهنده اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.ModifierUserName)
            .NotEmpty().WithMessage("نام کاربر تغییردهنده اجباریست");
    }
}