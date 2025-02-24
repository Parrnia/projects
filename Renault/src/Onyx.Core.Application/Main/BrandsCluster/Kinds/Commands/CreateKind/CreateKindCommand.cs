using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Commands.CreateKind;
public record CreateKindCommand : IRequest<int>
{
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int ModelId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateKindCommandHandler : IRequestHandler<CreateKindCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateKindCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateKindCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new Kind()
        {
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            ModelId = request.ModelId,
            IsActive = request.IsActive,
        };

        _context.Kinds.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
