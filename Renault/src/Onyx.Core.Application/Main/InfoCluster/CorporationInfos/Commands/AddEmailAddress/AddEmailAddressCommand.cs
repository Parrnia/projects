using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddEmailAddress;
public record AddEmailAddressCommand : IRequest<Unit>
{
    public int CorporationInfoId { get; init; }
    public string EmailAddress { get; init; } = null!;
}

public class AddEmailAddressCommandHandler : IRequestHandler<AddEmailAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddEmailAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddEmailAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }
        var emailAddresses = entity.EmailAddresses.ToList();
        emailAddresses.Add(request.EmailAddress);

        entity.EmailAddresses = emailAddresses;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
