using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Material;
public class ProductOptionValueMaterialConfiguration : IEntityTypeConfiguration<ProductOptionValueMaterial>
{
    public void Configure(EntityTypeBuilder<ProductOptionValueMaterial> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        builder.Property(t => t.ProductOptionMaterialId)
            .HasComment("ویژگی جنس محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductOptionMaterial)
            .WithMany(b => b.Values);
    }
}
