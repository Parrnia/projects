using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.CreateFooterLinkContainer;
public record CreateFooterLinkContainerCommand : IRequest<int>
{
    public string Header { get; init; } = null!;
    public int Order { get; init; }
    public bool IsActive { get; init; }
}

public class CreateFooterLinkContainerCommandHandler : IRequestHandler<CreateFooterLinkContainerCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateFooterLinkContainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFooterLinkContainerCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new FooterLinkContainer()
        {
            Header = request.Header,
            Order = request.Order,
            IsActive = request.IsActive
        };


        _context.FooterLinkContainers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
