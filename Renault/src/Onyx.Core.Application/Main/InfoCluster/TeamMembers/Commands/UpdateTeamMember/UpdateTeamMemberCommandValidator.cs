using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Commands.UpdateTeamMember;
public class UpdateTeamMemberCommandValidator : AbstractValidator<UpdateTeamMemberCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateTeamMemberCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName).WithMessage("چیزی با این نام لاتین موجود است")
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
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}