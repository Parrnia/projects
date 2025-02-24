using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasIndex(t => t.EnTitle).IsUnique();
        builder.HasIndex(t => t.FaTitle).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.EnTitle)
            .HasComment("عنوان انگلیسی")
            .IsRequired();
        builder.Property(t => t.FaTitle)
            .HasComment("عنوان فارسی")
            .IsRequired();
            

        builder.HasMany(b => b.Products)
            .WithMany(f => f.Tags);
    }
}
