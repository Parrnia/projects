using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Commands.UpdateProduct;
public record UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; init; }
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
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Include(c => c.Attributes)
            .Include(c => c.ProductAttributeType)
            .ThenInclude(c => c.AttributeGroups)
            .ThenInclude(c => c.Attributes)
            .Include(c => c.ColorOption)
            .Include(c => c.MaterialOption)
            .Include(c => c.ProductDisplayVariants)
            .SingleOrDefaultAsync(c => c.Id == request.Id , cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

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
        

        entity.Code = request.Code;
        entity.ProductNo = request.ProductNo;
        entity.OldProductNo = request.OldProductNo;
        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.ProductCatalog = request.ProductCatalog;
        entity.OrderRate = request.OrderRate;
        entity.Height = request.Height;
        entity.Width = request.Width;
        entity.NetWeight = request.NetWeight;
        entity.GrossWeight = request.GrossWeight;
        entity.VolumeWeight = request.VolumeWeight;
        entity.Mileage = request.Mileage;
        entity.Duration = request.Duration;
        entity.ProviderId = request.ProviderId;
        entity.CountryId = request.CountryId;
        entity.ProductTypeId = request.ProductTypeId;
        entity.ProductStatusId = request.ProductStatusId;
        entity.MainCountingUnitId = request.MainCountingUnitId;
        entity.CommonCountingUnitId = request.CommonCountingUnitId;
        entity.ProductBrandId = request.ProductBrandId;
        entity.ProductCategoryId = request.ProductCategoryId;
        entity.Excerpt = request.Excerpt;
        entity.Description = request.Description;
        entity.Slug = request.LocalizedName.ToLower().Replace(' ', '-');
        entity.Sku = request.Sku;
        entity.Compatibility = (CompatibilityEnum)request.Compatibility;
        entity.IsActive = request.IsActive;
        //entity.Kinds = kinds;
        //entity.Tags = tags;

        var dbProductAttributeType =
            await _context.ProductAttributeTypes
                .Include(c => c.AttributeGroups)
                .ThenInclude(c => c.Attributes)
                .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeTypeId,
                    cancellationToken) ?? throw new NotFoundException(nameof(ProductAttributeType), request.ProductAttributeTypeId);
        entity.SetProductAttributeType(dbProductAttributeType);




        if (request.ProductOptionColorId != null)
        {
            var colorOption = await _context.ProductOptionColors.SingleOrDefaultAsync(e => e.Id == request.ProductOptionColorId, cancellationToken) ?? throw new NotFoundException(nameof(ProductOptionColor), request.ProductOptionColorId);
            var attributeGroup = await _context.ProductTypeAttributeGroups
                .SingleOrDefaultAsync(c => c.Name == colorOption.Name, cancellationToken);
            if (attributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
            }
            entity.SetColorOption(colorOption, attributeGroup);
        }
        else
        {
            entity.SetColorOption(null,null);
        }

        if (request.ProductOptionMaterialId != null)
        {
            var materialOption = await _context.ProductOptionMaterials.SingleOrDefaultAsync(e => e.Id == request.ProductOptionMaterialId, cancellationToken) ?? throw new NotFoundException(nameof(ProductOptionMaterial), request.ProductOptionMaterialId);
            var attributeGroup = await _context.ProductTypeAttributeGroups
                .SingleOrDefaultAsync(c => c.Name == materialOption.Name, cancellationToken);
            if (attributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
            }
            entity.SetMaterialOption(materialOption, attributeGroup);
        }

        var displayName = entity.ProductDisplayVariants.SingleOrDefault(c => c.Name == entity.LocalizedName);
        if (displayName != null)
        {
            displayName.Name = entity.LocalizedName;
        }

        if (entity.IsActive == false)
        {
            entity.ProductDisplayVariants.ForEach(c => c.IsActive = false);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}