using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.UserProfilesCluster;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(t => t.Avatar)
            .HasComment("آواتار");


        builder.HasMany(b => b.Posts)
            .WithOne(f => f.Author);
    }
}
