using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BlogsCluster;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasIndex(t => t.Title).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Body)
            .HasComment("متن")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(t => t.Image)
            .HasComment("تصویر");
        builder.Property(t => t.Date)
            .HasComment("تاریخ انتشار")
            .IsRequired();
        builder.Property(t => t.BlogCategoryId)
            .HasComment("زیر دسته بندی بلاگ")
            .IsRequired();
        builder.Property(t => t.AuthorId)
            .HasComment("مولف پست")
            .IsRequired();


        builder.HasOne(b => b.BlogCategory)
            .WithMany(f => f.Posts);
        builder.HasOne(b => b.Author)
            .WithMany(f => f.Posts);
        builder.HasMany(b => b.Comments)
            .WithOne(f => f.Post);
    }
}
