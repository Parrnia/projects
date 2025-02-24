using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddPhoneNumber;
public class AddPhoneNumberCommandValidator : AbstractValidator<AddPhoneNumberCommand>
{
    private readonly IApplicationDbContext _context;
    public AddPhoneNumberCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage("شماره تماس اجباریست");
    }
}
