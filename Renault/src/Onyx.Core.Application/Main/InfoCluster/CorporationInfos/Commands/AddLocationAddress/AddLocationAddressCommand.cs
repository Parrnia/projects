using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddLocationAddress;
public record AddLocationAddressCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public string LocationAddress { get; init; } = null!;
}

public class AddLocationAddressCommandHandler : IRequestHandler<AddLocationAddressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddLocationAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddLocationAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var locationAddresses = entity.LocationAddresses.ToList();
        locationAddresses.Add(request.LocationAddress);

        entity.LocationAddresses = locationAddresses;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
