using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Date)
            .HasComment("تاریخ ثبت")
            .IsRequired();
        builder.Property(t => t.Rating)
            .HasComment("امتیازدهی")
            .IsRequired();
        builder.Property(t => t.Content)
            .HasComment("محتوا")
            .IsRequired();
        builder.Property(t => t.ProductId)
            .HasComment("محصول مرتبط")
            .IsRequired();
        builder.Property(t => t.CustomerId)
            .HasComment("نویسنده نظر")
            .IsRequired();
        builder.Property(t => t.AuthorName)
            .HasComment("نام مولف")
            .IsRequired();

        builder.HasOne(b => b.Product)
            .WithMany(b => b.Reviews);
        builder.HasOne(b => b.Customer)
            .WithMany(f => f.Reviews);
    }
}
