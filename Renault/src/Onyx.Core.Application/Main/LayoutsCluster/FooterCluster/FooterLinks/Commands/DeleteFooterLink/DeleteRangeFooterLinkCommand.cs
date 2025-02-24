using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.DeleteFooterLink;

public class DeleteRangeFooterLinkCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeFooterLinkCommandHandler : IRequestHandler<DeleteRangeFooterLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeFooterLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeFooterLinkCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.FooterLinks
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(FooterLink), id);
            }

            _context.FooterLinks.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
