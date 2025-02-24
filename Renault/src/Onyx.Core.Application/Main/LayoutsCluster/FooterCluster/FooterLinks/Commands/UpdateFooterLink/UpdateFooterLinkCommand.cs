using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.UpdateFooterLink;
public record UpdateFooterLinkCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
    public int FooterLinkContainerId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateFooterLinkCommandHandler : IRequestHandler<UpdateFooterLinkCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateFooterLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateFooterLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FooterLinks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FooterLink), request.Id);
        }

        entity.Title = request.Title;
        entity.Url = request.Url;
        entity.FooterLinkContainerId = request.FooterLinkContainerId;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}