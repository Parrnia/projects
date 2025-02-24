using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id).Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        //    .MustAsync(BeUniqueUserId)
        //    .WithMessage("The specified UserIdInIdentityServer already exists.");

    }

    public async Task<bool> BeUniqueUserId(Guid userIdInIdentityServer, CancellationToken cancellationToken)
    {
        return await _context.Users.AllAsync(l => l.Id != userIdInIdentityServer, cancellationToken);
    }
}