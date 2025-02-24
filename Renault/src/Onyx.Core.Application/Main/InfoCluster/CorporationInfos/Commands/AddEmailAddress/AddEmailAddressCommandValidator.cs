using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddEmailAddress;
public class AddEmailAddressCommandValidator : AbstractValidator<AddEmailAddressCommand>
{
    private readonly IApplicationDbContext _context;
    public AddEmailAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
        RuleFor(v => v.EmailAddress)
            .NotEmpty().WithMessage("آدرس ایمیل اجباریست");
    }
}
