using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveEmailAddress;
public record RemoveEmailAddressCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public List<string> EmailAddresses { get; init; } = new List<string>();
}

public class RemoveEmailAddressCommandHandler : IRequestHandler<RemoveEmailAddressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public RemoveEmailAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(RemoveEmailAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var emailAddresses = entity.EmailAddresses.ToList();

        foreach (var emailAddress in request.EmailAddresses)
        {
            emailAddresses.Remove(emailAddress);
        }

        entity.EmailAddresses = emailAddresses;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
