using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.CreateBlogCategory;
public record CreateBlogCategoryCommand : IRequest<int>
{
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public Guid Image { get; init; }
    public int? Items { get; init; }
    public int? BlogParentCategoryId { get; init; }
    public bool IsActive { get; init; }
}

public class CreateBlogCategoryCommandHandler : IRequestHandler<CreateBlogCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateBlogCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new BlogCategory()
        {
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            Slug = request.LocalizedName.ToLower().Replace(' ', '-'),
            Image = request.Image,
            BlogParentCategoryId = request.BlogParentCategoryId,
            IsActive = request.IsActive
        };

        await _fileStore.SaveStoredFile(
            request.Image,
            FileCategory.BlogCategory.ToString(),
            FileCategory.BlogCategory,
            null,
            false,
            cancellationToken);

        _context.BlogCategories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}