using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.UpdateBlogCategory;
public record UpdateBlogCategoryCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public Guid Image { get; init; }
    public int? Items { get; init; }
    public int? BlogParentCategoryId { get; init; }
    public bool IsActive { get; init; }

}

public class UpdateBlogCategoryCommandHandler : IRequestHandler<UpdateBlogCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateBlogCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlogCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(BlogCategory), request.Id);
        }

        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.Slug = request.LocalizedName.ToLower().Replace(' ', '-');
        entity.BlogParentCategoryId = request.BlogParentCategoryId;
        entity.IsActive = request.IsActive;

        if (entity.Image != request.Image)
        {
            
            await _fileStore.RemoveStoredFile(entity.Image, cancellationToken);


            await _fileStore.SaveStoredFile(
                request.Image,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Image = request.Image;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}