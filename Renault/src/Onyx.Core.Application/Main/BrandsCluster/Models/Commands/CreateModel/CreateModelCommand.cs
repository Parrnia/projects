using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.CreateModel;
public record CreateModelCommand : IRequest<int>
{
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int FamilyId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateModelCommand request, CancellationToken cancellationToken)
    {
        var entity = new Model()
        {
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            FamilyId = request.FamilyId,
            IsActive = request.IsActive,
            //Kinds = request.Kinds.Select(f => new Kind()
            //{
            //    LocalizedName = f.LocalizedName,
            //    Name = f.Name
            //}).ToList()
        };

        _context.Models.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
