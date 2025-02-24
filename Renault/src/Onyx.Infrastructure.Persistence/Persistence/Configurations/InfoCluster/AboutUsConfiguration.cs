using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;

public class AboutUsConfiguration : IEntityTypeConfiguration<AboutUs>
{
    public void Configure(EntityTypeBuilder<AboutUs> builder)
    {
        builder.HasIndex(t => t.Title).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.TextContent)
            .HasComment("محتوای متنی")
            .IsRequired();
        builder.Property(t => t.ImageContent)
            .HasComment("تصویر")
            .IsRequired();
        builder.Property(t => t.IsDefault)
            .HasComment("آیا پیش فرض است؟")
            .IsRequired();


        builder.HasMany(f => f.TeamMembers)
            .WithOne(m => m.AboutUs);
        builder.HasMany(f => f.Testimonials)
            .WithOne(m => m.AboutUs);
    }
}

