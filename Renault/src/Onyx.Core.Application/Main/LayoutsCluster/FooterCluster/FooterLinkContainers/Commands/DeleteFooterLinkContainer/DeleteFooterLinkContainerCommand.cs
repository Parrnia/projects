using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.DeleteFooterLinkContainer;

public record DeleteFooterLinkContainerCommand(int Id) : IRequest<Unit>;

public class DeleteFooterLinkContainerCommandHandler : IRequestHandler<DeleteFooterLinkContainerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteFooterLinkContainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFooterLinkContainerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FooterLinkContainers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FooterLinkContainer), request.Id);
        }

        _context.FooterLinkContainers.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}