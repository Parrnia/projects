using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;

public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
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
        builder.Property(t => t.Rating)
            .IsRequired()
            .HasComment("امتیاز");
        builder.Property(t => t.Review)
            .IsRequired()
            .HasComment("دیدگاه");

        builder.HasOne(f => f.AboutUs)
            .WithMany(m => m.Testimonials);
    }
}
