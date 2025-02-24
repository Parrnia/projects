using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.UpdateProductCategory;
public record UpdateProductCategoryCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? ProductCategoryNo { get; init; }
    public Guid? Image { get; init; }
    public Guid? MenuImage { get; init; }
    public int? ProductParentCategoryId { get; init; }
    public bool IsPopular { get; init; }
    public bool IsFeatured { get; init; }
    public bool IsActive { get; init; }

}

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateProductCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _context.ProductCategories
            .Include(c => c.ProductParentCategory)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(nameof(ProductCategory), request.Id);
        }

        productCategory.Code = request.Code;
        productCategory.LocalizedName = request.LocalizedName;
        productCategory.Name = request.Name;
        productCategory.Slug = request.LocalizedName.ToLower().Replace(' ', '-');
        productCategory.ProductCategoryNo = request.ProductCategoryNo;
        productCategory.ProductParentCategoryId = request.ProductParentCategoryId;
        productCategory.IsFeatured = request.IsFeatured;
        productCategory.IsPopular = request.IsPopular;
        productCategory.IsActive = request.IsActive;

        if (productCategory.Image != request.Image)
        {
            if (productCategory.Image != null)
            {
                await _fileStore.RemoveStoredFile((Guid)productCategory.Image, cancellationToken);
            }

            if (request.Image != null)
            {
                await _fileStore.SaveStoredFile(
                    (Guid)request.Image,
                    FileCategory.ProductCategory.ToString(),
                    FileCategory.ProductCategory,
                    null,
                    false,
                    cancellationToken);
            }
            productCategory.Image = request.Image;
        }



        var link = await _context.Links
            .SingleOrDefaultAsync(c => c.RelatedProductCategoryId == productCategory.Id, cancellationToken);
        if (link == null)
        {
            throw new NotFoundException(nameof(Link), productCategory.Id);
        }

        link.Title = productCategory.LocalizedName;
        link.Url = "/shop/category/" + productCategory.Slug + "/products";
        link.RelatedProductCategoryId = productCategory.Id;
        link.IsActive = request.IsActive;


        if (productCategory.MenuImage != request.MenuImage)
        {
            if (productCategory.MenuImage != null)
            {
                await _fileStore.RemoveStoredFile((Guid)productCategory.MenuImage, cancellationToken);
            }

            if (request.MenuImage != null)
            {
                await _fileStore.SaveStoredFile(
                    (Guid)request.MenuImage,
                    FileCategory.Link.ToString(),
                    FileCategory.Link,
                    null,
                    false,
                    cancellationToken);
            }
            link.Image = request.MenuImage;
        }

        if (productCategory.ProductParentCategoryId != null)
        {
            var parentLink = await _context.Links
                .SingleOrDefaultAsync(c => c.RelatedProductCategoryId == productCategory.ProductParentCategoryId, cancellationToken);

            if (parentLink == null)
            {
                throw new NotFoundException(nameof(Link), productCategory.ProductParentCategoryId);
            }
            link.ParentLink = parentLink;
        }




        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}