using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست")
            //.MustAsync(BeUniqueUserIdInIdentityServer).WithMessage("کاربری با این شناسه کاربری موجود است")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");

    }

    public async Task<bool> BeUniqueUserIdInIdentityServer(Guid userIdInIdentityServer, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AllAsync(l => l.Id != userIdInIdentityServer, cancellationToken);
    }
}
