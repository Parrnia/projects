using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveLocationAddress;
public record RemoveLocationAddressCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public List<string> LocationAddresses { get; init; } = new List<string>();
}

public class RemoveLocationAddressCommandHandler : IRequestHandler<RemoveLocationAddressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public RemoveLocationAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(RemoveLocationAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var locationAddresses = entity.LocationAddresses.ToList();

        foreach (var locationAddress in request.LocationAddresses)
        {
            locationAddresses.Remove(locationAddress);
        }

        entity.LocationAddresses = locationAddresses;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
