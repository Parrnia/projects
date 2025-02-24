using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster.FooterCluster;
public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Icon)
            .HasComment("آیکون")
            .IsRequired();
        builder.Property(t => t.Url)
            .HasComment("آدرس url")
            .IsRequired();
    }
}
