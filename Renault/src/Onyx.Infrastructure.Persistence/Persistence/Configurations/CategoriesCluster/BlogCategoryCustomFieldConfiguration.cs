using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.CategoriesCluster;
public class BlogCategoryCustomFieldConfiguration : IEntityTypeConfiguration<BlogCategoryCustomField>
{
    public void Configure(EntityTypeBuilder<BlogCategoryCustomField> builder)
    {
        builder.HasIndex(t => t.FieldName).IsUnique();
        

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.BlogCategoryId)
            .HasComment("دسته بندی بلاگ")
            .IsRequired();

        builder.HasOne(p => p.BlogCategory)
            .WithMany(p => p.BlogCategoryCustomFields);
    }
}