using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemovePhoneNumber;
public class RemovePhoneNumberCommandValidator : AbstractValidator<RemovePhoneNumberCommand>
{
    private readonly IApplicationDbContext _context;
    public RemovePhoneNumberCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
    }
}
