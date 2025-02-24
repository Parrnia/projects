using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Countries.Commands.UpdateCountry;
public record UpdateCountryCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool IsActive { get; init; }
}

public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCountryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Countries
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Country), request.Id);
        }

        entity.Code = request.Code;
        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}