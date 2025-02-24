using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveWorkingHour;
public class RemoveWorkingHourCommandValidator : AbstractValidator<RemoveWorkingHourCommand>
{
    private readonly IApplicationDbContext _context;
    public RemoveWorkingHourCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
    }
}
