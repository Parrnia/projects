using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddLocationAddress;
public class AddLocationAddressCommandValidator : AbstractValidator<AddLocationAddressCommand>
{
    private readonly IApplicationDbContext _context;
    public AddLocationAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
        RuleFor(v => v.LocationAddress)
            .NotEmpty().WithMessage("آدرس اجباریست");
    }
}
