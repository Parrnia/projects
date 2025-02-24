using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveEmailAddress;
public class RemoveEmailAddressCommandValidator : AbstractValidator<RemoveEmailAddressCommand>
{
    private readonly IApplicationDbContext _context;
    public RemoveEmailAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
    }
}
