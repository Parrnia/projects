using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.CreateFooterLink;
public record CreateFooterLinkCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
    public int FooterLinkContainerId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateFooterLinkCommandHandler : IRequestHandler<CreateFooterLinkCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateFooterLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFooterLinkCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new FooterLink()
        {
            Title = request.Title,
            Url = request.Url,
            FooterLinkContainerId = request.FooterLinkContainerId,
            IsActive = request.IsActive
        };


        _context.FooterLinks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
