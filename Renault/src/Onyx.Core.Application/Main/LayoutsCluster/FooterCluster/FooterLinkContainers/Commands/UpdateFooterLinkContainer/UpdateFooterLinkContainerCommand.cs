using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.UpdateFooterLinkContainer;
public record UpdateFooterLinkContainerCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Header { get; init; } = null!;
    public int Order { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateFooterLinkContainerCommandHandler : IRequestHandler<UpdateFooterLinkContainerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateFooterLinkContainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateFooterLinkContainerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FooterLinkContainers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FooterLinkContainer), request.Id);
        }

        entity.Header = request.Header;
        entity.Order = request.Order;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}