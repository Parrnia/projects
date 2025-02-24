using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BlogsCluster;
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Text)
            .HasMaxLength(80)
            .HasComment("متن")
            .IsRequired();
        builder.Property(t => t.Date)
            .HasComment("تاریخ ثبت")
            .IsRequired();
        //builder.Property(t => t.Children)
        //    .HasComment("پاسخ ها");
        builder.Property(t => t.AuthorId)
            .HasComment("نظر دهنده")
            .IsRequired();
        builder.Property(t => t.PostId)
            .HasComment("پست مربوط به نظر")
            .IsRequired();
        //builder.Property(t => t.ParentCommentId)
        //    .HasComment("نظر مادر");


        builder.HasOne(b => b.Author)
            .WithMany(f => f.Comments)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(b => b.Post)
            .WithMany(f => f.Comments);
        //builder.HasOne(c => c.ParentComment)
        //    .WithMany()
        //    .HasForeignKey(c => c.ParentCommentId);
        //builder.HasMany(c => c.Children).WithOne()
        //    .HasForeignKey(c => c.ParentCommentId);
    }
}
