using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddWorkingHour;
public record AddWorkingHourCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public string WorkingHour { get; init; } = null!;
}

public class AddWorkingHourCommandHandler : IRequestHandler<AddWorkingHourCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddWorkingHourCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddWorkingHourCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var workingHours = entity.WorkingHours.ToList();
        workingHours.Add(request.WorkingHour);

        entity.WorkingHours = workingHours;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
