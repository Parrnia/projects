using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyx.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsEnumerable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان"),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "محتوای متنی"),
                    ImageContent = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, comment: "آیا پیش فرض است؟"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsEnumerable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "مقدار"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockBanners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان"),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ دکمه اولیه"),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "متن کلید"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر"),
                    BlockBannerPosition = table.Column<int>(type: "int", nullable: false, comment: "موقعیت روی صفحه"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockBanners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام لاتین"),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "عنوان کوتاه"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر"),
                    CategoryType = table.Column<int>(type: "int", nullable: false),
                    BlogParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogCategories_BlogCategories_BlogParentCategoryId",
                        column: x => x.BlogParentCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carousels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "آدرس url"),
                    DesktopImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر دسکتاپ"),
                    MobileImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر موبایل"),
                    Offer = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "تخفیف"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان"),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "جزئیات"),
                    ButtonLabel = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "برچسب دکمه"),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "ترتیب"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carousels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorporationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactUsMessage = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "پیام ارتباط با ما"),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoweredBy = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "حامی"),
                    CallUs = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "ارتباط با ما"),
                    DesktopLogo = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "لوکوی دسکتاپ"),
                    MobileLogo = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "لوگوی تلفن همراه"),
                    FooterLogo = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "لوگو فوتر"),
                    SliderBackGroundImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر پس زمینه اسلایدر"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, comment: "آیا پیش فرض است؟"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "مقدار"),
                    Text = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "متن"),
                    CostTypeEnum = table.Column<int>(type: "int", nullable: false, comment: "نوع هزینه"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountingUnitTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftCountingUnitTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountingUnitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Avatar = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeEnum = table.Column<int>(type: "int", nullable: false),
                    DiscountPercent = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false, comment: "درصد تخفیف"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterLinkContainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "هدر"),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "ترتیب"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLinkContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "آدرس url"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    ParentLinkId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_Links_ParentLinkId",
                        column: x => x.ParentLinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    LocalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام لاتین"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoryNo = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, comment: "شماره دسته کالا"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MenuImage = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryType = table.Column<int>(type: "int", nullable: false),
                    ProductParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductCategories_ProductParentCategoryId",
                        column: x => x.ProductParentCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "نوع گزینه محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "نوع گزینه محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProductStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypeAttributeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeAttributeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProductTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    LocalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام لاتین"),
                    LocalizedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "شمارنده داخلی"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "توضیحات"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "متن سوال"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "متن پاسخ"),
                    QuestionType = table.Column<int>(type: "int", nullable: false, comment: "موضوع سوال"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiAddress = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "آدرس api"),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "بدنه درخواست"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "پیام خطا"),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false, comment: "نتیجه درخواست"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "زمان زدن درخواست"),
                    RequestType = table.Column<int>(type: "int", nullable: false, comment: "نوع درخواست"),
                    ApiType = table.Column<int>(type: "int", nullable: false, comment: "نوع api"),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "آدرس url"),
                    Icon = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "آیکون"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "شناسه قایل"),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "پسوند فایل"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false, comment: "اندازه فایل"),
                    Folder = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مسیر پوشه فایل"),
                    Category = table.Column<int>(type: "int", nullable: false, comment: "دسته بندی فایل"),
                    UploadName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام اصلی فایل آپلود شده"),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "زمان آپلود"),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "آپلود کننده فایل"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnTitle = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان انگلیسی"),
                    FaTitle = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان فارسی"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان"),
                    BtnPrimaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ دکمه اولیه"),
                    BtnPrimaryHoverColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ هاور دکمه اولیه"),
                    BtnSecondaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ دکمه ثانویه"),
                    BtnSecondaryHoverColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ هاور دکمه ثانویه"),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ قالب"),
                    HeaderAndFooterColor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ هدر و فوتر"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Avatar = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "آواتار")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "نام"),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "سمت"),
                    Avatar = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر پروفایل"),
                    AboutUsId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_AboutUsEnumerable_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUsEnumerable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "نام"),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "سمت"),
                    Avatar = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر پروفایل"),
                    Rating = table.Column<int>(type: "int", nullable: false, comment: "امتیاز"),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "دیدگاه"),
                    AboutUsId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_AboutUsEnumerable_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUsEnumerable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategoryCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "مقدار فیلد"),
                    BlogCategoryId = table.Column<int>(type: "int", nullable: false, comment: "دسته بندی بلاگ"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategoryCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogCategoryCustomFields_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountingUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftCountingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    IsDecimal = table.Column<bool>(type: "bit", nullable: false, comment: "واحد اعشاری"),
                    CountingUnitTypeId = table.Column<int>(type: "int", nullable: true, comment: "نوع واحد شمارنده"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountingUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountingUnits_CountingUnitTypes_CountingUnitTypeId",
                        column: x => x.CountingUnitTypeId,
                        principalTable: "CountingUnitTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    BrandLogo = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "تصویر لوگو"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "عنوان کوتاه"),
                    CountryId = table.Column<int>(type: "int", nullable: true, comment: "کشور"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBrands_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    BrandLogo = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "تصویر لوگو"),
                    LocalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "فیلد شمارنده"),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "عنوان کوتاه"),
                    CountryId = table.Column<int>(type: "int", nullable: true, comment: "کشور"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleBrands_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "عنوان"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "شرکت"),
                    CountryId = table.Column<int>(type: "int", nullable: false, comment: "کشور"),
                    AddressDetails1 = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "جزئیات آدرس"),
                    AddressDetails2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شهر"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "منطقه"),
                    Postcode = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "کد پستی"),
                    Default = table.Column<bool>(type: "bit", nullable: false, comment: "پیش فرض است؟"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "مشتری مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ ثبت"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "مقدار اعتبار"),
                    ModifierUserName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام کاربر تغییردهنده"),
                    ModifierUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "شناسه کاربر تغییردهنده"),
                    OrderToken = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "شماره سفارش مربوطه"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credits_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "موضوع"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "پیام"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ ثبت"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "شناسه مشتری"),
                    CustomerPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شماره تماس مشتری"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام مشتری"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "رمز"),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "شماره"),
                    Quantity = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "تعداد"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "جمع قیمت کل محصولات"),
                    DiscountPercentForRole = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false, comment: "درصد تخفیف محاسبه شده بر اساس نقش"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "مبلغ پرداختی"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "زمان سفارش"),
                    AddressTitle = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان"),
                    AddressCompany = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شرکت"),
                    AddressCountry = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "کشور"),
                    AddressDetails1 = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "جزئیات آدرس1"),
                    AddressDetails2 = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "جزئیات آدرس2"),
                    AddressCity = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شهر"),
                    AddressState = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "منطقه"),
                    AddressPostcode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "کد پستی"),
                    OrderAddress_Id = table.Column<int>(type: "int", nullable: false),
                    OrderAddress_Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderAddress_CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderAddress_LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderAddress_LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderAddress_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    OrderAddress_IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "مشتری سفارش دهنده"),
                    CustomerTypeEnum = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شماره برای ارسال پیامک"),
                    CustomerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام مشتری"),
                    CustomerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام خانوادگی مشتری یا کد اقتصادی"),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "کد ملی"),
                    PersonType = table.Column<int>(type: "int", nullable: false, comment: "نوع هویت مشتری"),
                    TaxPercent = table.Column<double>(type: "float", nullable: false, comment: "درصد مالیات"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WidgetComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostTitle = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "پاسخ ها"),
                    Text = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true, comment: "متن"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ ثبت"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "نظر دهنده"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WidgetComments_Customers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FooterLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "آدرس url"),
                    FooterLinkContainerId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterLinks_FooterLinkContainers_FooterLinkContainerId",
                        column: x => x.FooterLinkContainerId,
                        principalTable: "FooterLinkContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "مقدار فیلد"),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false, comment: "دسته بندی محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryCustomFields_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionColorCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductOptionColorId = table.Column<int>(type: "int", nullable: false, comment: "مقدار ویژگی رنگ محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionColorCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptionColorCustomFields_ProductOptionColors_ProductOptionColorId",
                        column: x => x.ProductOptionColorId,
                        principalTable: "ProductOptionColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValueColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رنگ"),
                    ProductOptionColorId = table.Column<int>(type: "int", nullable: false, comment: "ویژگی رنگ محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValueColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptionValueColors_ProductOptionColors_ProductOptionColorId",
                        column: x => x.ProductOptionColorId,
                        principalTable: "ProductOptionColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionMaterialCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductOptionMaterialId = table.Column<int>(type: "int", nullable: false, comment: "مقدار ویژگی جنس محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionMaterialCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptionMaterialCustomFields_ProductOptionMaterials_ProductOptionMaterialId",
                        column: x => x.ProductOptionMaterialId,
                        principalTable: "ProductOptionMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValueMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    ProductOptionMaterialId = table.Column<int>(type: "int", nullable: false, comment: "ویژگی جنس محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValueMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptionValueMaterials_ProductOptionMaterials_ProductOptionMaterialId",
                        column: x => x.ProductOptionMaterialId,
                        principalTable: "ProductOptionMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeTypeProductTypeAttributeGroup",
                columns: table => new
                {
                    AttributeGroupsId = table.Column<int>(type: "int", nullable: false),
                    ProductAttributeTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeTypeProductTypeAttributeGroup", x => new { x.AttributeGroupsId, x.ProductAttributeTypesId });
                    table.ForeignKey(
                        name: "FK_ProductAttributeTypeProductTypeAttributeGroup_ProductAttributeTypes_ProductAttributeTypesId",
                        column: x => x.ProductAttributeTypesId,
                        principalTable: "ProductAttributeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributeTypeProductTypeAttributeGroup_ProductTypeAttributeGroups_AttributeGroupsId",
                        column: x => x.AttributeGroupsId,
                        principalTable: "ProductTypeAttributeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypeAttributeGroupAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار"),
                    ProductTypeAttributeGroupId = table.Column<int>(type: "int", nullable: false, comment: "گروه بندی نوع ویژگی محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeAttributeGroupAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeAttributeGroupAttributes_ProductTypeAttributeGroups_ProductTypeAttributeGroupId",
                        column: x => x.ProductTypeAttributeGroupId,
                        principalTable: "ProductTypeAttributeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypeAttributeGroupCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductTypeAttributeGroupId = table.Column<int>(type: "int", nullable: false, comment: "گروه نوع ویژگی محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeAttributeGroupCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeAttributeGroupCustomFields_ProductTypeAttributeGroups_ProductTypeAttributeGroupId",
                        column: x => x.ProductTypeAttributeGroupId,
                        principalTable: "ProductTypeAttributeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "عنوان"),
                    Body = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "متن"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "تصویر"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ انتشار"),
                    BlogCategoryId = table.Column<int>(type: "int", nullable: false, comment: "زیر دسته بندی بلاگ"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "مولف پست"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "کد شمارنده"),
                    ProductNo = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, comment: "کد کالا"),
                    OldProductNo = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, comment: "کد کالا قبلی"),
                    LocalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام فارسی قطعه"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام لاتین قطعه"),
                    ProductCatalog = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "کد شناسایی کالا در کاتالوگ"),
                    OrderRate = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "ضریب سفارش دهی"),
                    Height = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "ارتفاع کالا"),
                    Width = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "عرض کالا"),
                    Length = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "طول کالا"),
                    NetWeight = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "وزن خالص کالا"),
                    GrossWeight = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "وزن ناخالص کالا"),
                    VolumeWeight = table.Column<decimal>(type: "decimal(12,3)", nullable: true, comment: "وزن حجمی کالا"),
                    Mileage = table.Column<int>(type: "int", nullable: true, comment: "کیلومتر گارانتی"),
                    Duration = table.Column<int>(type: "int", nullable: true, comment: "تعداد ماه گارانتی"),
                    ProviderId = table.Column<int>(type: "int", nullable: true, comment: "کد تامین کننده"),
                    CountryId = table.Column<int>(type: "int", nullable: true, comment: "کد کشور"),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true, comment: "کد نوع کالا"),
                    ProductStatusId = table.Column<int>(type: "int", nullable: true, comment: "کد وضعیت کالا"),
                    MainCountingUnitId = table.Column<int>(type: "int", nullable: true, comment: "واحد شمارش اصلی"),
                    CommonCountingUnitId = table.Column<int>(type: "int", nullable: true, comment: "واحد شمارش رایج"),
                    ProductBrandId = table.Column<int>(type: "int", nullable: false, comment: "برند"),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false, comment: "زیردسته کالا"),
                    Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "گزیده"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "توضیحات"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "واحد نگهداری موجودی"),
                    Compatibility = table.Column<int>(type: "int", nullable: false, comment: "سازگاری محصول"),
                    ProductAttributeTypeId = table.Column<int>(type: "int", nullable: false, comment: "نوع ویژگی محصول"),
                    ColorOptionId = table.Column<int>(type: "int", nullable: true),
                    MaterialOptionId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CountingUnits_CommonCountingUnitId",
                        column: x => x.CommonCountingUnitId,
                        principalTable: "CountingUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_CountingUnits_MainCountingUnitId",
                        column: x => x.MainCountingUnitId,
                        principalTable: "CountingUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductAttributeTypes_ProductAttributeTypeId",
                        column: x => x.ProductAttributeTypeId,
                        principalTable: "ProductAttributeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductBrands_ProductBrandId",
                        column: x => x.ProductBrandId,
                        principalTable: "ProductBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductOptionColors_ColorOptionId",
                        column: x => x.ColorOptionId,
                        principalTable: "ProductOptionColors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductOptionMaterials_MaterialOptionId",
                        column: x => x.MaterialOptionId,
                        principalTable: "ProductOptionMaterials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductStatuses_ProductStatusId",
                        column: x => x.ProductStatusId,
                        principalTable: "ProductStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftFamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "کلید اصلی در دیتابیس قبلی"),
                    LocalizedName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "نام لاتین"),
                    VehicleBrandId = table.Column<int>(type: "int", nullable: false, comment: "برند"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_VehicleBrands_VehicleBrandId",
                        column: x => x.VehicleBrandId,
                        principalTable: "VehicleBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Authority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rrn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: true),
                    PayGateTranId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOrderId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPayment_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStateBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStateBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStateBases_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTotals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "مبلغ"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "سفارش مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTotals_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "رمز مبادله با سون"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "شماره"),
                    Quantity = table.Column<double>(type: "float", nullable: false, comment: "تعداد"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "جمع قیمت کل محصولات"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "مبلغ پرداختی"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "زمان ثبت بازگشت سفارش"),
                    CostRefundType = table.Column<int>(type: "int", nullable: false, comment: "شیوه بازپرداخت"),
                    ReturnOrderTransportationType = table.Column<int>(type: "int", nullable: false, comment: "شیوه بازگشت کالا"),
                    CustomerAccountInfo = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "اطلاعات حساب"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "سفارش مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false, comment: "متن"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ ثبت"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "نظر دهنده"),
                    PostId = table.Column<int>(type: "int", nullable: false, comment: "پست مربوط به نظر"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Customers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCount = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "تعداد"),
                    SafetyStockQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "مقدار ذخیره احتیاطی"),
                    MinStockQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "مقدار حداقل موجودی"),
                    MaxStockQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "مقدار حداکثر موجودی"),
                    MaxSalePriceNonCompanyProductPercent = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: true, comment: "سقف قیمت فروش کالای غیر شرکتی-درصد"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "نام"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه"),
                    Featured = table.Column<bool>(type: "bit", nullable: false, comment: "ویژه"),
                    ValueName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام مقدار"),
                    ValueSlug = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان کوتاه مقدار"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "محصول مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "محصول مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCustomFields_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDisplayVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "نام نمایشی محصول"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "محصول مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDisplayVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDisplayVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "فایل"),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "ترتیب"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "محصول مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => new { x.ProductsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProductTag_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ ثبت"),
                    Rating = table.Column<int>(type: "int", nullable: false, comment: "امتیازدهی"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "محتوا"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام مولف"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "محصول مرتبط"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "نویسنده نظر"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    LocalizedName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "نام لاتین"),
                    FamilyId = table.Column<int>(type: "int", nullable: false, comment: "خانواده"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderStateBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnOrderStatus = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderStateBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderStateBases_ReturnOrders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderTotals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "عنوان"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "مبلغ"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "نوع"),
                    ReturnOrderTotalApplyType = table.Column<int>(type: "int", nullable: false, comment: "الگوی اعمال هزینه"),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: false, comment: "سفارش بازشگت مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderTotals_ReturnOrders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BadgeProductAttributeOption",
                columns: table => new
                {
                    BadgesId = table.Column<int>(type: "int", nullable: false),
                    ProductAttributeOptionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeProductAttributeOption", x => new { x.BadgesId, x.ProductAttributeOptionsId });
                    table.ForeignKey(
                        name: "FK_BadgeProductAttributeOption_Badges_BadgesId",
                        column: x => x.BadgesId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeProductAttributeOption_ProductAttributeOptions_ProductAttributeOptionsId",
                        column: x => x.ProductAttributeOptionsId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "قیمت واحد"),
                    DiscountPercentForProduct = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false, comment: "درصد تخفیف محاسبه شده بر روی کالا"),
                    Quantity = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "تعداد"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "جمع کل قیمت سفارش"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "سفارش مرتبط"),
                    ProductLocalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام فارسی قطعه"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام لاتین قطعه"),
                    ProductNo = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "کد کالا"),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_ProductAttributeOptions_ProductAttributeOptionId",
                        column: x => x.ProductAttributeOptionId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ قیمت"),
                    MainPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "قیمت اصلی"),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_ProductAttributeOptions_ProductAttributeOptionId",
                        column: x => x.ProductAttributeOptionId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeOptionRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumStockToDisplayProductForThisCustomerTypeEnum = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "حداقل موجودی کالا برای نمایش کالا به کاربر"),
                    Availability = table.Column<int>(type: "int", nullable: false, comment: "قابلیت دسترسی"),
                    MainMaxOrderQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "حداکثر مقدار سفارش گذاری اصلی"),
                    CurrentMaxOrderQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "حداکثر مقدار سفارش گذاری کنونی"),
                    MainMinOrderQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "حداقل مقدار سفارش گذاری اصلی"),
                    CurrentMinOrderQty = table.Column<double>(type: "float(18)", precision: 18, scale: 3, nullable: false, comment: "حداقل مقدار سفارش گذاری کنونی"),
                    CustomerTypeEnum = table.Column<int>(type: "int", nullable: false),
                    DiscountPercent = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false, comment: "درصد تخفیف روی کالا"),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptionRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptionRoles_ProductAttributeOptions_ProductAttributeOptionId",
                        column: x => x.ProductAttributeOptionId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeOptionValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام گزینه ساختاری"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار ویژگی محصول"),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptionValues_ProductAttributeOptions_ProductAttributeOptionId",
                        column: x => x.ProductAttributeOptionId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderItemGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "قیمت واحد"),
                    TotalDiscountPercent = table.Column<double>(type: "float", nullable: false, comment: "درصد تخفیف محاسبه شده کل"),
                    ProductLocalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام فارسی قطعه"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام لاتین قطعه"),
                    ProductNo = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "کد کالا"),
                    TotalQuantity = table.Column<double>(type: "float", nullable: false),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderItemGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItemGroups_ProductAttributeOptions_ProductAttributeOptionId",
                        column: x => x.ProductAttributeOptionId,
                        principalTable: "ProductAttributeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItemGroups_ReturnOrders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false, comment: "ویژگی محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeCustomFields_ProductAttributes_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeValueCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار فیلد"),
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false, comment: "مقدار ویژگی محصول"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValueCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValueCustomFields_ProductAttributes_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftKindId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "کلید اصلی در دیتابیس قبلی"),
                    LocalizedName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "نام فارسی"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "نام لاتین"),
                    ModelId = table.Column<int>(type: "int", nullable: false, comment: "مدل"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kinds_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderTotalDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "توضیحات"),
                    ReturnOrderTotalId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderTotalDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderTotalDocuments_ReturnOrderTotals_ReturnOrderTotalId",
                        column: x => x.ReturnOrderTotalId,
                        principalTable: "ReturnOrderTotals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false, comment: "آیتم سفارش مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemProductAttributeOptionValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام گزینه ساختاری"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار ویژگی محصول"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemProductAttributeOptionValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemProductAttributeOptionValue_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderItemGroupProductAttributeOptionValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام گزینه ساختاری"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "مقدار ویژگی محصول"),
                    ReturnOrderItemGroupId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderItemGroupProductAttributeOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItemGroupProductAttributeOptionValues_ReturnOrderItemGroups_ReturnOrderItemGroupId",
                        column: x => x.ReturnOrderItemGroupId,
                        principalTable: "ReturnOrderItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<double>(type: "float", nullable: false, comment: "تعداد"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "جمع کل قیمت سفارش بازگشتی"),
                    ReturnOrderReasonId = table.Column<int>(type: "int", nullable: false, comment: "دلیل بازگشت کالا"),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false, comment: "پذیرفته شده"),
                    ReturnOrderItemGroupId = table.Column<int>(type: "int", nullable: false, comment: "گروه آیتم سفارش مرتبط"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItems_ReturnOrderItemGroups_ReturnOrderItemGroupId",
                        column: x => x.ReturnOrderItemGroupId,
                        principalTable: "ReturnOrderItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Related7SoftProductKindId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Related7SoftProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    KindId = table.Column<int>(type: "int", nullable: false),
                    Related7SoftKindId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKinds_Kinds_KindId",
                        column: x => x.KindId,
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductKinds_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VinNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true, comment: "شماره vin"),
                    KindId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Kinds_KindId",
                        column: x => x.KindId,
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderItemDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "تصویر"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "توضیحات"),
                    ReturnOrderItemId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderItemDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnOrderItemDocuments_ReturnOrderItems_ReturnOrderItemId",
                        column: x => x.ReturnOrderItemId,
                        principalTable: "ReturnOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderReasons",
                columns: table => new
                {
                    ReturnOrderItemId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "توضیحات"),
                    ReturnOrderReasonType = table.Column<int>(type: "int", nullable: false, comment: "نوع کلی"),
                    CustomerType = table.Column<int>(type: "int", nullable: false, comment: "نوع دلیل سمت مشتری"),
                    OrganizationType = table.Column<int>(type: "int", nullable: false, comment: "نوع دلیل سمت سازمان"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderReasons", x => x.ReturnOrderItemId);
                    table.ForeignKey(
                        name: "FK_ReturnOrderReasons_ReturnOrderItems_ReturnOrderItemId",
                        column: x => x.ReturnOrderItemId,
                        principalTable: "ReturnOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsEnumerable_Title",
                table: "AboutUsEnumerable",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Postcode_CustomerId",
                table: "Addresses",
                columns: new[] { "Postcode", "CustomerId" },
                unique: true,
                filter: "CustomerId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Title_CustomerId",
                table: "Addresses",
                columns: new[] { "Title", "CustomerId" },
                unique: true,
                filter: "CustomerId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BadgeProductAttributeOption_ProductAttributeOptionsId",
                table: "BadgeProductAttributeOption",
                column: "ProductAttributeOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_Value",
                table: "Badges",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlockBanners_BlockBannerPosition",
                table: "BlockBanners",
                column: "BlockBannerPosition",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_BlogParentCategoryId",
                table: "BlogCategories",
                column: "BlogParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_LocalizedName",
                table: "BlogCategories",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_Name",
                table: "BlogCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategoryCustomFields_BlogCategoryId",
                table: "BlogCategoryCustomFields",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategoryCustomFields_FieldName",
                table: "BlogCategoryCustomFields",
                column: "FieldName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carousels_Title",
                table: "Carousels",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CostTypes_CostTypeEnum",
                table: "CostTypes",
                column: "CostTypeEnum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostTypes_Text",
                table: "CostTypes",
                column: "Text",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostTypes_Value",
                table: "CostTypes",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountingUnits_CountingUnitTypeId",
                table: "CountingUnits",
                column: "CountingUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CountingUnits_LocalizedName",
                table: "CountingUnits",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountingUnits_Name",
                table: "CountingUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountingUnitTypes_LocalizedName",
                table: "CountingUnitTypes",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountingUnitTypes_Name",
                table: "CountingUnitTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_LocalizedName",
                table: "Countries",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_CustomerId",
                table: "Credits",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_OrderToken",
                table: "Credits",
                column: "OrderToken",
                unique: true,
                filter: "[OrderToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_CustomerId",
                table: "CustomerTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypes_CustomerTypeEnum",
                table: "CustomerTypes",
                column: "CustomerTypeEnum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_LocalizedName",
                table: "Families",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_Name",
                table: "Families",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_VehicleBrandId",
                table: "Families",
                column: "VehicleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_FooterLinks_FooterLinkContainerId",
                table: "FooterLinks",
                column: "FooterLinkContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_LocalizedName",
                table: "Kinds",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_ModelId",
                table: "Kinds",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_Name",
                table: "Kinds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_ParentLinkId",
                table: "Links",
                column: "ParentLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_Title",
                table: "Links",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_FamilyId",
                table: "Models",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_LocalizedName",
                table: "Models",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptions_OrderItemId",
                table: "OrderItemOptions",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemProductAttributeOptionValue_OrderItemId",
                table: "OrderItemProductAttributeOptionValue",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductAttributeOptionId",
                table: "OrderItems",
                column: "ProductAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayment_OrderId",
                table: "OrderPayment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Number",
                table: "Orders",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Token",
                table: "Orders",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateBases_OrderId",
                table: "OrderStateBases",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTotals_OrderId",
                table: "OrderTotals",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogCategoryId",
                table: "Posts",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                table: "Posts",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductAttributeOptionId",
                table: "Prices",
                column: "ProductAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeCustomFields_ProductAttributeId",
                table: "ProductAttributeCustomFields",
                column: "ProductAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptionRoles_ProductAttributeOptionId_CustomerTypeEnum",
                table: "ProductAttributeOptionRoles",
                columns: new[] { "ProductAttributeOptionId", "CustomerTypeEnum" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptions_ProductId",
                table: "ProductAttributeOptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptionValues_ProductAttributeOptionId",
                table: "ProductAttributeOptionValues",
                column: "ProductAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_Name_ProductId",
                table: "ProductAttributes",
                columns: new[] { "Name", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_ProductId",
                table: "ProductAttributes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeTypeProductTypeAttributeGroup_ProductAttributeTypesId",
                table: "ProductAttributeTypeProductTypeAttributeGroup",
                column: "ProductAttributeTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeTypes_Name",
                table: "ProductAttributeTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValueCustomFields_ProductAttributeId",
                table: "ProductAttributeValueCustomFields",
                column: "ProductAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_CountryId",
                table: "ProductBrands",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_LocalizedName",
                table: "ProductBrands",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_Name",
                table: "ProductBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_LocalizedName",
                table: "ProductCategories",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Name",
                table: "ProductCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductParentCategoryId",
                table: "ProductCategories",
                column: "ProductParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryCustomFields_FieldName",
                table: "ProductCategoryCustomFields",
                column: "FieldName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryCustomFields_ProductCategoryId",
                table: "ProductCategoryCustomFields",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomFields_ProductId",
                table: "ProductCustomFields",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDisplayVariants_Name_ProductId",
                table: "ProductDisplayVariants",
                columns: new[] { "Name", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDisplayVariants_ProductId",
                table: "ProductDisplayVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKinds_KindId",
                table: "ProductKinds",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKinds_ProductId",
                table: "ProductKinds",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionColorCustomFields_ProductOptionColorId",
                table: "ProductOptionColorCustomFields",
                column: "ProductOptionColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionMaterialCustomFields_ProductOptionMaterialId",
                table: "ProductOptionMaterialCustomFields",
                column: "ProductOptionMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionValueColors_ProductOptionColorId",
                table: "ProductOptionValueColors",
                column: "ProductOptionColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionValueMaterials_ProductOptionMaterialId",
                table: "ProductOptionValueMaterials",
                column: "ProductOptionMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorOptionId",
                table: "Products",
                column: "ColorOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommonCountingUnitId",
                table: "Products",
                column: "CommonCountingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CountryId",
                table: "Products",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LocalizedName",
                table: "Products",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MainCountingUnitId",
                table: "Products",
                column: "MainCountingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MaterialOptionId",
                table: "Products",
                column: "MaterialOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductAttributeTypeId",
                table: "Products",
                column: "ProductAttributeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductBrandId",
                table: "Products",
                column: "ProductBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductStatusId",
                table: "Products",
                column: "ProductStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProviderId",
                table: "Products",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_LocalizedName",
                table: "ProductStatuses",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_Name",
                table: "ProductStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_TagsId",
                table: "ProductTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeAttributeGroupAttributes_ProductTypeAttributeGroupId",
                table: "ProductTypeAttributeGroupAttributes",
                column: "ProductTypeAttributeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeAttributeGroupCustomFields_ProductTypeAttributeGroupId",
                table: "ProductTypeAttributeGroupCustomFields",
                column: "ProductTypeAttributeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_LocalizedName",
                table: "ProductTypes",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_Name",
                table: "ProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers_LocalizedName",
                table: "Providers",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers_Name",
                table: "Providers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionText",
                table: "Questions",
                column: "QuestionText",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItemDocuments_ReturnOrderItemId",
                table: "ReturnOrderItemDocuments",
                column: "ReturnOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItemGroupProductAttributeOptionValues_ReturnOrderItemGroupId",
                table: "ReturnOrderItemGroupProductAttributeOptionValues",
                column: "ReturnOrderItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItemGroups_ProductAttributeOptionId",
                table: "ReturnOrderItemGroups",
                column: "ProductAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItemGroups_ReturnOrderId",
                table: "ReturnOrderItemGroups",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderItems_ReturnOrderItemGroupId",
                table: "ReturnOrderItems",
                column: "ReturnOrderItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrders_OrderId",
                table: "ReturnOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderStateBases_ReturnOrderId",
                table: "ReturnOrderStateBases",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderTotalDocuments_ReturnOrderTotalId",
                table: "ReturnOrderTotalDocuments",
                column: "ReturnOrderTotalId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderTotals_ReturnOrderId",
                table: "ReturnOrderTotals",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_FileId",
                table: "StoredFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_EnTitle",
                table: "Tags",
                column: "EnTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_FaTitle",
                table: "Tags",
                column: "FaTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_AboutUsId",
                table: "TeamMembers",
                column: "AboutUsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_Name",
                table: "TeamMembers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_AboutUsId",
                table: "Testimonials",
                column: "AboutUsId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_Name",
                table: "Testimonials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Themes_Title",
                table: "Themes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBrands_CountryId",
                table: "VehicleBrands",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBrands_LocalizedName",
                table: "VehicleBrands",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBrands_Name",
                table: "VehicleBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_KindId",
                table: "Vehicles",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_WidgetComments_AuthorId",
                table: "WidgetComments",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BadgeProductAttributeOption");

            migrationBuilder.DropTable(
                name: "BlockBanners");

            migrationBuilder.DropTable(
                name: "BlogCategoryCustomFields");

            migrationBuilder.DropTable(
                name: "Carousels");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CorporationInfos");

            migrationBuilder.DropTable(
                name: "CostTypes");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "CustomerTickets");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "FooterLinks");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "OrderItemOptions");

            migrationBuilder.DropTable(
                name: "OrderItemProductAttributeOptionValue");

            migrationBuilder.DropTable(
                name: "OrderPayment");

            migrationBuilder.DropTable(
                name: "OrderStateBases");

            migrationBuilder.DropTable(
                name: "OrderTotals");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ProductAttributeCustomFields");

            migrationBuilder.DropTable(
                name: "ProductAttributeOptionRoles");

            migrationBuilder.DropTable(
                name: "ProductAttributeOptionValues");

            migrationBuilder.DropTable(
                name: "ProductAttributeTypeProductTypeAttributeGroup");

            migrationBuilder.DropTable(
                name: "ProductAttributeValueCustomFields");

            migrationBuilder.DropTable(
                name: "ProductCategoryCustomFields");

            migrationBuilder.DropTable(
                name: "ProductCustomFields");

            migrationBuilder.DropTable(
                name: "ProductDisplayVariants");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductKinds");

            migrationBuilder.DropTable(
                name: "ProductOptionColorCustomFields");

            migrationBuilder.DropTable(
                name: "ProductOptionMaterialCustomFields");

            migrationBuilder.DropTable(
                name: "ProductOptionValueColors");

            migrationBuilder.DropTable(
                name: "ProductOptionValueMaterials");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "ProductTypeAttributeGroupAttributes");

            migrationBuilder.DropTable(
                name: "ProductTypeAttributeGroupCustomFields");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "ReturnOrderItemDocuments");

            migrationBuilder.DropTable(
                name: "ReturnOrderItemGroupProductAttributeOptionValues");

            migrationBuilder.DropTable(
                name: "ReturnOrderReasons");

            migrationBuilder.DropTable(
                name: "ReturnOrderStateBases");

            migrationBuilder.DropTable(
                name: "ReturnOrderTotalDocuments");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SocialLinks");

            migrationBuilder.DropTable(
                name: "StoredFiles");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "WidgetComments");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "FooterLinkContainers");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ProductTypeAttributeGroups");

            migrationBuilder.DropTable(
                name: "ReturnOrderItems");

            migrationBuilder.DropTable(
                name: "ReturnOrderTotals");

            migrationBuilder.DropTable(
                name: "AboutUsEnumerable");

            migrationBuilder.DropTable(
                name: "Kinds");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ReturnOrderItemGroups");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "ProductAttributeOptions");

            migrationBuilder.DropTable(
                name: "ReturnOrders");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "VehicleBrands");

            migrationBuilder.DropTable(
                name: "CountingUnits");

            migrationBuilder.DropTable(
                name: "ProductAttributeTypes");

            migrationBuilder.DropTable(
                name: "ProductBrands");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductOptionColors");

            migrationBuilder.DropTable(
                name: "ProductOptionMaterials");

            migrationBuilder.DropTable(
                name: "ProductStatuses");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CountingUnitTypes");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
