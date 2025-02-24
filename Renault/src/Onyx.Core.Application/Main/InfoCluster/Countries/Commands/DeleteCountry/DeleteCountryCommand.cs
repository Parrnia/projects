using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Countries.Commands.DeleteCountry;

public record DeleteCountryCommand(int Id) : IRequest<Unit>;

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCountryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Countries
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Country), request.Id);
        }

        _context.Countries.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}