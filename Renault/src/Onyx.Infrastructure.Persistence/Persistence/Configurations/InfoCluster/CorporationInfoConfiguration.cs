using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Infrastructure.Persistence.ValueComparers;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;

public class CorporationInfoConfiguration : IEntityTypeConfiguration<CorporationInfo>
{
    public void Configure(EntityTypeBuilder<CorporationInfo> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ContactUsMessage)
            .IsRequired()
            .HasComment("پیام ارتباط با ما");
        builder.Property(t => t.PoweredBy)
            .IsRequired()
            .HasComment("حامی");
        builder.Property(t => t.CallUs)
            .IsRequired()
            .HasComment("ارتباط با ما");
        builder.Property(t => t.DesktopLogo)
            .IsRequired()
            .HasComment("لوکوی دسکتاپ");
        builder.Property(t => t.MobileLogo)
            .IsRequired()
            .HasComment("لوگوی تلفن همراه");
        builder.Property(t => t.FooterLogo)
            .IsRequired()
            .HasComment("لوگو فوتر");
        builder.Property(t => t.SliderBackGroundImage)
            .IsRequired()
            .HasComment("تصویر پس زمینه اسلایدر");
        builder.Property(t => t.IsDefault)
            .HasComment("آیا پیش فرض است؟")
            .IsRequired();

        builder.Property(t => t.PhoneNumbers)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>(),
                StringListComparer.Instance
            );
        builder.Property(t => t.EmailAddresses)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>(),
                StringListComparer.Instance
            );
        builder.Property(t => t.LocationAddresses)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>(),
                StringListComparer.Instance
            );
        builder.Property(t => t.WorkingHours)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>(),
                StringListComparer.Instance
            );
    }
}
