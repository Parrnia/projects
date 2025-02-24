using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.CreateTeamMember;
public class CreateTeamMemberCommandValidator : AbstractValidator<CreateTeamMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTeamMemberCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("عضو تیمی با این نام لاتین موجود است")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.Position)
            .NotEmpty().WithMessage("سمت اجباریست");
        RuleFor(v => v.Avatar)
            .NotEmpty().WithMessage("شناسه تصویر فرد اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.AboutUsId)
            .NotEmpty().WithMessage("شناسه درباره ما اجباریست");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
