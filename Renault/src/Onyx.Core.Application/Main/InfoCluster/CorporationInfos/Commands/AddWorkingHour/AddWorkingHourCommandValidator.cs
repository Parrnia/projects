using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddWorkingHour;
public class AddWorkingHourCommandValidator : AbstractValidator<AddWorkingHourCommand>
{
    private readonly IApplicationDbContext _context;
    public AddWorkingHourCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CorporationInfoId)
            .NotEmpty().WithMessage("شناسه اطلاعات شرکت اجباریست");
        RuleFor(v => v.WorkingHour)
            .NotEmpty().WithMessage("ساعت کاری اجباریست");
    }
}
