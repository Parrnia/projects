using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.RequestsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.RequestsCluster;
public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ApiAddress)
            .IsRequired()
            .HasComment("آدرس api");
        builder.Property(t => t.RequestBody)
            .HasComment("بدنه درخواست");
        builder.Property(t => t.ErrorMessage)
            .HasComment("پیام خطا");
        builder.Property(t => t.ResponseStatus)
            .IsRequired()
            .HasComment("نتیجه درخواست");
        builder.Property(t => t.Created)
            .IsRequired()
            .HasComment("زمان زدن درخواست");
        builder.Property(t => t.RequestType)
            .IsRequired()
            .HasComment("نوع درخواست");
        builder.Property(t => t.ApiType)
            .IsRequired()
            .HasComment("نوع api");
    }
}
