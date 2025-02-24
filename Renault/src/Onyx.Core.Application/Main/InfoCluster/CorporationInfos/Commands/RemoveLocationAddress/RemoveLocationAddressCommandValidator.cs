using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveLocationAddress;
public class RemoveLocationAddressCommandValidator : AbstractValidator<RemoveLocationAddressCommand>
{
    private readonly IApplicationDbContext _context;
    public RemoveLocationAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
    }
}
