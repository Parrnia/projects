using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Countries.Commands.CreateCountry;
public record CreateCountryCommand : IRequest<int>
{
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool IsActive { get; init; }
}

public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCountryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Country()
        {
            Code = request.Code,
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            IsActive = request.IsActive
        };


        _context.Countries.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
