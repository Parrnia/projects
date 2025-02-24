using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.CreateProductCategory;
public record CreateProductCategoryCommand : IRequest<int>
{
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

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateProductCategoryCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = new ProductCategory()
        {
            Code = request.Code,
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            Slug = request.LocalizedName.ToLower().Replace(' ', '-'),
            ProductCategoryNo = request.ProductCategoryNo,
            Image = request.Image,
            ProductParentCategoryId = request.ProductParentCategoryId,
            IsPopular = request.IsPopular,
            IsFeatured = request.IsFeatured,
            IsActive = request.IsActive
        };
        _context.ProductCategories.Add(productCategory);


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



        await _context.SaveChangesAsync(cancellationToken);

        var dbProductCategory = await _context.ProductCategories
            .Include(c => c.ProductParentCategory)
            .ThenInclude(c => c.ProductParentCategory)
            .ThenInclude(c => c.ProductParentCategory)
            .ThenInclude(c => c.ProductParentCategory)
            .SingleOrDefaultAsync(c => c.Id == productCategory.Id, cancellationToken);

        if (dbProductCategory == null)
        {
            throw new NotFoundException(nameof(ProductCategory), productCategory.Id);
        }

        if (dbProductCategory.ProductParentCategory == null)
        {
            var link = new Link()
            {
                Title = productCategory.LocalizedName,
                Url = "/shop/category/" + productCategory.Slug + "/products",
                Image = request.MenuImage,
                RelatedProductCategoryId = dbProductCategory.Id,
                IsActive = request.IsActive
            };
            _context.Links.Add(link);
        }
        else if (dbProductCategory.ProductParentCategory.ProductParentCategory?.ProductParentCategory == null)
        {
            var parentLink = await _context.Links
                .SingleOrDefaultAsync(c => c.RelatedProductCategoryId == dbProductCategory.ProductParentCategory.Id, cancellationToken);

            if (parentLink == null)
            {
                throw new NotFoundException(nameof(Link), dbProductCategory.ProductParentCategory.Id);
            }

            var link = new Link()
            {
                Title = productCategory.LocalizedName,
                Url = "/shop/category/" + productCategory.Slug + "/products",
                RelatedProductCategoryId = dbProductCategory.Id,
                ParentLink = parentLink,
                IsActive = request.IsActive
            };
            _context.Links.Add(link);
        }


        productCategory.MenuImage = request.MenuImage;
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

        await _context.SaveChangesAsync(cancellationToken);

        return productCategory.Id;
    }
}
