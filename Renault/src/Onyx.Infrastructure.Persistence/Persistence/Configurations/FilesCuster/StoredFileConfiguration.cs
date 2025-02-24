using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Onyx.Domain.Entities.FilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.FilesCuster;
internal class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.HasIndex(i => i.FileId);
        builder.Ignore(i => i.FileName);

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FileId)
            .HasComment("شناسه قایل")
            .IsRequired();
        builder.Property(t => t.Extension)
            .HasComment("پسوند فایل")
            .IsRequired();
        builder.Property(t => t.FileSize)
            .HasComment("اندازه فایل")
            .IsRequired();
        builder.Property(t => t.Folder)
            .HasComment("مسیر پوشه فایل")
            .IsRequired();
        builder.Property(t => t.Category)
            .HasComment("دسته بندی فایل")
            .IsRequired();
        builder.Property(t => t.UploadName)
            .HasComment("نام اصلی فایل آپلود شده")
            .IsRequired();
        builder.Property(t => t.UploadDate)
            .HasComment("زمان آپلود")
            .IsRequired();
        builder.Property(t => t.Owner)
            .HasComment("آپلود کننده فایل");
    }
}
