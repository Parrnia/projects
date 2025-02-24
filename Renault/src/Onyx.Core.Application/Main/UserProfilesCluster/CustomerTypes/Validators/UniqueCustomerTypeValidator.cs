using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Validators;

public record UniqueCustomerTypeValidator : IRequest<bool>
{
    public int CustomerTypeId { get; init; }

    public CustomerTypeEnum CustomerTypeEnum { get; init; }
}

public class UniqueRoleIdInIdentityServerValidatorHandler : IRequestHandler<UniqueCustomerTypeValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueRoleIdInIdentityServerValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCustomerTypeValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.CustomerTypes.Where(c => c.Id != request.CustomerTypeId)
            .AllAsync(e => e.CustomerTypeEnum != request.CustomerTypeEnum, cancellationToken);
        return isUnique;
    }
}
