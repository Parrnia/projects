using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Tags.Commands.DeleteTag;

public record DeleteTagCommand(int Id) : IRequest<Unit>;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Tag), request.Id);
        }

        _context.Tags.Remove(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}