using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Color;
public class ProductOptionColorConfiguration : IEntityTypeConfiguration<ProductOptionColor>
{
    public void Configure(EntityTypeBuilder<ProductOptionColor> builder)
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
            .WithOne(x => x.ProductOptionColor);
        builder.HasMany(x => x.ProductOptionColorCustomFields)
            .WithOne(x => x.ProductOptionColor);
        builder.HasMany(b => b.Products)
            .WithOne(b => b.ColorOption);
    }
}
