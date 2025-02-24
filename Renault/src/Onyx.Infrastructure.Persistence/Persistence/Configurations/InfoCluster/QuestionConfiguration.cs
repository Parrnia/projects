using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(t => t.QuestionText).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.QuestionText)
            .IsRequired()
            .HasComment("متن سوال");
        builder.Property(t => t.AnswerText)
            .IsRequired()
            .HasComment("متن پاسخ");
        builder.Property(t => t.QuestionType)
            .IsRequired()
            .HasComment("موضوع سوال");
    }
}
