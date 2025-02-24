using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<int>
{
    public int Code { get; init; }
    public string? ProductNo { get; init; }
    public string? OldProductNo { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? ProductCatalog { get; init; }
    public double OrderRate { get; init; }
    public decimal? Height { get; init; }
    public decimal? Width { get; init; }
    public decimal? Length { get; init; }
    public decimal? NetWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public decimal? VolumeWeight { get; init; }
    public int? Mileage { get; init; }
    public int? Duration { get; init; }
    public int? ProviderId { get; init; }
    public int? CountryId { get; init; }
    public int? ProductTypeId { get; init; }
    public int? ProductStatusId { get; init; }
    public int? MainCountingUnitId { get; init; }
    public int? CommonCountingUnitId { get; init; }
    public int ProductBrandId { get; init; }
    public int ProductCategoryId { get; init; }
    public int ProductAttributeTypeId { get; init; }
    public int? ProductOptionColorId { get; init; }
    public int? ProductOptionMaterialId { get; init; }
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Sku { get; set; }
    public int Compatibility { get; set; }
    public bool IsActive { get; init; }
    //public IList<int> KindIds { get; set; } = new List<int>();
    //public IList<int> TagIds { get; set; } = new List<int>();

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //var kinds = new List<Kind>();
            //foreach (var kindId in request.KindIds)
            //{
            //    var kind = await _context.Kinds.FindAsync(kindId, cancellationToken) ?? throw new NotFoundException(nameof(Kind), kindId);
            //    kinds.Add(kind);
            //}
            //var tags = new List<Tag>();
            //foreach (var tagId in request.TagIds)
            //{
            //    var tag = await _context.Tags.FindAsync(tagId, cancellationToken) ?? throw new NotFoundException(nameof(Tag), tagId);
            //    tags.Add(tag);
            //}
            var entity = new Product()
            {
                Code = request.Code,
                ProductNo = request.ProductNo,
                OldProductNo = request.OldProductNo,
                LocalizedName = request.LocalizedName,
                Name = request.Name,
                ProductCatalog = request.ProductCatalog,
                OrderRate = request.OrderRate,
                Height = request.Height,
                Width = request.Width,
                Length = request.Length,
                NetWeight = request.NetWeight,
                GrossWeight = request.GrossWeight,
                VolumeWeight = request.VolumeWeight,
                Mileage = request.Mileage,
                Duration = request.Duration,
                ProviderId = request.ProviderId,
                CountryId = request.CountryId,
                ProductTypeId = request.ProductTypeId,
                ProductStatusId = request.ProductStatusId,
                MainCountingUnitId = request.MainCountingUnitId,
                CommonCountingUnitId = request.CommonCountingUnitId,
                ProductBrandId = request.ProductBrandId,
                ProductCategoryId = request.ProductCategoryId,
                Excerpt = request.Excerpt,
                Description = request.Description,
                Slug = request.LocalizedName.ToLower().Replace(' ', '-'),
                Sku = request.Sku,
                Compatibility = (CompatibilityEnum)request.Compatibility,
                IsActive = request.IsActive
                //Kinds = kinds,
                //Tags = tags,
            };


            var dbProductAttributeType =
                await _context.ProductAttributeTypes
                    .Include(c => c.AttributeGroups)
                    .ThenInclude(c => c.Attributes)
                    .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeTypeId,
                        cancellationToken) ?? throw new NotFoundException(nameof(ProductAttributeType),
                    request.ProductAttributeTypeId);
            ;

            entity.SetProductAttributeType(dbProductAttributeType);

            if (request.ProductOptionColorId != null)
            {
                var colorOption =
                    await _context.ProductOptionColors.SingleOrDefaultAsync(e => e.Id == request.ProductOptionColorId,
                        cancellationToken) ??
                    throw new NotFoundException(nameof(ProductOptionColor), request.ProductOptionColorId);
                var attributeGroup = await _context.ProductTypeAttributeGroups
                    .SingleOrDefaultAsync(c => c.Name == colorOption.Name, cancellationToken);
                if (attributeGroup == null)
                {
                    throw new NotFoundException(nameof(ProductTypeAttributeGroup), colorOption.Name);
                }

                entity.SetColorOption(colorOption, attributeGroup);
            }


            if (request.ProductOptionMaterialId != null)
            {
                var materialOption =
                    await _context.ProductOptionMaterials.SingleOrDefaultAsync(
                        e => e.Id == request.ProductOptionMaterialId, cancellationToken) ??
                    throw new NotFoundException(nameof(ProductOptionMaterial), request.ProductOptionMaterialId);
                var attributeGroup = await _context.ProductTypeAttributeGroups
                    .SingleOrDefaultAsync(c => c.Name == materialOption.Name, cancellationToken);
                if (attributeGroup == null)
                {
                    throw new NotFoundException(nameof(ProductTypeAttributeGroup), materialOption.Name);
                }

                entity.SetMaterialOption(materialOption, attributeGroup);
            }

            var displayName = new ProductDisplayVariant
            {
                Name = entity.LocalizedName,
                IsActive = entity.IsActive
            };
            entity.ProductDisplayVariants.Add(displayName);


            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
