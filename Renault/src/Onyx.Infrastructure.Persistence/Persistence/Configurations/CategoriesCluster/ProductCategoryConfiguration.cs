using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.CategoriesCluster;
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftProductCategoryId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.Code)
            .HasComment("فیلد شمارنده")
            .IsRequired();
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.ProductCategoryNo)
            .HasComment("شماره دسته کالا")
            .HasMaxLength(70);
        builder.Property(t => t.IsPopular)
            .HasComment("محبوب")
            .IsRequired();
        builder.Property(t => t.IsFeatured)
            .HasComment("ویژه")
            .IsRequired();


        builder.HasOne(p => p.ProductParentCategory)
            .WithMany(x => x.ProductChildrenCategories)
            .HasForeignKey(x => x.ProductParentCategoryId).OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        builder.HasMany(p => p.ProductChildrenCategories)
            .WithOne(x => x.ProductParentCategory).OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        builder.HasMany(p => p.Products)
            .WithOne(c => c.ProductCategory);
        builder.HasMany(p => p.ProductCategoryCustomFields)
            .WithOne(p => p.ProductCategory);


    }
}
