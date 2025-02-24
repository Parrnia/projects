using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster.HeaderCluster;
public class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.HasIndex(t => t.Title).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Url)
            .HasComment("آدرس url")
            .IsRequired();


        builder.HasMany(b => b.ChildLinks)
            .WithOne(f => f.ParentLink)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(b => b.ParentLink)
            .WithMany(f => f.ChildLinks)
            .HasForeignKey(x => x.ParentLinkId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
