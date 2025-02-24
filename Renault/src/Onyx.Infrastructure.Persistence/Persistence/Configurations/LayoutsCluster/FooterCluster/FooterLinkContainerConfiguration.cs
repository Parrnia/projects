using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster.FooterCluster;
public class FooterLinkContainerConfiguration : IEntityTypeConfiguration<FooterLinkContainer>
{
    public void Configure(EntityTypeBuilder<FooterLinkContainer> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Header)
            .HasComment("هدر")
            .IsRequired();
        builder.Property(t => t.Order)
            .HasComment("ترتیب")
            .IsRequired();

        builder.HasMany(c => c.Links)
            .WithOne(c => c.FooterLinkContainer);
    }
}
