using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .IsRequired()
            .HasComment("نام");
        builder.Property(t => t.Position)
            .IsRequired()
            .HasComment("سمت");
        builder.Property(t => t.Avatar)
            .IsRequired()
            .HasComment("تصویر پروفایل");

        builder.HasOne(f => f.AboutUs)
            .WithMany(m => m.TeamMembers);
    }
}
