using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Material;
public class ProductOptionMaterialConfiguration : IEntityTypeConfiguration<ProductOptionMaterial>
{
    public void Configure(EntityTypeBuilder<ProductOptionMaterial> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        builder.Property(t => t.Type)
            .HasComment("نوع گزینه محصول")
            .IsRequired();


        builder.HasMany(x => x.Values)
            .WithOne(x => x.ProductOptionMaterial);
        builder.HasMany(x => x.ProductOptionMaterialCustomFields)
            .WithOne(x => x.ProductOptionMaterial);
        builder.HasMany(b => b.Products)
            .WithOne(b => b.MaterialOption);
    }
}
