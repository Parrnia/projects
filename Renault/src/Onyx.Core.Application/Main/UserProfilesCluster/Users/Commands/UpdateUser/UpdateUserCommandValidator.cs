using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private Guid _id;
    public UpdateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست")
            .MustAsync(BeUniqueUserIdInIdentityServer).WithMessage("کاربری با این شناسه کاربری موجود است")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }

    public async Task<bool> BeUniqueUserIdInIdentityServer(Guid userIdInIdentityServer, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(s => s.Id != _id)
            .AllAsync(l => l.Id != userIdInIdentityServer, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(Guid requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}