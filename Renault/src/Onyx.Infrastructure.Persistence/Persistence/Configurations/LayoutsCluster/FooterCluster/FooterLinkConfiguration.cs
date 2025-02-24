using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster.FooterCluster;
public class FooterLinkConfiguration : IEntityTypeConfiguration<FooterLink>
{
    public void Configure(EntityTypeBuilder<FooterLink> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Url)
            .HasComment("آدرس url")
            .IsRequired();

        builder.HasOne(c => c.FooterLinkContainer)
            .WithMany(c => c.Links);
    }
}
