using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.DeleteFooterLinkContainer;

public class DeleteRangeFooterLinkContainerCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeFooterLinkContainerCommandHandler : IRequestHandler<DeleteRangeFooterLinkContainerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeFooterLinkContainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeFooterLinkContainerCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.FooterLinkContainers
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(FooterLinkContainer), id);
            }

            _context.FooterLinkContainers.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
