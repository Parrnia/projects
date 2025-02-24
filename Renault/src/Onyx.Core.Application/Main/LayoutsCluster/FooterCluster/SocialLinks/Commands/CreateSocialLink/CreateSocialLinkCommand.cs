using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.CreateSocialLink;
public record CreateSocialLinkCommand : IRequest<int>
{
    public string Url { get; init; } = null!;
    public Guid Icon { get; init; }
    public bool IsActive { get; init; }
}

public class CreateSocialLinkCommandHandler : IRequestHandler<CreateSocialLinkCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;
    public CreateSocialLinkCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateSocialLinkCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new SocialLink()
        {
            Url = request.Url,
            Icon = request.Icon,
            IsActive = request.IsActive
        };

        await _fileStore.SaveStoredFile(
            request.Icon,
            FileCategory.SocialLink.ToString(),
            FileCategory.SocialLink,
            null,
            false,
            cancellationToken);

        _context.SocialLinks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
