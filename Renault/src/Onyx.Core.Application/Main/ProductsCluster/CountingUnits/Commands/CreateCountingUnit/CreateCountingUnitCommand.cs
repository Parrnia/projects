using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.CreateCountingUnit;
public record CreateCountingUnitCommand : IRequest<int>
{
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool IsDecimal { get; init; }
    public int? CountingUnitTypeId { get; init; }

}

public class CreateCountingUnitCommandHandler : IRequestHandler<CreateCountingUnitCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCountingUnitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCountingUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = new CountingUnit()
        {
            Code = request.Code,
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            IsDecimal = request.IsDecimal,
            CountingUnitTypeId = request.CountingUnitTypeId
        };



        _context.CountingUnits.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
