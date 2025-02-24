using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.UpdateCountingUnit;
public record UpdateCountingUnitCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool IsDecimal { get; init; }
    public int? CountingUnitTypeId { get; init; }
}

public class UpdateCountingUnitCommandHandler : IRequestHandler<UpdateCountingUnitCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCountingUnitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCountingUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CountingUnits
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CountingUnit), request.Id);
        }

        entity.Code = request.Code;
        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.IsDecimal = request.IsDecimal;
        entity.CountingUnitTypeId = request.CountingUnitTypeId;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}