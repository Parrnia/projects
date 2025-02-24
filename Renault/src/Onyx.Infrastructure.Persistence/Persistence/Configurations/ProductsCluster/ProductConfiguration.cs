using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftProductId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.Code)
            .HasComment("کد شمارنده")
            .IsRequired();
        builder.Property(t => t.ProductNo)
            .HasComment("کد کالا")
            .HasMaxLength(70);
        builder.Property(t => t.OldProductNo)
            .HasComment("کد کالا قبلی")
            .HasMaxLength(70);
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی قطعه")
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین قطعه")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.ProductCatalog)
            .HasComment("کد شناسایی کالا در کاتالوگ");
        builder.Property(t => t.OrderRate)
            .HasComment("ضریب سفارش دهی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.Height)
            .HasComment("ارتفاع کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.Width)
            .HasComment("عرض کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.Length)
            .HasComment("طول کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.NetWeight)
            .HasComment("وزن خالص کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.GrossWeight)
            .HasComment("وزن ناخالص کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.VolumeWeight)
            .HasComment("وزن حجمی کالا")
            .HasColumnType("decimal(12,3)");
        builder.Property(t => t.Mileage)
            .HasComment("کیلومتر گارانتی");
        builder.Property(t => t.Duration)
            .HasComment("تعداد ماه گارانتی");


        builder.Property(t => t.ProviderId)
            .HasComment("کد تامین کننده");
        builder.Property(t => t.CountryId)
            .HasComment("کد کشور");
        builder.Property(t => t.ProductTypeId)
            .HasComment("کد نوع کالا");
        builder.Property(t => t.ProductStatusId)
            .HasComment("کد وضعیت کالا");
        builder.Property(t => t.MainCountingUnitId)
            .HasComment("واحد شمارش اصلی");
        builder.Property(t => t.CommonCountingUnitId)
            .HasComment("واحد شمارش رایج");
        builder.Property(t => t.ProductBrandId)
            .HasComment("برند")
            .IsRequired();
        builder.Property(t => t.ProductCategoryId)
            .HasComment("زیردسته کالا")
            .IsRequired();



        builder.Property(t => t.Excerpt)
            .HasComment("گزیده")
            .IsRequired();
        builder.Property(t => t.Description)
            .HasComment("توضیحات")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        builder.Property(t => t.Sku)
            .HasComment("واحد نگهداری موجودی");
        builder.Property(t => t.Compatibility)
            .HasComment("سازگاری محصول")
            .IsRequired();
        builder.Property(t => t.ProductAttributeTypeId)
            .HasComment("نوع ویژگی محصول")
            .IsRequired();
        

        builder.HasOne(p => p.Provider)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.Country)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.ProductType)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.ProductStatus)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.MainCountingUnit)
            .WithMany(p => p.ProductsForMainCountingUnit);
        builder.HasOne(p => p.CommonCountingUnit)
            .WithMany(p => p.ProductsForCommonCountingUnit);
        builder.HasOne(p => p.ProductBrand)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.ProductCategory)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.ProductAttributeType)
            .WithMany(p => p.Products);
        builder.HasOne(p => p.ColorOption)
            .WithMany(r => r.Products);
        builder.HasOne(p => p.MaterialOption)
            .WithMany(r => r.Products);
        


        builder.HasMany(p => p.AttributeOptions)
            .WithOne(r => r.Product);
        builder.HasMany(p => p.Images)
            .WithOne(r => r.Product);
        builder.HasMany(p => p.Attributes)
            .WithOne(r => r.Product);
        builder.HasMany(p => p.ProductCustomFields)
            .WithOne(r => r.Product);
        builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Product);
        builder.HasMany(p => p.ProductDisplayVariants)
            .WithOne(r => r.Product);



        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products);
        builder.HasMany(p => p.Kinds)
            .WithMany(p => p.Products)
            .UsingEntity<ProductKind>();
    }
}
