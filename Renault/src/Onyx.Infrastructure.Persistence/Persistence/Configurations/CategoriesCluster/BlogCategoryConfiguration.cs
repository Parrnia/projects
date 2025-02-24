using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.CategoriesCluster;
public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
{
    public void Configure(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .HasMaxLength(100);
        builder.Property(t => t.Image)
            .HasComment("تصویر")
            .IsRequired();



        builder.HasOne(p => p.BlogParentCategory)
            .WithMany(x => x.BlogChildrenCategories)
            .HasForeignKey(x => x.BlogParentCategoryId).OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        builder.HasMany(p => p.BlogChildrenCategories)
            .WithOne(x => x.BlogParentCategory).OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        builder.HasMany(p => p.Posts)
            .WithOne(c => c.BlogCategory);
        builder.HasMany(p => p.BlogCategoryCustomFields)
            .WithOne(p => p.BlogCategory);
    }
}
