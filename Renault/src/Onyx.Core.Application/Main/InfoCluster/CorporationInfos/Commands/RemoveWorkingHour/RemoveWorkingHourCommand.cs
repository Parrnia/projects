using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveWorkingHour;
public record RemoveWorkingHourCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public List<string> WorkingHours { get; init; } = new List<string>();
}

public class RemoveWorkingHourCommandHandler : IRequestHandler<RemoveWorkingHourCommand, int>
{
    private readonly IApplicationDbContext _context;

    public RemoveWorkingHourCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(RemoveWorkingHourCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var workingHours = entity.WorkingHours.ToList();

        foreach (var workingHour in request.WorkingHours)
        {
            workingHours.Remove(workingHour);
        }

        entity.WorkingHours = workingHours;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
