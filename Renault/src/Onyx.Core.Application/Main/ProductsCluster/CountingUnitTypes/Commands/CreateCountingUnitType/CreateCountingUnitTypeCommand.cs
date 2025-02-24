using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.CreateCountingUnitType;
public record CreateCountingUnitTypeCommand : IRequest<int>
{
    public int Code { get; init; }
    public string Name { get; init; } = null!;
    public string LocalizedName { get; init; } = null!;
}

public class CreateCountingUnitTypeCommandHandler : IRequestHandler<CreateCountingUnitTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCountingUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCountingUnitTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new CountingUnitType
        {
            Code = request.Code,
            Name = request.Name,
            LocalizedName = request.LocalizedName

        };


        _context.CountingUnitTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
