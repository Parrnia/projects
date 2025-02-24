using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.DeleteModel;
public record DeleteModelCommand(int Id) : IRequest<Unit>;

public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteModelCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Models
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Model), request.Id);
        }

        _context.Models.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}