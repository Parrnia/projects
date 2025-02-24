using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BlogsCluster;
public class WidgetCommentConfiguration : IEntityTypeConfiguration<WidgetComment>
{
    public void Configure(EntityTypeBuilder<WidgetComment> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.PostTitle)
            .HasComment("پاسخ ها");
        builder.Property(t => t.Text)
            .HasMaxLength(80)
            .HasComment("متن");
        builder.Property(t => t.Date)
            .HasComment("تاریخ ثبت")
            .IsRequired();
        builder.Property(t => t.AuthorId)
            .HasComment("نظر دهنده")
            .IsRequired();


        builder.HasOne(b => b.Author)
            .WithMany(f => f.WidgetComments);
    }
}
