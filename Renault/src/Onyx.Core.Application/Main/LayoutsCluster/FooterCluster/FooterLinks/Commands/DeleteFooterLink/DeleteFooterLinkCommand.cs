using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.DeleteFooterLink;

public record DeleteFooterLinkCommand(int Id) : IRequest<Unit>;

public class DeleteFooterLinkCommandHandler : IRequestHandler<DeleteFooterLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteFooterLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFooterLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FooterLinks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FooterLink), request.Id);
        }

        _context.FooterLinks.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}