using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        #region RenualtPartModel

        #region First Step

        #region CorporationInfo
        if (!_context.CorporationInfos.Any())
        {
            _context.CorporationInfos.Add(new CorporationInfo()
            {
                ContactUsMessage =
                    "رنو در سال ۲۰۰۵ با رانندگی فرناندو آلونسو به قهرمانی مسابقات فرمول یک دست یافت و در سال ۲۰۰۹ شروع به طراحی و ساخت خودروهای الکتریکی کرد. رنو با مشارکت فرانس تلکوم اولین تولیدکننده اتومبیل «ادیس لاین» با سیستم ماهواره‌ای بوده و مدل «ول ساتیس» رنو به عنوان بی صداترین اتومبیل جهان شناخته شده است.",
                PhoneNumbers = new List<string>(){ "021-75043563" } ,
                EmailAddresses = new List<string>(){ "it@neginkhodro.ir" },
                LocationAddresses = new List<string>(){ "تهران، خیابان دماوند (شرق به غرب) نرسیده به اتحاد، جنب خیابان بابائیان"},
                WorkingHours = new List<string>(){"شنبه تا چهارشنبه"},
                PoweredBy = "تمام حقوق اين وب‌سايت متعلق به شرکت رنوپارت است.",
                CallUs = "021-75043563",
                DesktopLogo = Guid.Empty, // "/assets/images/onyxparts.png",
                MobileLogo = Guid.Empty, //"/assets/images/onyxparts.png",
                FooterLogo = Guid.Empty, // "/assets/images/payments.png",
                Created = DateTime.Now,
                CreatedBy = null,
                LastModified = null,
                LastModifiedBy = null
            });
        }
        #endregion
        #region AboutUs
        var aboutUs = new AboutUs()
        {
            TextContent =
                "اونیکس یک شرکت بین المللی با  سابقه در فروش قطعات یدکی خودرو، کامیون و موتور سیکلت است. در طول کار ما موفق شدیم یک سرویس منحصر به فرد برای فروش و تحویل قطعات یدکی در سراسر ایران ایجاد کنیم.",
            ImageContent = Guid.Empty, // "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        if (!_context.AboutUsEnumerable.Any())
        {
            _context.AboutUsEnumerable.Add(aboutUs);
        }
        #endregion
        #region Footer
        //if (!_context.Footers.Any())
        //{
        //    _context.Footers.AddRange(new List<Footer> {
        //        new Footer()
        //    {
        //        ColumnNumber = 1,
        //        Title = "لینک ها",
        //        Description = "sdfsdfsd",
        //        Contents = "<h5 class=\"footer-links__title\">Header</h5>\r\n\r\n<ul class=\"footer-links__list\">\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 1</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 2</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 3</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 4</a>\r\n\r\n    </li>\r\n\r\n</ul>\r\n",
        //        Tags = "sdf",
        //        Alt = "adasf",
        //        Created = DateTime.Now,
        //        CreatedBy = null,
        //        LastModified = null,
        //        LastModifiedBy = null
        //    },
        //        new Footer()
        //        {
        //            ColumnNumber = 1,
        //            Title = "همکاران",
        //            Description = "sdfsdfsd",
        //            Contents = "<h5 class=\"footer-links__title\">Header</h5>\r\n\r\n<ul class=\"footer-links__list\">\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 1</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 2</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 3</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 4</a>\r\n\r\n    </li>\r\n\r\n</ul>\r\n",
        //            Tags = "sdf",
        //            Alt = "adasf",
        //            Created = DateTime.Now,
        //            CreatedBy = null,
        //            LastModified = null,
        //            LastModifiedBy = null
        //        },
        //        new Footer()
        //        {
        //            ColumnNumber = 1,
        //            Title = "پیوندها",
        //            Description = "sdfsdfsd",
        //            Contents = "<h5 class=\"footer-links__title\">Header</h5>\r\n\r\n<ul class=\"footer-links__list\">\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 1</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 2</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 3</a>\r\n\r\n    </li>\r\n\r\n    <li class=\"footer-links__item\">\r\n\r\n        <a [routerLink]=\"url\" class=\"footer-links__link\">title 4</a>\r\n\r\n    </li>\r\n\r\n</ul>\r\n",
        //            Tags = "sdf",
        //            Alt = "adasf",
        //            Created = DateTime.Now,
        //            CreatedBy = null,
        //            LastModified = null,
        //            LastModifiedBy = null
        //        }
        //    });
        //}
        #endregion

        #region Carousel

        var carousel1 = new Carousel()
        {
            Url = "shop",
            DesktopImage = Guid.Empty, // "assets/images/slides/slide-3.jpg",
            MobileImage = Guid.Empty, // "assets/images/slides/slide-3-mobile.jpg",
            Offer = "30 درصد تخفیف",
            Title = "هنگامی که محصولات را <br> به همراه نصب از ما می خرید",
            Details = "نصب قطعات <br> بر عهده ما خواهد بود",
            ButtonLabel = "خرید کنید", // "رفتن به فروشگاه",
            IsActive = true,
            Order = 1
        };
        var carousel2 = new Carousel()
        {
            Url = "shop",
            DesktopImage = Guid.Empty, // "assets/images/slides/slide-2.jpg",
            MobileImage = Guid.Empty, //"assets/images/slides/slide-2-mobile.jpg",
            Title = "قطعه مورد نظر خود را پیدا نمی کنید؟",
            Details = "ما هر آن چه شما بخواهید داریم",
            ButtonLabel = "خرید کنید", // "رفتن به فروشگاه",
            IsActive = true,
            Order = 2
        };
        var carousel3 = new Carousel()
        {
            Url = "shop",
            DesktopImage = Guid.Empty, // "assets/images/slides/slide-1.jpg",
            MobileImage = Guid.Empty, // "assets/images/slides/slide-1-mobile.jpg",
            Offer = "30 درصد تخفیف",
            Title = "دامنه ای متنوع از تایرها",
            Details = "هر سایز و قطری،<br> تابستانی یا زمستانی، باران یا برف",
            ButtonLabel = "خرید کنید", // "رفتن به فروشگاه",
            IsActive = true,
            Order = 3
        };


        if (!_context.Carousels.Any())
        {
            _context.Carousels.AddRange(new List<Carousel>() { carousel1, carousel2, carousel3 });
        }

        #endregion

        #region Country   
        var country1 = new Country()
        {
            Name = "Iran",
            LocalizedName = "ایران",
            Code = 1,
            Related7SoftCountryId = new Guid(),
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var country2 = new Country()
        {
            Name = "Germany",
            LocalizedName = "آلمان",
            Code = 2,
            Related7SoftCountryId = new Guid(),
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var country3 = new Country()
        {
            Name = "France",
            LocalizedName = "فرانسه",
            Code = 3,
            Related7SoftCountryId = new Guid(),
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.Countries.Any())
        {
            _context.Countries.AddRange(new List<Country>() { country1, country2, country3 });
        }
        #endregion
        #region Question   
        var question1 = new Question()
        {
            QuestionText = "1متن سوال",
            AnswerText = "متن پاسخ1",
            QuestionType = QuestionType.ShippingInformation
        };
        var question2 = new Question()
        {
            QuestionText = "2متن سوال",
            AnswerText = "متن پاسخ2",
            QuestionType = QuestionType.ShippingInformation
        };
        var question3 = new Question()
        {
            QuestionText = "3متن سوال",
            AnswerText = "متن پاسخ3",
            QuestionType = QuestionType.ShippingInformation
        };
        var question4 = new Question()
        {
            QuestionText = "4متن سوال",
            AnswerText = "متن پاسخ4",
            QuestionType = QuestionType.ShippingInformation
        };
        var question5 = new Question()
        {
            QuestionText = "5متن سوال",
            AnswerText = "متن پاسخ5",
            QuestionType = QuestionType.PaymentInformation
        };
        var question6 = new Question()
        {
            QuestionText = "6متن سوال",
            AnswerText = "متن پاسخ6",
            QuestionType = QuestionType.PaymentInformation
        };
        var question7 = new Question()
        {
            QuestionText = "7متن سوال",
            AnswerText = "متن پاسخ7",
            QuestionType = QuestionType.OrdersAndReturns
        };
        var question8 = new Question()
        {
            QuestionText = "8متن سوال",
            AnswerText = "متن پاسخ8",
            QuestionType = QuestionType.OrdersAndReturns
        };


        if (!_context.Questions.Any())
        {
            _context.Questions.AddRange(new List<Question>() { question1, question2, question3, question4, question5, question6, question7, question8 });
        }
        #endregion
        #region CountingUnitType
        var countingUnitType1 = new CountingUnitType()
        {
            Code = 1,
            Related7SoftCountingUnitTypeId = new Guid(),
            LocalizedName = "خودرو",
            Name = "Vehicle",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var countingUnitType2 = new CountingUnitType()
        {
            Code = 2,
            Related7SoftCountingUnitTypeId = new Guid(),
            LocalizedName = "کالا",
            Name = "Part",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.CountingUnitTypes.Any())
        {
            _context.CountingUnitTypes.AddRange(new List<CountingUnitType>() { countingUnitType1, countingUnitType2 });
        }
        #endregion
        #region ProductGroup
        var productCategory1 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 1,
            LocalizedName = "قطعات یدکی",
            Name = "Spare parts",
            ProductCategoryNo = "01",
            Image = Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "قطعات یدکی".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory2 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 2,
            LocalizedName = "ابزار مخصوص",
            Name = "Special tools",
            ProductCategoryNo = "03",
            Image =  Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "ابزار مخصوص".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory3 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 5,
            LocalizedName = "باتری",
            Name = "Batteries",
            ProductCategoryNo = "04",
            Image =  Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "باتری".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory4 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 6,
            LocalizedName = "گروه بندی 1",
            Name = "ProductCategory1",
            ProductCategoryNo = "05",
            Image =  Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "گروه بندی 1".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory5 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 7,
            LocalizedName = "گروه بندی 2",
            Name = "ProductCategory2",
            ProductCategoryNo = "06",
            Image =  Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "گروه بندی 2".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory6 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 8,
            LocalizedName = "گروه بندی 3",
            Name = "ProductCategory3",
            ProductCategoryNo = "07",
            Image =  Guid.Empty, // "",
            Created = DateTime.Now,
            Slug = "گروه بندی 3".ToLower().Replace(' ', '-'),
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };


        if (!_context.ProductCategories.Any())
        {
            _context.ProductCategories.AddRange(new List<ProductCategory>() { productCategory1, productCategory2, productCategory3, productCategory4, productCategory5, productCategory6 });
        }
        #endregion
        #region Provider
        var provider1 = new Provider()
        {
            Related7SoftProviderId = new Guid(),
            Code = 1,
            LocalizedName = "تامین کننده آزاد",
            Name = "Free Provider",
            LocalizedCode = "6000",
            Description = null,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var provider2 = new Provider()
        {
            Related7SoftProviderId = new Guid(),
            Code = 2,
            LocalizedName = "دفتر مرکزی",
            Name = "2",
            LocalizedCode = null,
            Description = null,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var provider3 = new Provider()
        {
            Related7SoftProviderId = new Guid(),
            Code = 3,
            LocalizedName = "رنو",
            Name = "Renault SAS",
            LocalizedCode = "100",
            Description = null,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var provider4 = new Provider()
        {
            Related7SoftProviderId = new Guid(),
            Code = 4,
            LocalizedName = "فرش و موکت بابل",
            Name = "4",
            LocalizedCode = "500",
            Description = null,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var provider5 = new Provider()
        {
            Related7SoftProviderId = new Guid(),
            Code = 5,
            LocalizedName = "امید تجارت اناهید",
            Name = "5",
            LocalizedCode = "501",
            Description = null,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.Providers.Any())
        {
            _context.Providers.AddRange(new List<Provider>() { provider1, provider2, provider3, provider4, provider5 });
        }
        #endregion
        #region ProductStatus
        var productStatus1 = new ProductStatus()
        {
            Related7SoftProductStatusId = new Guid(),
            Code = 1,
            LocalizedName = "اصلی",
            Name = "Original",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productStatus2 = new ProductStatus()
        {
            Related7SoftProductStatusId = new Guid(),
            Code = 2,
            LocalizedName = "غیر اصلی",
            Name = "NonOriginal",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.ProductStatuses.Any())
        {
            _context.ProductStatuses.AddRange(new List<ProductStatus>() { productStatus1, productStatus2 });
        }
        #endregion
        #region ProductType
        var productType1 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 1,
            LocalizedName = "قطعات یدکی",
            Name = "SpareParts",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType2 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 2,
            LocalizedName = "سیالات مصرفی",
            Name = "Consumable fluids",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productType3 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 3,
            LocalizedName = "ابزار مخصوص",
            Name = "SpecialTools",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType4 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 4,
            LocalizedName = "باتری",
            Name = "Batteries",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType5 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 5,
            LocalizedName = "اقلام آپشن",
            Name = "Accessories",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType6 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 6,
            LocalizedName = "سایر اقلام",
            Name = "Other items",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType7 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 7,
            LocalizedName = "متفرقه",
            Name = "etc.",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType8 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 8,
            LocalizedName = "خودرو",
            Name = "Vehicle",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productType9 = new ProductType()
        {
            Related7SoftProductTypeId = new Guid(),
            Code = 9,
            LocalizedName = "سبد خاص",
            Name = "-",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.ProductTypes.Any())
        {
            _context.ProductTypes.AddRange(new List<ProductType>()
                { productType1, productType2, productType3, productType4, productType5, productType6, productType7, productType8, productType9 });
        }
        #endregion
        #region User
        var user1 = new User()
        {
            Id = Guid.NewGuid()
        };
        var user2 = new User()
        {
            Id = Guid.NewGuid()
        };
        var user3 = new User()
        {
            Id = Guid.NewGuid()
        };
        var user4 = new User()
        {
            Id = Guid.NewGuid()
        };

        if (!_context.Users.Any())
        {
            _context.Users.AddRange(new List<User>() { user1, user2, user3, user4 });
        }
        #endregion
        #region Customer
        var customer1 = new Customer()
        {
            Id = Guid.NewGuid(),
        };
        customer1.SetCredit(100000, new Guid(), "AdminTest");
        var customer2 = new Customer()
        {
            Id = Guid.NewGuid(),
        };
        customer2.SetCredit(0, new Guid(), "AdminTest");
        var customer3 = new Customer()
        {
            Id = Guid.NewGuid(),
        };
        customer3.SetCredit(0, new Guid(), "AdminTest");
        var customer4 = new Customer()
        {
            Id = Guid.NewGuid(),
        };
        customer4.SetCredit(0, new Guid(), "AdminTest");


        if (!_context.Customers.Any())
        {
            _context.Customers.AddRange(new List<Customer>() { customer1, customer2, customer3, customer4 });
        }
        #endregion
        #region ProductImage

        var productImage1 = new ProductImage() { Image = Guid.Empty, Order = 1 };
        var productImage2 = new ProductImage() { Image =  Guid.Empty, Order = 2 };

        #endregion


        await _context.SaveChangesAsync();
        #endregion

        #region Second Step
        #region CustomerType
        var customerType1 = new Domain.Entities.UserProfilesCluster.CustomerType()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0
        };
        var customerType2 = new Domain.Entities.UserProfilesCluster.CustomerType()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 7
        };
        if (!_context.CustomerTypes.Any())
        {
            _context.CustomerTypes.AddRange(new List<Domain.Entities.UserProfilesCluster.CustomerType>() { customerType1, customerType2 });
        }
        #endregion
        #region CountingUnit
        var countingUnit1 = new CountingUnit()
        {
            Related7SoftCountingUnitId = new Guid(),
            Code = 1,
            LocalizedName = "شاخه",
            CountingUnitTypeId = countingUnitType2.Id,
            Name = "Tributary",
            IsDecimal = false,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var countingUnit2 = new CountingUnit()
        {
            Related7SoftCountingUnitId = new Guid(),
            Code = 13,
            LocalizedName = "دستگاه",
            CountingUnitTypeId = countingUnitType1.Id,
            Name = "Machine",
            IsDecimal = false,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var countingUnit3 = new CountingUnit()
        {
            Related7SoftCountingUnitId = new Guid(),
            Code = 11,
            LocalizedName = "رول",
            CountingUnitTypeId = countingUnitType2.Id,
            Name = "Roll",
            IsDecimal = false,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.CountingUnits.Any())
        {
            _context.CountingUnits.AddRange(new List<CountingUnit>() { countingUnit1, countingUnit2, countingUnit3 });
        }
        #endregion
        #region TeamMember
        var teamMember1 = new TeamMember()
        {
            Name = "میشل روسو",
            Position = "مدیر ارشد اجرایی",
            Avatar = Guid.Empty, //"assets/images/teammates/teammate1.jpg",
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var teamMember2 = new TeamMember()
        {
            Name = "سامانتا اسمیت",
            Position = "مدیر داخلی",
            Avatar = Guid.Empty,// "assets/images/teammates/teammate2.jpg",
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var teamMember3 = new TeamMember()
        {
            Name = "آنتونی هریس",
            Position = "مدیر مالی",
            Avatar = Guid.Empty,// "assets/images/teammates/teammate3.jpg",
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var teamMember4 = new TeamMember()
        {
            Name = "کاترین میلر",
            Position = "کارمند فروش",
            Avatar = Guid.Empty,// "assets/images/teammates/teammate4.jpg",
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var teamMember5 = new TeamMember()
        {
            Name = "بوریس گیلمور",
            Position = "مهندس",
            Avatar = Guid.Empty,// "assets/images/teammates/teammate5.jpg",
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };


        if (!_context.TeamMembers.Any())
        {
            _context.TeamMembers.AddRange(new List<TeamMember>() { teamMember1, teamMember2, teamMember3, teamMember4, teamMember5 });
        }
        #endregion
        #region Testimonial

        var testimonial1 = new Testimonial()
        {
            Name = "جسیکا موره",
            Position = "مدیر شرکت مبلیا",
            Avatar = Guid.Empty,// "assets/images/testimonials/testimonial-1.jpg",
            Review = "این تقسیم بندی منسوخ نشده بلکه تغییر کرده است. فلسفه طبیعی به علوم مختلف طبیعی به ویژه نجوم و کیهان شناسی تقسیم شده است. فلسفه اخلاق، علوم اجتماعی را به وجود آورده است، اما همچنان شامل نظریه ارزش است.",
            Rating = 2,
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var testimonial2 = new Testimonial()
        {
            Name = "پیتر بریدج",
            Position = "راننده ماشین سنگین",
            Avatar = Guid.Empty,// "assets/images/testimonials/testimonial-2.jpg",
            Review = "پرسش‌های فلسفی را می‌توان به دسته‌هایی دسته‌بندی کرد. این گروه بندی ها به فیلسوفان اجازه می دهد. گروه بندی ها همچنین رویکرد فلسفه را برای دانشجویان آسان تر می کند.",
            Rating = 3,
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var testimonial3 = new Testimonial()
        {
            Name = "جف کاوالسی",
            Position = "مدیر استروکا",
            Avatar = Guid.Empty,// "assets/images/testimonials/testimonial-3.jpg",
            Review = "ایده های تصور شده توسط یک جامعه بازتاب عمیقی بر اعمال جامعه دارد. فلسفه کاربردهایی از جمله در اخلاق – به ویژه اخلاق کاربردی – و فلسفه سیاسی دارد.",
            Rating = 4,
            AboutUsId = aboutUs.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };


        if (!_context.Testimonials.Any())
        {
            _context.Testimonials.AddRange(new List<Testimonial>() { testimonial1, testimonial2, testimonial3 });
        }
        #endregion
        #region VehicleBrand
        var vehicleBrand1 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country3.Id,
            Slug = "slug1",
            Code = 1,
            LocalizedName = "سیتروئن",
            Name = "citroen",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/citroen.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand2 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country2.Id,
            Slug = "slug2",
            Code = 2,
            LocalizedName = "جیلی",
            Name = "geely",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/geely.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand3 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug3",
            Code = 3,
            LocalizedName = "هیوندای",
            Name = "hyundai",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/hyundai.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand4 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug4",
            Code = 4,
            LocalizedName = "ایران خودرو",
            Name = "irankhodro",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/irankhodro.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand5 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug5",
            Code = 5,
            LocalizedName = "جک",
            Name = "jak",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/jak.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand6 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug6",
            Code = 6,
            LocalizedName = "کیا",
            Name = "kia",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/kia.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand7 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug7",
            Code = 7,
            LocalizedName = "لکسوس",
            Name = "Lexus",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/lexus-min.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand8 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug8",
            Code = 8,
            LocalizedName = "لیفان",
            Name = "lifan",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/lifan.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand9 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug9",
            Code = 9,
            LocalizedName = "مزدا",
            Name = "mazda",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/mazda.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand10 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug10",
            Code = 10,
            LocalizedName = "نیسان",
            Name = "nissan",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/nissan.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand11 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug11",
            Code = 11,
            LocalizedName = "پژو",
            Name = "peugeot",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/peugeot.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand12 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug12",
            Code = 12,
            LocalizedName = "رنو",
            Name = "renault",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/renault.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand13 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug13",
            Code = 13,
            LocalizedName = "سایپا",
            Name = "saipa",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/saipa.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand14 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug14",
            Code = 14,
            LocalizedName = "سوزوکی",
            Name = "suzuki",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/suzuki.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand15 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug15",
            Code = 15,
            LocalizedName = "تویوتا",
            Name = "toyota",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/toyota.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand16 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug16",
            Code = 16,
            LocalizedName = "ام وی ام",
            Name = "mvm",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/mvm.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        




        var vehicleBrand17 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug17",
            Code = 17,
            LocalizedName = "برند خودرو 17",
            Name = "VehicleBrand17",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand18 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug18",
            Code = 18,
            LocalizedName = "برند خودرو 18",
            Name = "VehicleBrand18",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand19 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug19",
            Code = 19,
            LocalizedName = "برند خودرو 19",
            Name = "VehicleBrand19",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        
        var vehicleBrand20 = new VehicleBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug20",
            Code = 20,
            LocalizedName = "برند خودرو 20",
            Name = "VehicleBrand20",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        


        if (!_context.VehicleBrands.Any())
        {
            _context.VehicleBrands.AddRange(new List<VehicleBrand>()
            {
                vehicleBrand1, vehicleBrand2, vehicleBrand3, vehicleBrand4, vehicleBrand5, vehicleBrand6, vehicleBrand7, vehicleBrand8, vehicleBrand9, vehicleBrand10,
                vehicleBrand11, vehicleBrand12, vehicleBrand13, vehicleBrand14, vehicleBrand15, vehicleBrand16, vehicleBrand17, vehicleBrand18, vehicleBrand19, vehicleBrand20
            });
        }
        #endregion
        #region ProductBrand
        var productBrand1 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country3.Id,
            Slug = "slug1",
            Code = 1,
            LocalizedName = "برند محصول 1",
            Name = "ProductBrand 1",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/citroen.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand2 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country2.Id,
            Slug = "slug2",
            Code = 2,
            LocalizedName = "برند محصول 2",
            Name = "ProductBrand 2",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/geely.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand3 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug3",
            Code = 3,
            LocalizedName = "برند محصول 3",
            Name = "ProductBrand 3",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/hyundai.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand4 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug4",
            Code = 4,
            LocalizedName = "برند محصول 4",
            Name = "ProductBrand 4",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/irankhodro.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand5 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug5",
            Code = 5,
            LocalizedName = "برند محصول 5",
            Name = "ProductBrand 5",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/jak.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand6 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug6",
            Code = 6,
            LocalizedName = "برند محصول 6",
            Name = "ProductBrand 6",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/kia.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand7 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug7",
            Code = 7,
            LocalizedName = "برند محصول 7",
            Name = "ProductBrand 7",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/lexus-min.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand8 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug8",
            Code = 8,
            LocalizedName = "برند محصول 8",
            Name = "ProductBrand 8",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/lifan.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand9 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug9",
            Code = 9,
            LocalizedName = "برند محصول 9",
            Name = "ProductBrand 9",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/mazda.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand10 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug10",
            Code = 10,
            LocalizedName = "برند محصول10",
            Name = "ProductBrand10",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/nissan.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand11 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug11",
            Code = 11,
            LocalizedName = "برند محصول11",
            Name = "ProductBrand11",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/peugeot.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand12 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug12",
            Code = 12,
            LocalizedName = "برند محصول12",
            Name = "ProductBrand12",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/renault.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand13 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug13",
            Code = 13,
            LocalizedName = "برند محصول13",
            Name = "ProductBrand13",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/saipa.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand14 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug14",
            Code = 14,
            LocalizedName = "برند محصول14",
            Name = "ProductBrand14",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/suzuki.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand15 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug15",
            Code = 15,
            LocalizedName = "برند محصول15",
            Name = "ProductBrand15",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/toyota.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand16 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug16",
            Code = 16,
            LocalizedName = "برند محصول16",
            Name = "ProductBrand16",
            BrandLogo = Guid.Empty,// "/assets/images/onyx/brands/mvm.jpg",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };





        var productBrand17 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug17",
            Code = 17,
            LocalizedName = "برند محصول17",
            Name = "ProductBrand17",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand18 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug18",
            Code = 18,
            LocalizedName = "برند محصول18",
            Name = "ProductBrand18",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand19 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug19",
            Code = 19,
            LocalizedName = "برند محصول19",
            Name = "ProductBrand19",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        var productBrand20 = new ProductBrand()
        {
            Related7SoftBrandId = new Guid(),
            CountryId = country1.Id,
            Slug = "slug20",
            Code = 20,
            LocalizedName = "برند محصول20",
            Name = "ProductBrand20",
            BrandLogo = Guid.Empty,// "",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };









        if (!_context.ProductBrands.Any())
        {
            _context.ProductBrands.AddRange(new List<ProductBrand>()
            {
                productBrand1, productBrand2, productBrand3, productBrand4, productBrand5, productBrand6, productBrand7, productBrand8, productBrand9, productBrand10,
                productBrand11, productBrand12, productBrand13, productBrand14, productBrand15, productBrand16, productBrand17, productBrand18, productBrand19, productBrand20
            });
        }
        #endregion
        #region ProductCategory
        var productCategory11 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 9,
            Name = "CKD items",
            Slug = "اقلام CKD".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "اقلام CKD",
            ProductCategoryNo = "28",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory12 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 11,
            Name = "Original SST",
            Slug = "ا. م. اصلی".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "ا. م. اصلی",
            ProductCategoryNo = "39",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory1.ProductChildrenCategories?.Add(productCategory11);
        productCategory1.ProductChildrenCategories?.Add(productCategory12);
        var productCategory13 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 53,
            Name = "Main battery",
            Slug = "باتری اصلی خودرو".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "باتری اصلی خودرو",
            ProductCategoryNo = "41",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory14 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 4,
            Name = "ProductCategory14",
            Slug = "1دسته بندی 4".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 4",
            ProductCategoryNo = "4",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory2.ProductChildrenCategories?.Add(productCategory13);
        productCategory2.ProductChildrenCategories?.Add(productCategory14);
        var productCategory15 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 5,
            Name = "ProductCategory15",
            Slug = "1دسته بندی 5".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 5",
            ProductCategoryNo = "5",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory16 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 6,
            Name = "ProductCategory16",
            Slug = "1دسته بندی 6".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 6",
            ProductCategoryNo = "6",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory3.ProductChildrenCategories?.Add(productCategory15);
        productCategory3.ProductChildrenCategories?.Add(productCategory16);
        var productCategory17 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 7,
            Name = "ProductCategory17",
            Slug = "1دسته بندی 7".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 7",
            ProductCategoryNo = "7",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory18 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 8,
            Name = "ProductCategory18",
            Slug = "1دسته بندی 8".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 8",
            ProductCategoryNo = "8",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory4.ProductChildrenCategories?.Add(productCategory17);
        productCategory4.ProductChildrenCategories?.Add(productCategory18);
        var productCategory19 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 9,
            Name = "ProductCategory19",
            Slug = "1دسته بندی 9".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "1دسته بندی 9",
            ProductCategoryNo = "9",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory110 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 10,
            Name = "ProductCategory110",
            Slug = "دسته بندی 110".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "دسته بندی 110",
            ProductCategoryNo = "10",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory5.ProductChildrenCategories?.Add(productCategory19);
        productCategory5.ProductChildrenCategories?.Add(productCategory110);
        var productCategory111 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 11,
            Name = "ProductCategory111",
            Slug = "دسته بندی 111".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "دسته بندی 111",
            ProductCategoryNo = "11",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory112 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Code = 12,
            Name = "ProductCategory112",
            Slug = "دسته بندی 112".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            LocalizedName = "دسته بندی 112",
            ProductCategoryNo = "12",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory6.ProductChildrenCategories?.Add(productCategory111);
        productCategory6.ProductChildrenCategories?.Add(productCategory112);

        #endregion
        #region Address

        var address1 = new Address()
        {
            Title = "title1",
            CountryId = country1.Id,
            AddressDetails1 = "addressdetails1",
            AddressDetails2 = "addressdetails2",
            City = "city1",
            State = "state1",
            Postcode = "postcode1",
            Default = true,
            CustomerId = customer1.Id
        };
        var address2 = new Address()
        {
            Title = "title2",
            CountryId = country2.Id,
            AddressDetails1 = "addressdetails1",
            AddressDetails2 = "addressdetails2",
            City = "city2",
            State = "state2",
            Postcode = "postcode2",
            Default = true,
            CustomerId = customer2.Id
        };
        var address3 = new Address()
        {
            Title = "title3",
            CountryId = country3.Id,
            AddressDetails1 = "addressdetails1",
            AddressDetails2 = "addressdetails2",
            City = "city3",
            State = "state3",
            Postcode = "postcode3",
            Default = true,
            CustomerId = customer3.Id
        };
        var address4 = new Address()
        {
            Title = "title4",
            CountryId = country3.Id,
            AddressDetails1 = "addressdetails1",
            AddressDetails2 = "addressdetails2",
            City = "city4",
            State = "state4",
            Postcode = "postcode4",
            Default = true,
            CustomerId = customer4.Id
        };

        if (!_context.Addresses.Any())
        {
            _context.Addresses.AddRange(new List<Address>() { address1, address2, address3, address4 });
        }

        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Third Step

        #region Family
        var family1 = new Family()
        {
            Name = "Family1",
            LocalizedName = "خانواده 1",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family2 = new Family()
        {
            Name = "Family2",
            LocalizedName = "خانواده 2",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family3 = new Family()
        {
            Name = "Family3",
            LocalizedName = "خانواده3",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family4 = new Family()
        {
            Name = "Family4",
            LocalizedName = "خانواده4",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family5 = new Family()
        {
            Name = "Family5",
            LocalizedName = "خانواده5",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family6 = new Family()
        {
            Name = "Family6",
            LocalizedName = "خانواده6",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family7 = new Family()
        {
            Name = "Family7",
            LocalizedName = "خانواده7",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var family8 = new Family()
        {
            Name = "Family8",
            LocalizedName = "خانواده8",
            Related7SoftFamilyId = new Guid(),
            VehicleBrandId = vehicleBrand4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        if (!_context.Families.Any())
        {
            _context.Families.AddRange(new List<Family>()
            {
                family1,family2,family3,family4,family5,family6,family7,family8
            });
        }
        #endregion
        #region ProductSubCategory
        var productCategory41 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 1".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 1,
            Name = "ProductCategory41",
            LocalizedName = "4زیردسته 1",
            ProductCategoryNo = "01",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory42 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 2".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 2,
            Name = "ProductCategory42",
            LocalizedName = "4زیردسته 2",
            ProductCategoryNo = "02",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory11.ProductChildrenCategories?.Add(productCategory41);
        productCategory11.ProductChildrenCategories?.Add(productCategory42);
        var productCategory43 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 3".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 3,
            Name = "ProductCategory43",
            LocalizedName = "4زیردسته 3",
            ProductCategoryNo = "03",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory44 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 4".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 4,
            Name = "ProductCategory44",
            LocalizedName = "4زیردسته 4",
            ProductCategoryNo = "04",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory12.ProductChildrenCategories?.Add(productCategory43);
        productCategory12.ProductChildrenCategories?.Add(productCategory44);
        var productCategory45 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 5".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 5,
            Name = "ProductCategory45",
            LocalizedName = "4زیردسته 5",
            ProductCategoryNo = "05",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory46 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 6".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 6,
            Name = "ProductCategory46",
            LocalizedName = "4زیردسته 6",
            ProductCategoryNo = "06",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory13.ProductChildrenCategories?.Add(productCategory45);
        productCategory13.ProductChildrenCategories?.Add(productCategory46);
        var productCategory47 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 7".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 7,
            Name = "ProductCategory47",
            LocalizedName = "4زیردسته 7",
            ProductCategoryNo = "07",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory48 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 8".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 8,
            Name = "ProductCategory48",
            LocalizedName = "4زیردسته 8",
            ProductCategoryNo = "8",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory14.ProductChildrenCategories?.Add(productCategory47);
        productCategory14.ProductChildrenCategories?.Add(productCategory48);
        var productCategory49 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 9".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 9,
            Name = "ProductCategory49",
            LocalizedName = "4زیردسته 9",
            ProductCategoryNo = "9",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory410 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 10".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 10,
            Name = "ProductCategory410",
            LocalizedName = "4زیردسته 10",
            ProductCategoryNo = "10",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory15.ProductChildrenCategories?.Add(productCategory49);
        productCategory15.ProductChildrenCategories?.Add(productCategory410);
        var productCategory411 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 11".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 11,
            Name = "ProductCategory411",
            LocalizedName = "4زیردسته 11",
            ProductCategoryNo = "11",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory412 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 12".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 12,
            Name = "ProductCategory412",
            LocalizedName = "4زیردسته 12",
            ProductCategoryNo = "12",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory16.ProductChildrenCategories?.Add(productCategory411);
        productCategory16.ProductChildrenCategories?.Add(productCategory412);
        var productCategory413 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 13".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 13,
            Name = "ProductCategory413",
            LocalizedName = "4زیردسته 13",
            ProductCategoryNo = "13",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory414 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 14".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 14,
            Name = "ProductCategory414",
            LocalizedName = "4زیردسته 14",
            ProductCategoryNo = "14",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory17.ProductChildrenCategories?.Add(productCategory413);
        productCategory17.ProductChildrenCategories?.Add(productCategory414);
        var productCategory415 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 15".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 15,
            Name = "ProductCategory415",
            LocalizedName = "4زیردسته 15",
            ProductCategoryNo = "15",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory416 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 16".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 16,
            Name = "ProductCategory416",
            LocalizedName = "4زیردسته 16",
            ProductCategoryNo = "16",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory18.ProductChildrenCategories?.Add(productCategory415);
        productCategory18.ProductChildrenCategories?.Add(productCategory416);
        var productCategory417 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 17".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 17,
            Name = "ProductCategory417",
            LocalizedName = "4زیردسته 17",
            ProductCategoryNo = "17",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory418 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 18".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 18,
            Name = "ProductCategory418",
            LocalizedName = "4زیردسته 18",
            ProductCategoryNo = "18",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory19.ProductChildrenCategories?.Add(productCategory417);
        productCategory19.ProductChildrenCategories?.Add(productCategory418);
        var productCategory419 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 19".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 19,
            Name = "ProductCategory419",
            LocalizedName = "4زیردسته 19",
            ProductCategoryNo = "19",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory420 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 20".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 20,
            Name = "ProductCategory420",
            LocalizedName = "4زیردسته 20",
            ProductCategoryNo = "20",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory110.ProductChildrenCategories?.Add(productCategory419);
        productCategory110.ProductChildrenCategories?.Add(productCategory420);
        var productCategory421 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 21".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 21,
            Name = "ProductCategory421",
            LocalizedName = "4زیردسته 21",
            ProductCategoryNo = "21",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory422 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 22".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 22,
            Name = "ProductCategory422",
            LocalizedName = "4زیردسته 22",
            ProductCategoryNo = "22",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory111.ProductChildrenCategories?.Add(productCategory421);
        productCategory111.ProductChildrenCategories?.Add(productCategory422);
        var productCategory423 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 23".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 23,
            Name = "ProductCategory423",
            LocalizedName = "4زیردسته 23",
            ProductCategoryNo = "23",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var productCategory424 = new ProductCategory()
        {
            Related7SoftProductCategoryId = new Guid(),
            Slug = "4زیردسته 24".ToLower().Replace(' ', '-'),
            Image =  Guid.Empty, // "",
            Code = 24,
            Name = "ProductCategory424",
            LocalizedName = "4زیردسته 24",
            ProductCategoryNo = "24",
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        productCategory112.ProductChildrenCategories?.Add(productCategory423);
        productCategory112.ProductChildrenCategories?.Add(productCategory424);
        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Forth Step

        #region Model
        var model1 = new Model()
        {
            Name = "Model1",
            LocalizedName = "مدل 1",
            Related7SoftModelId = new Guid(),
            FamilyId = family1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model2 = new Model()
        {
            Name = "Model2",
            LocalizedName = "مدل 2",
            Related7SoftModelId = new Guid(),
            FamilyId = family2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model3 = new Model()
        {
            Name = "Model3",
            LocalizedName = "مدل 3",
            Related7SoftModelId = new Guid(),
            FamilyId = family3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model4 = new Model()
        {
            Name = "Model4",
            LocalizedName = "مدل 4",
            Related7SoftModelId = new Guid(),
            FamilyId = family4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model5 = new Model()
        {
            Name = "Model5",
            LocalizedName = "مدل 5",
            Related7SoftModelId = new Guid(),
            FamilyId = family5.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model6 = new Model()
        {
            Name = "Model6",
            LocalizedName = "مدل 6",
            Related7SoftModelId = new Guid(),
            FamilyId = family6.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model7 = new Model()
        {
            Name = "Model7",
            LocalizedName = "مدل 7",
            Related7SoftModelId = new Guid(),
            FamilyId = family7.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model8 = new Model()
        {
            Name = "Model8",
            LocalizedName = "مدل 8",
            Related7SoftModelId = new Guid(),
            FamilyId = family8.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model9 = new Model()
        {
            Name = "Model9",
            LocalizedName = "مدل 9",
            Related7SoftModelId = new Guid(),
            FamilyId = family1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model10 = new Model()
        {
            Name = "Model10",
            LocalizedName = "مدل 10",
            Related7SoftModelId = new Guid(),
            FamilyId = family2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model11 = new Model()
        {
            Name = "Model11",
            LocalizedName = "مدل 11",
            Related7SoftModelId = new Guid(),
            FamilyId = family3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model12 = new Model()
        {
            Name = "Model12",
            LocalizedName = "مدل 12",
            Related7SoftModelId = new Guid(),
            FamilyId = family4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model13 = new Model()
        {
            Name = "Model13",
            LocalizedName = "مدل 13",
            Related7SoftModelId = new Guid(),
            FamilyId = family5.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model14 = new Model()
        {
            Name = "Model14",
            LocalizedName = "مدل 14",
            Related7SoftModelId = new Guid(),
            FamilyId = family6.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model15 = new Model()
        {
            Name = "Model15",
            LocalizedName = "مدل 15",
            Related7SoftModelId = new Guid(),
            FamilyId = family7.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var model16 = new Model()
        {
            Name = "Model16",
            LocalizedName = "مدل 16",
            Related7SoftModelId = new Guid(),
            FamilyId = family8.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        if (!_context.Models.Any())
        {
            _context.Models.AddRange(new List<Model>(){
                model1,model2,model3,model4,model5,model6,model7,model8,model9,model10,model11,model12,model13, model14, model15,model16
            });
        }
        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Fifth Step

        #region Kind
        var kind1 = new Kind()
        {
            Name = "Kind1",
            LocalizedName = "نوع 1",
            Related7SoftKindId = new Guid(),
            ModelId = model1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind2 = new Kind()
        {
            Name = "Kind2",
            LocalizedName = "نوع 2",
            Related7SoftKindId = new Guid(),
            ModelId = model2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind3 = new Kind()
        {
            Name = "Kind3",
            LocalizedName = "نوع 3",
            Related7SoftKindId = new Guid(),
            ModelId = model3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind4 = new Kind()
        {
            Name = "Kind4",
            LocalizedName = "نوع 4",
            Related7SoftKindId = new Guid(),
            ModelId = model4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind5 = new Kind()
        {
            Name = "Kind5",
            LocalizedName = "نوع 5",
            Related7SoftKindId = new Guid(),
            ModelId = model5.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind6 = new Kind()
        {
            Name = "Kind6",
            LocalizedName = "نوع 6",
            Related7SoftKindId = new Guid(),
            ModelId = model6.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind7 = new Kind()
        {
            Name = "Kind7",
            LocalizedName = "نوع 7",
            Related7SoftKindId = new Guid(),
            ModelId = model7.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind8 = new Kind()
        {
            Name = "Kind8",
            LocalizedName = "نوع 8",
            Related7SoftKindId = new Guid(),
            ModelId = model8.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind9 = new Kind()
        {
            Name = "Kind9",
            LocalizedName = "نوع 9",
            Related7SoftKindId = new Guid(),
            ModelId = model9.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind10 = new Kind()
        {
            Name = "Kind10",
            LocalizedName = "نوع 10",
            Related7SoftKindId = new Guid(),
            ModelId = model10.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind11 = new Kind()
        {
            Name = "Kind11",
            LocalizedName = "نوع 11",
            Related7SoftKindId = new Guid(),
            ModelId = model11.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind12 = new Kind()
        {
            Name = "Kind12",
            LocalizedName = "نوع 12",
            Related7SoftKindId = new Guid(),
            ModelId = model12.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind13 = new Kind()
        {
            Name = "Kind13",
            LocalizedName = "نوع 13",
            Related7SoftKindId = new Guid(),
            ModelId = model13.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind14 = new Kind()
        {
            Name = "Kind14",
            LocalizedName = "نوع 14",
            Related7SoftKindId = new Guid(),
            ModelId = model14.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind15 = new Kind()
        {
            Name = "Kind15",
            LocalizedName = "نوع 15",
            Related7SoftKindId = new Guid(),
            ModelId = model15.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind16 = new Kind()
        {
            Name = "Kind16",
            LocalizedName = "نوع 16",
            Related7SoftKindId = new Guid(),
            ModelId = model16.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind17 = new Kind()
        {
            Name = "Kind17",
            LocalizedName = "نوع 17",
            Related7SoftKindId = new Guid(),
            ModelId = model1.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind18 = new Kind()
        {
            Name = "Kind18",
            LocalizedName = "نوع 18",
            Related7SoftKindId = new Guid(),
            ModelId = model2.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind19 = new Kind()
        {
            Name = "Kind19",
            LocalizedName = "نوع 19",
            Related7SoftKindId = new Guid(),
            ModelId = model3.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind20 = new Kind()
        {
            Name = "Kind20",
            LocalizedName = "نوع 20",
            Related7SoftKindId = new Guid(),
            ModelId = model4.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind21 = new Kind()
        {
            Name = "Kind21",
            LocalizedName = "نوع 21",
            Related7SoftKindId = new Guid(),
            ModelId = model5.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind22 = new Kind()
        {
            Name = "Kind22",
            LocalizedName = "نوع 22",
            Related7SoftKindId = new Guid(),
            ModelId = model6.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind23 = new Kind()
        {
            Name = "Kind23",
            LocalizedName = "نوع 23",
            Related7SoftKindId = new Guid(),
            ModelId = model7.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind24 = new Kind()
        {
            Name = "Kind24",
            LocalizedName = "نوع 24",
            Related7SoftKindId = new Guid(),
            ModelId = model8.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind25 = new Kind()
        {
            Name = "Kind25",
            LocalizedName = "نوع 25",
            Related7SoftKindId = new Guid(),
            ModelId = model9.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind26 = new Kind()
        {
            Name = "Kind26",
            LocalizedName = "نوع 26",
            Related7SoftKindId = new Guid(),
            ModelId = model10.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind27 = new Kind()
        {
            Name = "Kind27",
            LocalizedName = "نوع 27",
            Related7SoftKindId = new Guid(),
            ModelId = model11.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind28 = new Kind()
        {
            Name = "Kind28",
            LocalizedName = "نوع 28",
            Related7SoftKindId = new Guid(),
            ModelId = model12.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind29 = new Kind()
        {
            Name = "Kind29",
            LocalizedName = "نوع 29",
            Related7SoftKindId = new Guid(),
            ModelId = model13.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind30 = new Kind()
        {
            Name = "Kind30",
            LocalizedName = "نوع 30",
            Related7SoftKindId = new Guid(),
            ModelId = model14.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind31 = new Kind()
        {
            Name = "Kind31",
            LocalizedName = "نوع 31",
            Related7SoftKindId = new Guid(),
            ModelId = model15.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        var kind32 = new Kind()
        {
            Name = "Kind32",
            LocalizedName = "نوع 32",
            Related7SoftKindId = new Guid(),
            ModelId = model16.Id,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };

        if (!_context.Kinds.Any())
        {
            _context.Kinds.AddRange(new List<Kind>()
            {
                kind1,kind2, kind3, kind4, kind5, kind6,kind7, kind8, kind9, kind10,
                kind11, kind12, kind13, kind14, kind15,kind16, kind17,kind18, kind19,kind20,
                kind21, kind22, kind23, kind24, kind25, kind26, kind27, kind28, kind29,kind30,kind31,kind32
            });
        }
        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Sixth Step

        #region Vehicle

        var vehicle1 = new Vehicle()
        {
            VinNumber = "12345678912345678",
            KindId = kind1.Id,
            CustomerId = customer1.Id
        };
        var vehicle2 = new Vehicle()
        {
            VinNumber = "22345678912345678",
            KindId = kind1.Id,
            CustomerId = customer1.Id
        };
        var vehicle3 = new Vehicle()
        {
            VinNumber = "32345678912345678",
            KindId = kind2.Id,
            CustomerId = customer2.Id
        };

        if (!_context.Vehicles.Any())
        {
            _context.Vehicles.AddRange(new List<Vehicle>() { vehicle1, vehicle2, vehicle3 });
        }
        #endregion


        #region Badge

        var badge1 = new Badge() { Value = "sale" };
        var badge2 = new Badge() { Value = "new" };
        var badge3 = new Badge() { Value = "hot" };

        #endregion
        #region Tag

        var tag1 = new Tag() { EnTitle = "Filter", FaTitle = "فیلتر" };
        var tag2 = new Tag() { EnTitle = "Bumper", FaTitle = "سپر" };

        #endregion

        #region Option For Product

        #region Structure

        #region Color

        #region ProductOptionValueColor

        var productOptionValueColor1 = new ProductOptionValueColor() { Name = "White", Slug = "white", Color = "#fff" };
        var productOptionValueColor2 = new ProductOptionValueColor() { Name = "Yellow", Slug = "yellow", Color = "#ffd333" };
        var productOptionValueColor3 = new ProductOptionValueColor() { Name = "Red", Slug = "red", Color = "#ff4040" };
        var productOptionValueColor4 = new ProductOptionValueColor() { Name = "Blue", Slug = "blue", Color = "#4080ff" };

        #endregion
        #region ProductOptionColor

        var productOptionColor1 = new ProductOptionColor()
        {
            Name = "Color",
            Slug = "color",
            Values = new List<ProductOptionValueColor>() { productOptionValueColor1, productOptionValueColor2, productOptionValueColor3, productOptionValueColor4 }
        };

        var attributeGroupColor = new ProductTypeAttributeGroup
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Attributes = new List<ProductTypeAttributeGroupAttribute>()
            {
                new ProductTypeAttributeGroupAttribute(EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(productOptionColor1.Type))
            }
        };

        #endregion

        #endregion

        #region Material

        #region ProductOptionValueMaterial

        var productOptionValueMaterial1 = new ProductOptionValueMaterial() { Name = "Steel", Slug = "steel" };
        var productOptionValueMaterial2 = new ProductOptionValueMaterial() { Name = "Aluminium", Slug = "aluminium" };
        var productOptionValueMaterial3 = new ProductOptionValueMaterial() { Name = "Thorium", Slug = "thorium" };

        #endregion
        #region ProductOptionMaterial

        var productOptionMaterial1 = new ProductOptionMaterial()
        {
            Name = "Material",
            Slug = "material",
            Values = new List<ProductOptionValueMaterial>() { productOptionValueMaterial1, productOptionValueMaterial2, productOptionValueMaterial3 }
        };

        var attributeGroupMaterial = new ProductTypeAttributeGroup
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Attributes = new List<ProductTypeAttributeGroupAttribute>()
            {
                new ProductTypeAttributeGroupAttribute(EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(productOptionMaterial1.Type))
            }
        };

        #endregion

        #endregion

        #endregion
        await _context.SaveChangesAsync();
        #endregion
        #region Attribute For Product

        #region ProductTypeAttributeGroupAttribute

        var productTypeAttributeGroupAttribute1 = new ProductTypeAttributeGroupAttribute("length");
        var productTypeAttributeGroupAttribute2 = new ProductTypeAttributeGroupAttribute("width");
        var productTypeAttributeGroupAttribute3 = new ProductTypeAttributeGroupAttribute("height");
        var productTypeAttributeGroupAttribute4 = new ProductTypeAttributeGroupAttribute("speed");
        var productTypeAttributeGroupAttribute5 = new ProductTypeAttributeGroupAttribute("voltage");
        var productTypeAttributeGroupAttribute6 = new ProductTypeAttributeGroupAttribute("power-source");

        //For Option
        var productTypeAttributeGroupAttribute7 = new ProductTypeAttributeGroupAttribute(productOptionColor1.Slug);
        var productTypeAttributeGroupAttribute8 = new ProductTypeAttributeGroupAttribute(productOptionMaterial1.Slug);

        #endregion
        #region ProductTypeAttributeGroup

        var productTypeAttributeGroup1 = new ProductTypeAttributeGroup()
        {
            Name = "Dimensions",
            Slug = "dimensions",
            Attributes = new List<ProductTypeAttributeGroupAttribute>() { productTypeAttributeGroupAttribute1, productTypeAttributeGroupAttribute2, productTypeAttributeGroupAttribute3 }
        };
        var productTypeAttributeGroup2 = new ProductTypeAttributeGroup()
        {
            Name = "General",
            Slug = "general",
            Attributes = new List<ProductTypeAttributeGroupAttribute>() { productTypeAttributeGroupAttribute4, productTypeAttributeGroupAttribute5, productTypeAttributeGroupAttribute6 }
        };
        var productTypeAttributeGroup3 = new ProductTypeAttributeGroup()
        {
            Name = "Option",
            Slug = "option",
            Attributes = new List<ProductTypeAttributeGroupAttribute>() { productTypeAttributeGroupAttribute7, productTypeAttributeGroupAttribute8 }
        };

        #endregion
        #region ProductAttributeType

        var productAttributeType1 = new ProductAttributeType()
        {
            Name = "Default",
            Slug = "default",
            AttributeGroups = new List<ProductTypeAttributeGroup>() { productTypeAttributeGroup1, productTypeAttributeGroup2, productTypeAttributeGroup3 }
        };

        #endregion

        #endregion


        #region Product

        var product1 = new Product()
        {
            Code = 1,
            ProductNo = "288B57752R",
            LocalizedName = "تیغه برف پاکن جلو راست",
            Name = "BRUSH-HAND PASSENGER WIPER",
            ProductCategoryId = productCategory41.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },
            Excerpt = "Excerpt1",
            Description = "Description1",
            Slug = "تیغه برف پاکن جلو راست".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.All,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product1.SetProductAttributeType(productAttributeType1);
        var product2 = new Product()
        {
            Code = 2,
            ProductNo = "2",
            LocalizedName = "محصول 2",
            Name = "product2",
            ProductCategoryId = productCategory41.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt2",
            Description = "Description2",
            Slug = "محصول 2".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product2.SetProductAttributeType(productAttributeType1);

        var product3 = new Product()
        {
            Code = 3,
            ProductNo = "3",
            LocalizedName = "محصول 3",
            Name = "product3",
            ProductCategoryId = productCategory41.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt3",
            Description = "Description3",
            Slug = "محصول 3".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product3.SetProductAttributeType(productAttributeType1);
        product3.SetColorOption(productOptionColor1, attributeGroupColor);
        product3.SetMaterialOption(productOptionMaterial1, attributeGroupMaterial);
        var product4 = new Product()
        {
            Code = 4,
            ProductNo = "4",
            LocalizedName = "محصول 4",
            Name = "product4",
            ProductCategoryId = productCategory42.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt4",
            Description = "Description4",
            Slug = "محصول 4".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product4.SetProductAttributeType(productAttributeType1);
        product4.SetColorOption(productOptionColor1,attributeGroupColor);
        product4.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product5 = new Product()
        {
            Code = 5,
            ProductNo = "5",
            LocalizedName = "محصول 5",
            Name = "product5",
            ProductCategoryId = productCategory42.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt5",
            Description = "Description5",
            Slug = "محصول 5".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product5.SetProductAttributeType(productAttributeType1);
        product5.SetColorOption(productOptionColor1,attributeGroupColor);
        product5.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product6 = new Product()
        {
            Code = 6,
            ProductNo = "6",
            LocalizedName = "محصول 6",
            Name = "product6",
            ProductCategoryId = productCategory42.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt6",
            Description = "Description6",
            Slug = "محصول 6".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product6.SetProductAttributeType(productAttributeType1);
        product6.SetColorOption(productOptionColor1,attributeGroupColor);
        product6.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product7 = new Product()
        {
            Code = 7,
            ProductNo = "7",
            LocalizedName = "محصول 7",
            Name = "product7",
            ProductCategoryId = productCategory43.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt7",
            Description = "Description7",
            Slug = "محصول 7".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product7.SetProductAttributeType(productAttributeType1);
        product7.SetColorOption(productOptionColor1,attributeGroupColor);
        product7.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product8 = new Product()
        {
            Code = 8,
            ProductNo = "8",
            LocalizedName = "محصول 8",
            Name = "product8",
            ProductCategoryId = productCategory43.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt8",
            Description = "Description8",
            Slug = "محصول 8".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product8.SetProductAttributeType(productAttributeType1);
        product8.SetColorOption(productOptionColor1,attributeGroupColor);
        product8.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product9 = new Product()
        {
            Code = 9,
            ProductNo = "9",
            LocalizedName = "محصول 9",
            Name = "product9",
            ProductCategoryId = productCategory43.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt9",
            Description = "Description9",
            Slug = "محصول 9".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product9.SetProductAttributeType(productAttributeType1);
        product9.SetColorOption(productOptionColor1,attributeGroupColor);
        product9.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product10 = new Product()
        {
            Code = 10,
            ProductNo = "10",
            LocalizedName = "محصول 10",
            Name = "product10",
            ProductCategoryId = productCategory44.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt10",
            Description = "Description10",
            Slug = "محصول 10".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product10.SetProductAttributeType(productAttributeType1);
        product10.SetColorOption(productOptionColor1,attributeGroupColor);
        product10.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product11 = new Product()
        {
            Code = 11,
            ProductNo = "11",
            LocalizedName = "محصول 11",
            Name = "product11",
            ProductCategoryId = productCategory44.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt11",
            Description = "Description11",
            Slug = "محصول 11".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product11.SetProductAttributeType(productAttributeType1);
        product11.SetColorOption(productOptionColor1,attributeGroupColor);
        product11.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product12 = new Product()
        {
            Code = 12,
            ProductNo = "12",
            LocalizedName = "محصول 12",
            Name = "product12",
            ProductCategoryId = productCategory44.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt12",
            Description = "Description12",
            Slug = "محصول 12".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product12.SetProductAttributeType(productAttributeType1);
        product12.SetColorOption(productOptionColor1,attributeGroupColor);
        product12.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product13 = new Product()
        {
            Code = 13,
            ProductNo = "13",
            LocalizedName = "محصول 13",
            Name = "product13",
            ProductCategoryId = productCategory45.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt13",
            Description = "Description13",
            Slug = "محصول 13".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product13.SetProductAttributeType(productAttributeType1);
        product13.SetColorOption(productOptionColor1,attributeGroupColor);
        product13.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product14 = new Product()
        {
            Code = 14,
            ProductNo = "14",
            LocalizedName = "محصول 14",
            Name = "product14",
            ProductCategoryId = productCategory45.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType2.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt14",
            Description = "Description14",
            Slug = "محصول 14".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product14.SetProductAttributeType(productAttributeType1);
        product14.SetColorOption(productOptionColor1,attributeGroupColor);
        product14.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product15 = new Product()
        {
            Code = 15,
            ProductNo = "15",
            LocalizedName = "محصول 15",
            Name = "product15",
            ProductCategoryId = productCategory45.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt15",
            Description = "Description15",
            Slug = "محصول 15".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product15.SetProductAttributeType(productAttributeType1);
        product15.SetColorOption(productOptionColor1,attributeGroupColor);
        product15.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product16 = new Product()
        {
            Code = 16,
            ProductNo = "16",
            LocalizedName = "محصول 16",
            Name = "product16",
            ProductCategoryId = productCategory46.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand17.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt16",
            Description = "Description16",
            Slug = "محصول 16".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product16.SetProductAttributeType(productAttributeType1);
        product16.SetColorOption(productOptionColor1,attributeGroupColor);
        product16.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product17 = new Product()
        {
            Code = 17,
            ProductNo = "17",
            LocalizedName = "محصول 17",
            Name = "product17",
            ProductCategoryId = productCategory46.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt17",
            Description = "Description17",
            Slug = "محصول 17".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product17.SetProductAttributeType(productAttributeType1);
        product17.SetColorOption(productOptionColor1,attributeGroupColor);
        product17.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product18 = new Product()
        {
            Code = 18,
            ProductNo = "18",
            LocalizedName = "محصول 18",
            Name = "product18",
            ProductCategoryId = productCategory46.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt18",
            Description = "Description18",
            Slug = "محصول 18".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product18.SetProductAttributeType(productAttributeType1);
        product18.SetColorOption(productOptionColor1,attributeGroupColor);
        product18.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product19 = new Product()
        {
            Code = 19,
            ProductNo = "19",
            LocalizedName = "محصول 19",
            Name = "product19",
            ProductCategoryId = productCategory47.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt19",
            Description = "Description19",
            Slug = "محصول 19".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product19.SetProductAttributeType(productAttributeType1);
        product19.SetColorOption(productOptionColor1,attributeGroupColor);
        product19.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product20 = new Product()
        {
            Code = 20,
            ProductNo = "20",
            LocalizedName = "محصول 20",
            Name = "product20",
            ProductCategoryId = productCategory47.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit1.Id,
            CommonCountingUnitId = countingUnit1.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt20",
            Description = "Description20",
            Slug = "محصول 20".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product20.SetProductAttributeType(productAttributeType1);
        product20.SetColorOption(productOptionColor1,attributeGroupColor);
        product20.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product21 = new Product()
        {
            Code = 21,
            ProductNo = "21",
            LocalizedName = "محصول 21",
            Name = "product21",
            ProductCategoryId = productCategory47.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType3.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt21",
            Description = "Description21",
            Slug = "محصول 21".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product21.SetProductAttributeType(productAttributeType1);
        product21.SetColorOption(productOptionColor1,attributeGroupColor);
        product21.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product22 = new Product()
        {
            Code = 22,
            ProductNo = "22",
            LocalizedName = "محصول 22",
            Name = "product22",
            ProductCategoryId = productCategory48.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt22",
            Description = "Description22",
            Slug = "محصول 22".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product22.SetProductAttributeType(productAttributeType1);
        product22.SetColorOption(productOptionColor1,attributeGroupColor);
        product22.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product23 = new Product()
        {
            Code = 23,
            ProductNo = "23",
            LocalizedName = "محصول 23",
            Name = "product23",
            ProductCategoryId = productCategory48.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt23",
            Description = "Description23",
            Slug = "محصول 23".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product23.SetProductAttributeType(productAttributeType1);
        product23.SetColorOption(productOptionColor1,attributeGroupColor);
        product23.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product24 = new Product()
        {
            Code = 24,
            ProductNo = "24",
            LocalizedName = "محصول 24",
            Name = "product24",
            ProductCategoryId = productCategory48.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider2.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt24",
            Description = "Description24",
            Slug = "محصول 24".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product24.SetProductAttributeType(productAttributeType1);
        product24.SetColorOption(productOptionColor1,attributeGroupColor);
        product24.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product25 = new Product()
        {
            Code = 25,
            ProductNo = "25",
            LocalizedName = "محصول 25",
            Name = "product25",
            ProductCategoryId = productCategory49.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt25",
            Description = "Description25",
            Slug = "محصول 25".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product25.SetProductAttributeType(productAttributeType1);
        product25.SetColorOption(productOptionColor1,attributeGroupColor);
        product25.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product26 = new Product()
        {
            Code = 26,
            ProductNo = "26",
            LocalizedName = "محصول 26",
            Name = "product26",
            ProductCategoryId = productCategory49.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt26",
            Description = "Description26",
            Slug = "محصول 26".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product26.SetProductAttributeType(productAttributeType1);
        product26.SetColorOption(productOptionColor1,attributeGroupColor);
        product26.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product27 = new Product()
        {
            Code = 27,
            ProductNo = "27",
            LocalizedName = "محصول 27",
            Name = "product27",
            ProductCategoryId = productCategory49.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt27",
            Description = "Description27",
            Slug = "محصول 27".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product27.SetProductAttributeType(productAttributeType1);
        product27.SetColorOption(productOptionColor1,attributeGroupColor);
        product27.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product28 = new Product()
        {
            Code = 28,
            ProductNo = "28",
            LocalizedName = "محصول 28",
            Name = "product28",
            ProductCategoryId = productCategory410.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType4.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt28",
            Description = "Description28",
            Slug = "محصول 28".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product28.SetProductAttributeType(productAttributeType1);
        product28.SetColorOption(productOptionColor1,attributeGroupColor);
        product28.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product29 = new Product()
        {
            Code = 29,
            ProductNo = "29",
            LocalizedName = "محصول 29",
            Name = "product29",
            ProductCategoryId = productCategory410.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt29",
            Description = "Description29",
            Slug = "محصول 29".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product29.SetProductAttributeType(productAttributeType1);
        product29.SetColorOption(productOptionColor1,attributeGroupColor);
        product29.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product30 = new Product()
        {
            Code = 30,
            ProductNo = "30",
            LocalizedName = "محصول 30",
            Name = "product30",
            ProductCategoryId = productCategory410.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt30",
            Description = "Description30",
            Slug = "محصول 30".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product30.SetProductAttributeType(productAttributeType1);
        product30.SetColorOption(productOptionColor1,attributeGroupColor);
        product30.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product31 = new Product()
        {
            Code = 31,
            ProductNo = "31",
            LocalizedName = "محصول 31",
            Name = "product31",
            ProductCategoryId = productCategory411.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt31",
            Description = "Description31",
            Slug = "محصول 31".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product31.SetProductAttributeType(productAttributeType1);
        product31.SetColorOption(productOptionColor1,attributeGroupColor);
        product31.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product32 = new Product()
        {
            Code = 32,
            ProductNo = "32",
            LocalizedName = "محصول 32",
            Name = "product32",
            ProductCategoryId = productCategory411.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand18.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus1.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt32",
            Description = "Description32",
            Slug = "محصول 32".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product32.SetProductAttributeType(productAttributeType1);
        product32.SetColorOption(productOptionColor1,attributeGroupColor);
        product32.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product33 = new Product()
        {
            Code = 33,
            ProductNo = "33",
            LocalizedName = "محصول 33",
            Name = "product33",
            ProductCategoryId = productCategory411.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt33",
            Description = "Description33",
            Slug = "محصول 33".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product33.SetProductAttributeType(productAttributeType1);
        product33.SetColorOption(productOptionColor1,attributeGroupColor);
        product33.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product34 = new Product()
        {
            Code = 34,
            ProductNo = "34",
            LocalizedName = "محصول 34",
            Name = "product34",
            ProductCategoryId = productCategory412.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt34",
            Description = "Description34",
            Slug = "محصول 34".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product34.SetProductAttributeType(productAttributeType1);
        product34.SetColorOption(productOptionColor1,attributeGroupColor);
        product34.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product35 = new Product()
        {
            Code = 35,
            ProductNo = "35",
            LocalizedName = "محصول 35",
            Name = "product35",
            ProductCategoryId = productCategory412.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType5.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt35",
            Description = "Description35",
            Slug = "محصول 35".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product35.SetProductAttributeType(productAttributeType1);
        product35.SetColorOption(productOptionColor1,attributeGroupColor);
        product35.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product36 = new Product()
        {
            Code = 36,
            ProductNo = "36",
            LocalizedName = "محصول 36",
            Name = "product36",
            ProductCategoryId = productCategory412.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider3.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt36",
            Description = "Description36",
            Slug = "محصول 36".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product36.SetProductAttributeType(productAttributeType1);
        product36.SetColorOption(productOptionColor1,attributeGroupColor);
        product36.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product37 = new Product()
        {
            Code = 37,
            ProductNo = "37",
            LocalizedName = "محصول 37",
            Name = "product37",
            ProductCategoryId = productCategory413.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt37",
            Description = "Description37",
            Slug = "محصول 37".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product37.SetProductAttributeType(productAttributeType1);
        product37.SetColorOption(productOptionColor1,attributeGroupColor);
        product37.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product38 = new Product()
        {
            Code = 38,
            ProductNo = "38",
            LocalizedName = "محصول 38",
            Name = "product38",
            ProductCategoryId = productCategory413.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt38",
            Description = "Description38",
            Slug = "محصول 38".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product38.SetProductAttributeType(productAttributeType1);
        product38.SetColorOption(productOptionColor1,attributeGroupColor);
        product38.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product39 = new Product()
        {
            Code = 39,
            ProductNo = "39",
            LocalizedName = "محصول 39",
            Name = "product39",
            ProductCategoryId = productCategory413.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt39",
            Description = "Description39",
            Slug = "محصول 39".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product39.SetProductAttributeType(productAttributeType1);
        product39.SetColorOption(productOptionColor1,attributeGroupColor);
        product39.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product40 = new Product()
        {
            Code = 40,
            ProductNo = "40",
            LocalizedName = "محصول 40",
            Name = "product40",
            ProductCategoryId = productCategory414.Id,
            CountryId = country2.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt40",
            Description = "Description40",
            Slug = "محصول 40".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product40.SetProductAttributeType(productAttributeType1);
        product40.SetColorOption(productOptionColor1,attributeGroupColor);
        product40.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product41 = new Product()
        {
            Code = 41,
            ProductNo = "41",
            LocalizedName = "محصول 41",
            Name = "product41",
            ProductCategoryId = productCategory414.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit2.Id,
            CommonCountingUnitId = countingUnit2.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt41",
            Description = "Description41",
            Slug = "محصول 41".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product41.SetProductAttributeType(productAttributeType1);
        product41.SetColorOption(productOptionColor1,attributeGroupColor);
        product41.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product42 = new Product()
        {
            Code = 42,
            ProductNo = "42",
            LocalizedName = "محصول 42",
            Name = "product42",
            ProductCategoryId = productCategory414.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType6.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt42",
            Description = "Description42",
            Slug = "محصول 42".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product42.SetProductAttributeType(productAttributeType1);
        product42.SetColorOption(productOptionColor1,attributeGroupColor);
        product42.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product43 = new Product()
        {
            Code = 43,
            ProductNo = "43",
            LocalizedName = "محصول 43",
            Name = "product43",
            ProductCategoryId = productCategory415.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt43",
            Description = "Description43",
            Slug = "محصول 43".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product43.SetProductAttributeType(productAttributeType1);
        product43.SetColorOption(productOptionColor1,attributeGroupColor);
        product43.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product44 = new Product()
        {
            Code = 44,
            ProductNo = "44",
            LocalizedName = "محصول 44",
            Name = "product44",
            ProductCategoryId = productCategory415.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt44",
            Description = "Description44",
            Slug = "محصول 44".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product44.SetProductAttributeType(productAttributeType1);
        product44.SetColorOption(productOptionColor1,attributeGroupColor);
        product44.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product45 = new Product()
        {
            Code = 45,
            ProductNo = "45",
            LocalizedName = "محصول 45",
            Name = "product45",
            ProductCategoryId = productCategory415.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt45",
            Description = "Description45",
            Slug = "محصول 45".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product45.SetProductAttributeType(productAttributeType1);
        product45.SetColorOption(productOptionColor1,attributeGroupColor);
        product45.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product46 = new Product()
        {
            Code = 46,
            ProductNo = "46",
            LocalizedName = "محصول 46",
            Name = "product46",
            ProductCategoryId = productCategory416.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt46",
            Description = "Description46",
            Slug = "محصول 46".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product46.SetProductAttributeType(productAttributeType1);
        product46.SetColorOption(productOptionColor1,attributeGroupColor);
        product46.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product47 = new Product()
        {
            Code = 47,
            ProductNo = "47",
            LocalizedName = "محصول 47",
            Name = "product47",
            ProductCategoryId = productCategory416.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt47",
            Description = "Description47",
            Slug = "محصول 47".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product47.SetProductAttributeType(productAttributeType1);
        product47.SetColorOption(productOptionColor1,attributeGroupColor);
        product47.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product48 = new Product()
        {
            Code = 48,
            ProductNo = "48",
            LocalizedName = "محصول 48",
            Name = "product48",
            ProductCategoryId = productCategory416.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand19.Id,
            ProviderId = provider4.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt48",
            Description = "Description48",
            Slug = "محصول 48".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product48.SetProductAttributeType(productAttributeType1);
        product48.SetColorOption(productOptionColor1,attributeGroupColor);
        product48.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product49 = new Product()
        {
            Code = 49,
            ProductNo = "49",
            LocalizedName = "محصول 49",
            Name = "product49",
            ProductCategoryId = productCategory417.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType7.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt49",
            Description = "Description49",
            Slug = "محصول 49".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product49.SetProductAttributeType(productAttributeType1);
        product49.SetColorOption(productOptionColor1,attributeGroupColor);
        product49.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product50 = new Product()
        {
            Code = 50,
            ProductNo = "50",
            LocalizedName = "محصول 50",
            Name = "product50",
            ProductCategoryId = productCategory417.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt50",
            Description = "Description50",
            Slug = "محصول 50".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product50.SetProductAttributeType(productAttributeType1);
        product50.SetColorOption(productOptionColor1,attributeGroupColor);
        product50.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product51 = new Product()
        {
            Code = 51,
            ProductNo = "51",
            LocalizedName = "محصول 51",
            Name = "product51",
            ProductCategoryId = productCategory417.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt51",
            Description = "Description51",
            Slug = "محصول 51".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product51.SetProductAttributeType(productAttributeType1);
        product51.SetColorOption(productOptionColor1,attributeGroupColor);
        product51.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product52 = new Product()
        {
            Code = 52,
            ProductNo = "52",
            LocalizedName = "محصول 52",
            Name = "product52",
            ProductCategoryId = productCategory418.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt52",
            Description = "Description52",
            Slug = "محصول 52".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product52.SetProductAttributeType(productAttributeType1);
        product52.SetColorOption(productOptionColor1,attributeGroupColor);
        product52.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product53 = new Product()
        {
            Code = 53,
            ProductNo = "53",
            LocalizedName = "محصول 53",
            Name = "product53",
            ProductCategoryId = productCategory418.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt53",
            Description = "Description53",
            Slug = "محصول 53".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product53.SetProductAttributeType(productAttributeType1);
        product53.SetColorOption(productOptionColor1,attributeGroupColor);
        product53.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product54 = new Product()
        {
            Code = 54,
            ProductNo = "54",
            LocalizedName = "محصول 54",
            Name = "product54",
            ProductCategoryId = productCategory418.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt54",
            Description = "Description54",
            Slug = "محصول 54".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product54.SetProductAttributeType(productAttributeType1);
        product54.SetColorOption(productOptionColor1,attributeGroupColor);
        product54.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product55 = new Product()
        {
            Code = 55,
            ProductNo = "55",
            LocalizedName = "محصول 55",
            Name = "product55",
            ProductCategoryId = productCategory419.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt55",
            Description = "Description55",
            Slug = "محصول 55".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product55.SetProductAttributeType(productAttributeType1);
        product55.SetColorOption(productOptionColor1,attributeGroupColor);
        product55.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product56 = new Product()
        {
            Code = 56,
            ProductNo = "56",
            LocalizedName = "محصول 56",
            Name = "product56",
            ProductCategoryId = productCategory419.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType8.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt56",
            Description = "Description56",
            Slug = "محصول 56".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product56.SetProductAttributeType(productAttributeType1);
        product56.SetColorOption(productOptionColor1,attributeGroupColor);
        product56.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product57 = new Product()
        {
            Code = 57,
            ProductNo = "57",
            LocalizedName = "محصول 57",
            Name = "product57",
            ProductCategoryId = productCategory419.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt57",
            Description = "Description57",
            Slug = "محصول 57".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product57.SetProductAttributeType(productAttributeType1);
        product57.SetColorOption(productOptionColor1,attributeGroupColor);
        product57.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product58 = new Product()
        {
            Code = 58,
            ProductNo = "58",
            LocalizedName = "محصول 58",
            Name = "product58",
            ProductCategoryId = productCategory420.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt58",
            Description = "Description58",
            Slug = "محصول 58".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product58.SetProductAttributeType(productAttributeType1);
        product58.SetColorOption(productOptionColor1,attributeGroupColor);
        product58.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product59 = new Product()
        {
            Code = 59,
            ProductNo = "59",
            LocalizedName = "محصول 59",
            Name = "product59",
            ProductCategoryId = productCategory420.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt59",
            Description = "Description59",
            Slug = "محصول 59".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product59.SetProductAttributeType(productAttributeType1);
        product59.SetColorOption(productOptionColor1,attributeGroupColor);
        product59.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product60 = new Product()
        {
            Code = 60,
            ProductNo = "60",
            LocalizedName = "محصول 60",
            Name = "product60",
            ProductCategoryId = productCategory420.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider5.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt60",
            Description = "Description60",
            Slug = "محصول 60".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product60.SetProductAttributeType(productAttributeType1);
        product60.SetColorOption(productOptionColor1,attributeGroupColor);
        product60.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product61 = new Product()
        {
            Code = 61,
            ProductNo = "61",
            LocalizedName = "محصول 61",
            Name = "product61",
            ProductCategoryId = productCategory421.Id,
            CountryId = country3.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt61",
            Description = "Description61",
            Slug = "محصول 61".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product61.SetProductAttributeType(productAttributeType1);
        product61.SetColorOption(productOptionColor1,attributeGroupColor);
        product61.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product62 = new Product()
        {
            Code = 62,
            ProductNo = "62",
            LocalizedName = "محصول 62",
            Name = "product62",
            ProductCategoryId = productCategory422.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt62",
            Description = "Description62",
            Slug = "محصول 62".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product62.SetProductAttributeType(productAttributeType1);
        product62.SetColorOption(productOptionColor1,attributeGroupColor);
        product62.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product63 = new Product()
        {
            Code = 63,
            ProductNo = "63",
            LocalizedName = "محصول 63",
            Name = "product63",
            ProductCategoryId = productCategory423.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType9.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt63",
            Description = "Description63",
            Slug = "محصول 63".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product63.SetProductAttributeType(productAttributeType1);
        product63.SetColorOption(productOptionColor1,attributeGroupColor);
        product63.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);
        var product64 = new Product()
        {
            Code = 64,
            ProductNo = "64",
            LocalizedName = "محصول 64",
            Name = "product64",
            ProductCategoryId = productCategory424.Id,
            CountryId = country1.Id,
            ProductBrandId = productBrand20.Id,
            ProviderId = provider1.Id,
            Height = null,
            Width = null,
            Length = null,
            ProductCatalog = null,
            OrderRate = 1.000,
            MainCountingUnitId = countingUnit3.Id,
            CommonCountingUnitId = countingUnit3.Id,
            NetWeight = null,
            GrossWeight = null,
            VolumeWeight = null,
            ProductTypeId = productType1.Id,
            ProductStatusId = productStatus2.Id,
            OldProductNo = null,
            Mileage = null,
            Duration = null,
            Related7SoftProductId = new Guid(),
            Images = new List<ProductImage>() { productImage1, productImage2 },
            Tags = new List<Tag>() { tag1, tag2 },

            Excerpt = "Excerpt64",
            Description = "Description64",
            Slug = "محصول 64".ToLower().Replace(' ', '-'),
            Compatibility = CompatibilityEnum.Unknown,
            Created = DateTime.Now,
            CreatedBy = null,
            LastModified = null,
            LastModifiedBy = null
        };
        product64.SetProductAttributeType(productAttributeType1);
        product64.SetColorOption(productOptionColor1,attributeGroupColor);
        product64.SetMaterialOption(productOptionMaterial1,attributeGroupMaterial);


        if (!_context.Products.Any())
        {
            _context.Products.AddRange(new List<Product>()
            {
                product1,product2,product3,product4,product5,product6,product7,product8,product9,product10,
                product11,product12,product13,product14,product15,product16,product17,product18,product19,product20,
                product21,product22,product23,product24,product25,product26,product27,product28,product29,product30,
                product31,product32,product33,product34,product35,product36,product37,product38,product39,product40,
                product41,product42,product43,product44, product45,product46,product47,product48,product49,product50,
                product51,product52,product53,product54,product55,product56,product57,product58,product59,product60,
                product61,product62,product63,product64
            });

        }
        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Seventh Step

        #region Review

        var review1 = new Review() { Rating = 1, Content = "content1", CustomerId = customer1.Id, ProductId = product1.Id, AuthorName = "author1" };
        var review2 = new Review() { Rating = 2, Content = "content2", CustomerId = customer2.Id, ProductId = product2.Id, AuthorName = "author2" };
        var review3 = new Review() { Rating = 3, Content = "content3", CustomerId = customer3.Id, ProductId = product3.Id, AuthorName = "author3" };
        var review4 = new Review() { Rating = 4, Content = "content4", CustomerId = customer4.Id, ProductId = product4.Id, AuthorName = "author4" };
        var review5 = new Review() { Rating = 1, Content = "content5", CustomerId = customer1.Id, ProductId = product5.Id, AuthorName = "author1" };
        var review6 = new Review() { Rating = 2, Content = "content6", CustomerId = customer2.Id, ProductId = product6.Id, AuthorName = "author2" };
        var review7 = new Review() { Rating = 3, Content = "content7", CustomerId = customer3.Id, ProductId = product7.Id, AuthorName = "author3" };
        var review8 = new Review() { Rating = 4, Content = "content8", CustomerId = customer4.Id, ProductId = product8.Id, AuthorName = "author4" };
        var review9 = new Review() { Rating = 1, Content = "content9", CustomerId = customer1.Id, ProductId = product9.Id, AuthorName = "author1" };
        var review10 = new Review() { Rating = 2, Content = "content10", CustomerId = customer2.Id, ProductId = product10.Id, AuthorName = "author2" };
        var review11 = new Review() { Rating = 3, Content = "content11", CustomerId = customer3.Id, ProductId = product11.Id, AuthorName = "author3" };
        var review12 = new Review() { Rating = 4, Content = "content12", CustomerId = customer4.Id, ProductId = product12.Id, AuthorName = "author4" };
        var review13 = new Review() { Rating = 1, Content = "content13", CustomerId = customer1.Id, ProductId = product13.Id, AuthorName = "author1" };
        var review14 = new Review() { Rating = 2, Content = "content14", CustomerId = customer2.Id, ProductId = product14.Id, AuthorName = "author2" };
        var review15 = new Review() { Rating = 3, Content = "content15", CustomerId = customer3.Id, ProductId = product15.Id, AuthorName = "author3" };
        var review16 = new Review() { Rating = 4, Content = "content16", CustomerId = customer4.Id, ProductId = product16.Id, AuthorName = "author4" };
        var review17 = new Review() { Rating = 1, Content = "content17", CustomerId = customer1.Id, ProductId = product17.Id, AuthorName = "author1" };
        var review18 = new Review() { Rating = 2, Content = "content18", CustomerId = customer2.Id, ProductId = product18.Id, AuthorName = "author2" };
        var review19 = new Review() { Rating = 3, Content = "content19", CustomerId = customer3.Id, ProductId = product19.Id, AuthorName = "author3" };
        var review20 = new Review() { Rating = 4, Content = "content20", CustomerId = customer4.Id, ProductId = product20.Id, AuthorName = "author4" };
        var review21 = new Review() { Rating = 1, Content = "content21", CustomerId = customer1.Id, ProductId = product21.Id, AuthorName = "author1" };
        var review22 = new Review() { Rating = 2, Content = "content22", CustomerId = customer2.Id, ProductId = product22.Id, AuthorName = "author2" };
        var review23 = new Review() { Rating = 3, Content = "content23", CustomerId = customer3.Id, ProductId = product23.Id, AuthorName = "author3" };
        var review24 = new Review() { Rating = 4, Content = "content24", CustomerId = customer4.Id, ProductId = product24.Id, AuthorName = "author4" };
        var review25 = new Review() { Rating = 1, Content = "content25", CustomerId = customer1.Id, ProductId = product25.Id, AuthorName = "author1" };
        var review26 = new Review() { Rating = 2, Content = "content26", CustomerId = customer2.Id, ProductId = product26.Id, AuthorName = "author2" };
        var review27 = new Review() { Rating = 3, Content = "content27", CustomerId = customer3.Id, ProductId = product27.Id, AuthorName = "author3" };
        var review28 = new Review() { Rating = 4, Content = "content28", CustomerId = customer4.Id, ProductId = product28.Id, AuthorName = "author4" };
        var review29 = new Review() { Rating = 1, Content = "content29", CustomerId = customer1.Id, ProductId = product29.Id, AuthorName = "author1" };
        var review30 = new Review() { Rating = 2, Content = "content30", CustomerId = customer2.Id, ProductId = product30.Id, AuthorName = "author2" };
        var review31 = new Review() { Rating = 3, Content = "content31", CustomerId = customer3.Id, ProductId = product31.Id, AuthorName = "author3" };
        var review32 = new Review() { Rating = 4, Content = "content32", CustomerId = customer4.Id, ProductId = product32.Id, AuthorName = "author4" };
        var review33 = new Review() { Rating = 1, Content = "content33", CustomerId = customer1.Id, ProductId = product33.Id, AuthorName = "author1" };
        var review34 = new Review() { Rating = 2, Content = "content34", CustomerId = customer2.Id, ProductId = product34.Id, AuthorName = "author2" };
        var review35 = new Review() { Rating = 3, Content = "content35", CustomerId = customer3.Id, ProductId = product35.Id, AuthorName = "author3" };
        var review36 = new Review() { Rating = 4, Content = "content36", CustomerId = customer4.Id, ProductId = product36.Id, AuthorName = "author4" };
        var review37 = new Review() { Rating = 1, Content = "content37", CustomerId = customer1.Id, ProductId = product37.Id, AuthorName = "author1" };
        var review38 = new Review() { Rating = 2, Content = "content38", CustomerId = customer2.Id, ProductId = product38.Id, AuthorName = "author2" };
        var review39 = new Review() { Rating = 3, Content = "content39", CustomerId = customer3.Id, ProductId = product39.Id, AuthorName = "author3" };
        var review40 = new Review() { Rating = 4, Content = "content40", CustomerId = customer4.Id, ProductId = product40.Id, AuthorName = "author4" };
        var review41 = new Review() { Rating = 1, Content = "content41", CustomerId = customer1.Id, ProductId = product41.Id, AuthorName = "author1" };
        var review42 = new Review() { Rating = 2, Content = "content42", CustomerId = customer2.Id, ProductId = product42.Id, AuthorName = "author2" };
        var review43 = new Review() { Rating = 3, Content = "content43", CustomerId = customer3.Id, ProductId = product43.Id, AuthorName = "author3" };
        var review44 = new Review() { Rating = 4, Content = "content44", CustomerId = customer4.Id, ProductId = product44.Id, AuthorName = "author4" };
        var review45 = new Review() { Rating = 1, Content = "content45", CustomerId = customer1.Id, ProductId = product45.Id, AuthorName = "author1" };
        var review46 = new Review() { Rating = 2, Content = "content46", CustomerId = customer2.Id, ProductId = product46.Id, AuthorName = "author2" };
        var review47 = new Review() { Rating = 3, Content = "content47", CustomerId = customer3.Id, ProductId = product47.Id, AuthorName = "author3" };
        var review48 = new Review() { Rating = 4, Content = "content48", CustomerId = customer4.Id, ProductId = product48.Id, AuthorName = "author4" };
        var review49 = new Review() { Rating = 1, Content = "content49", CustomerId = customer1.Id, ProductId = product49.Id, AuthorName = "author1" };
        var review50 = new Review() { Rating = 2, Content = "content50", CustomerId = customer2.Id, ProductId = product50.Id, AuthorName = "author2" };
        var review51 = new Review() { Rating = 3, Content = "content51", CustomerId = customer3.Id, ProductId = product51.Id, AuthorName = "author3" };
        var review52 = new Review() { Rating = 4, Content = "content52", CustomerId = customer4.Id, ProductId = product52.Id, AuthorName = "author4" };
        var review53 = new Review() { Rating = 1, Content = "content53", CustomerId = customer1.Id, ProductId = product53.Id, AuthorName = "author1" };
        var review54 = new Review() { Rating = 2, Content = "content54", CustomerId = customer2.Id, ProductId = product54.Id, AuthorName = "author2" };
        var review55 = new Review() { Rating = 3, Content = "content55", CustomerId = customer3.Id, ProductId = product55.Id, AuthorName = "author3" };
        var review56 = new Review() { Rating = 4, Content = "content56", CustomerId = customer4.Id, ProductId = product56.Id, AuthorName = "author4" };
        var review57 = new Review() { Rating = 1, Content = "content57", CustomerId = customer1.Id, ProductId = product57.Id, AuthorName = "author1" };
        var review58 = new Review() { Rating = 2, Content = "content58", CustomerId = customer2.Id, ProductId = product58.Id, AuthorName = "author2" };
        var review59 = new Review() { Rating = 3, Content = "content59", CustomerId = customer3.Id, ProductId = product59.Id, AuthorName = "author3" };
        var review60 = new Review() { Rating = 4, Content = "content60", CustomerId = customer4.Id, ProductId = product60.Id, AuthorName = "author4" };
        var review61 = new Review() { Rating = 1, Content = "content61", CustomerId = customer1.Id, ProductId = product61.Id, AuthorName = "author1" };
        var review62 = new Review() { Rating = 2, Content = "content62", CustomerId = customer2.Id, ProductId = product62.Id, AuthorName = "author2" };
        var review63 = new Review() { Rating = 3, Content = "content63", CustomerId = customer3.Id, ProductId = product63.Id, AuthorName = "author3" };
        var review64 = new Review() { Rating = 4, Content = "content64", CustomerId = customer4.Id, ProductId = product64.Id, AuthorName = "author4" };
        var review65 = new Review() { Rating = 1, Content = "content65", CustomerId = customer1.Id, ProductId = product1.Id, AuthorName = "author1" };
        var review66 = new Review() { Rating = 2, Content = "content66", CustomerId = customer2.Id, ProductId = product2.Id, AuthorName = "author2" };
        var review67 = new Review() { Rating = 3, Content = "content67", CustomerId = customer3.Id, ProductId = product3.Id, AuthorName = "author3" };
        var review68 = new Review() { Rating = 4, Content = "content68", CustomerId = customer4.Id, ProductId = product4.Id, AuthorName = "author4" };
        var review69 = new Review() { Rating = 1, Content = "content69", CustomerId = customer1.Id, ProductId = product5.Id, AuthorName = "author1" };
        var review70 = new Review() { Rating = 2, Content = "content70", CustomerId = customer2.Id, ProductId = product6.Id, AuthorName = "author2" };
        var review71 = new Review() { Rating = 3, Content = "content71", CustomerId = customer3.Id, ProductId = product7.Id, AuthorName = "author3" };
        var review72 = new Review() { Rating = 4, Content = "content72", CustomerId = customer4.Id, ProductId = product8.Id, AuthorName = "author4" };
        var review73 = new Review() { Rating = 1, Content = "content73", CustomerId = customer1.Id, ProductId = product9.Id, AuthorName = "author1" };
        var review74 = new Review() { Rating = 2, Content = "content74", CustomerId = customer2.Id, ProductId = product10.Id, AuthorName = "author2" };
        var review75 = new Review() { Rating = 3, Content = "content75", CustomerId = customer3.Id, ProductId = product11.Id, AuthorName = "author3" };
        var review76 = new Review() { Rating = 4, Content = "content76", CustomerId = customer4.Id, ProductId = product12.Id, AuthorName = "author4" };
        var review77 = new Review() { Rating = 1, Content = "content77", CustomerId = customer1.Id, ProductId = product13.Id, AuthorName = "author1" };
        var review78 = new Review() { Rating = 2, Content = "content78", CustomerId = customer2.Id, ProductId = product14.Id, AuthorName = "author2" };
        var review79 = new Review() { Rating = 3, Content = "content79", CustomerId = customer3.Id, ProductId = product15.Id, AuthorName = "author3" };
        var review80 = new Review() { Rating = 4, Content = "content80", CustomerId = customer4.Id, ProductId = product16.Id, AuthorName = "author4" };
        var review81 = new Review() { Rating = 1, Content = "content81", CustomerId = customer1.Id, ProductId = product17.Id, AuthorName = "author1" };
        var review82 = new Review() { Rating = 2, Content = "content82", CustomerId = customer2.Id, ProductId = product18.Id, AuthorName = "author2" };
        var review83 = new Review() { Rating = 3, Content = "content83", CustomerId = customer3.Id, ProductId = product19.Id, AuthorName = "author3" };
        var review84 = new Review() { Rating = 4, Content = "content84", CustomerId = customer4.Id, ProductId = product20.Id, AuthorName = "author4" };
        var review85 = new Review() { Rating = 1, Content = "content85", CustomerId = customer1.Id, ProductId = product21.Id, AuthorName = "author1" };
        var review86 = new Review() { Rating = 2, Content = "content86", CustomerId = customer2.Id, ProductId = product22.Id, AuthorName = "author2" };
        var review87 = new Review() { Rating = 3, Content = "content87", CustomerId = customer3.Id, ProductId = product23.Id, AuthorName = "author3" };
        var review88 = new Review() { Rating = 4, Content = "content88", CustomerId = customer4.Id, ProductId = product24.Id, AuthorName = "author4" };
        var review89 = new Review() { Rating = 1, Content = "content89", CustomerId = customer1.Id, ProductId = product25.Id, AuthorName = "author1" };
        var review90 = new Review() { Rating = 2, Content = "content90", CustomerId = customer2.Id, ProductId = product26.Id, AuthorName = "author2" };
        var review91 = new Review() { Rating = 3, Content = "content91", CustomerId = customer3.Id, ProductId = product27.Id, AuthorName = "author3" };
        var review92 = new Review() { Rating = 4, Content = "content92", CustomerId = customer4.Id, ProductId = product28.Id, AuthorName = "author4" };
        var review93 = new Review() { Rating = 1, Content = "content93", CustomerId = customer1.Id, ProductId = product29.Id, AuthorName = "author1" };
        var review94 = new Review() { Rating = 2, Content = "content94", CustomerId = customer2.Id, ProductId = product30.Id, AuthorName = "author2" };
        var review95 = new Review() { Rating = 3, Content = "content95", CustomerId = customer3.Id, ProductId = product31.Id, AuthorName = "author3" };
        var review96 = new Review() { Rating = 4, Content = "content96", CustomerId = customer4.Id, ProductId = product32.Id, AuthorName = "author4" };
        var review97 = new Review() { Rating = 1, Content = "content97", CustomerId = customer1.Id, ProductId = product33.Id, AuthorName = "author1" };
        var review98 = new Review() { Rating = 2, Content = "content98", CustomerId = customer2.Id, ProductId = product34.Id, AuthorName = "author2" };
        var review99 = new Review() { Rating = 3, Content = "content99", CustomerId = customer3.Id, ProductId = product35.Id, AuthorName = "author3" };
        var review100 = new Review() { Rating = 4, Content = "content100", CustomerId = customer4.Id, ProductId = product36.Id, AuthorName = "author4" };
        var review101 = new Review() { Rating = 1, Content = "content101", CustomerId = customer1.Id, ProductId = product37.Id, AuthorName = "author1" };
        var review102 = new Review() { Rating = 2, Content = "content102", CustomerId = customer2.Id, ProductId = product38.Id, AuthorName = "author2" };
        var review103 = new Review() { Rating = 3, Content = "content103", CustomerId = customer3.Id, ProductId = product39.Id, AuthorName = "author3" };
        var review104 = new Review() { Rating = 4, Content = "content104", CustomerId = customer4.Id, ProductId = product40.Id, AuthorName = "author4" };
        var review105 = new Review() { Rating = 1, Content = "content105", CustomerId = customer1.Id, ProductId = product41.Id, AuthorName = "author1" };
        var review106 = new Review() { Rating = 2, Content = "content106", CustomerId = customer2.Id, ProductId = product42.Id, AuthorName = "author2" };
        var review107 = new Review() { Rating = 3, Content = "content107", CustomerId = customer3.Id, ProductId = product43.Id, AuthorName = "author3" };
        var review108 = new Review() { Rating = 4, Content = "content108", CustomerId = customer4.Id, ProductId = product44.Id, AuthorName = "author4" };
        var review109 = new Review() { Rating = 1, Content = "content109", CustomerId = customer1.Id, ProductId = product45.Id, AuthorName = "author1" };
        var review110 = new Review() { Rating = 2, Content = "content110", CustomerId = customer2.Id, ProductId = product46.Id, AuthorName = "author2" };
        var review111 = new Review() { Rating = 3, Content = "content111", CustomerId = customer3.Id, ProductId = product47.Id, AuthorName = "author3" };
        var review112 = new Review() { Rating = 4, Content = "content112", CustomerId = customer4.Id, ProductId = product48.Id, AuthorName = "author4" };
        var review113 = new Review() { Rating = 1, Content = "content113", CustomerId = customer1.Id, ProductId = product49.Id, AuthorName = "author1" };
        var review114 = new Review() { Rating = 2, Content = "content114", CustomerId = customer2.Id, ProductId = product50.Id, AuthorName = "author2" };
        var review115 = new Review() { Rating = 3, Content = "content115", CustomerId = customer3.Id, ProductId = product51.Id, AuthorName = "author3" };
        var review116 = new Review() { Rating = 4, Content = "content116", CustomerId = customer4.Id, ProductId = product52.Id, AuthorName = "author4" };
        var review117 = new Review() { Rating = 1, Content = "content117", CustomerId = customer1.Id, ProductId = product53.Id, AuthorName = "author1" };
        var review118 = new Review() { Rating = 2, Content = "content118", CustomerId = customer2.Id, ProductId = product54.Id, AuthorName = "author2" };
        var review119 = new Review() { Rating = 3, Content = "content119", CustomerId = customer3.Id, ProductId = product55.Id, AuthorName = "author3" };
        var review120 = new Review() { Rating = 4, Content = "content120", CustomerId = customer4.Id, ProductId = product56.Id, AuthorName = "author4" };
        var review121 = new Review() { Rating = 1, Content = "content121", CustomerId = customer1.Id, ProductId = product57.Id, AuthorName = "author1" };
        var review122 = new Review() { Rating = 2, Content = "content122", CustomerId = customer2.Id, ProductId = product58.Id, AuthorName = "author2" };
        var review123 = new Review() { Rating = 3, Content = "content123", CustomerId = customer3.Id, ProductId = product59.Id, AuthorName = "author3" };
        var review124 = new Review() { Rating = 4, Content = "content124", CustomerId = customer4.Id, ProductId = product60.Id, AuthorName = "author4" };
        var review125 = new Review() { Rating = 1, Content = "content125", CustomerId = customer1.Id, ProductId = product61.Id, AuthorName = "author1" };
        var review126 = new Review() { Rating = 2, Content = "content126", CustomerId = customer2.Id, ProductId = product62.Id, AuthorName = "author2" };
        var review127 = new Review() { Rating = 3, Content = "content127", CustomerId = customer3.Id, ProductId = product63.Id, AuthorName = "author3" };
        var review128 = new Review() { Rating = 4, Content = "content128", CustomerId = customer4.Id, ProductId = product64.Id, AuthorName = "author4" };


        if (!_context.Reviews.Any())
        {
            _context.Reviews.AddRange(new List<Review>()
            {
                review1,review2,review3,review4,review5,review6,review7,review8,review9,review10,review11,review12,review13,review14,review15,review16,
                review17,review18,review19,review20,review21,review22,review23,review24,review25,review26,review27,review28,review29,review30,review31,review32,
                review33,review34,review35,review36,review37,review38,review39,review40,review41,review42,review43,review44,review45,review46,review47,review48,
                review49,review50,review51,review52,review53,review54,review55,review56,review57,review58,review59,review60,review61,review62,review63,review64,
                review65,review66,review67,review68,review69,review70,review71,review72,review73,review74,review75,review76,review77,review78,review79,review80,
                review81,review82,review83,review84,review85,review86,review87,review88,review89,review90,review91,review92,review93,review94,review95,review96,
                review97,review98,review99,review100,review101,review102,review103,review104,review105,review106,review107,review108,review109,review110,review111,review112,
                review113,review114,review115,review116,review117,review118,review119,review120,review121,review122,review123,review124,review125,review126,review127,review128
            });
        }

        #endregion

        #region ProductAttribute

        var productAttribute1 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product1.Id
        };
        var productAttribute2 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product1.Id
        };
        var productAttribute3 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product1.Id
        };
        var productAttribute4 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product1.Id
        };
        var productAttribute5 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product1.Id
        };
        var productAttribute6 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product1.Id
        };
        var productAttribute7 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product2.Id
        };
        var productAttribute8 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product2.Id
        };
        var productAttribute9 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product2.Id
        };
        var productAttribute10 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product2.Id
        };
        var productAttribute11 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product2.Id
        };
        var productAttribute12 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product2.Id
        };
        var productAttribute13 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product3.Id
        };
        var productAttribute14 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product3.Id
        };
        var productAttribute15 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product3.Id
        };
        var productAttribute16 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product3.Id
        };
        var productAttribute17 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product3.Id
        };
        var productAttribute18 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product3.Id
        };
        var productAttribute19 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product4.Id
        };
        var productAttribute20 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product4.Id
        };
        var productAttribute21 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product4.Id
        };
        var productAttribute22 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product4.Id
        };
        var productAttribute23 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product4.Id
        };
        var productAttribute24 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product4.Id
        };
        var productAttribute25 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product5.Id
        };
        var productAttribute26 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product5.Id
        };
        var productAttribute27 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product5.Id
        };
        var productAttribute28 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product5.Id
        };
        var productAttribute29 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product5.Id
        };
        var productAttribute30 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product5.Id
        };
        var productAttribute31 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product6.Id
        };
        var productAttribute32 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product6.Id
        };
        var productAttribute33 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product6.Id
        };
        var productAttribute34 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product6.Id
        };
        var productAttribute35 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product6.Id
        };
        var productAttribute36 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product6.Id
        };
        var productAttribute37 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product7.Id
        };
        var productAttribute38 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product7.Id
        };
        var productAttribute39 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product7.Id
        };
        var productAttribute40 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product7.Id
        };
        var productAttribute41 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product7.Id
        };
        var productAttribute42 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product7.Id
        };
        var productAttribute43 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product8.Id
        };
        var productAttribute44 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product8.Id
        };
        var productAttribute45 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product8.Id
        };
        var productAttribute46 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product8.Id
        };
        var productAttribute47 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product8.Id
        };
        var productAttribute48 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product8.Id
        };
        var productAttribute49 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product9.Id
        };
        var productAttribute50 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product9.Id
        };
        var productAttribute51 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product9.Id
        };
        var productAttribute52 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product9.Id
        };
        var productAttribute53 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product9.Id
        };
        var productAttribute54 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product9.Id
        };
        var productAttribute55 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product10.Id
        };
        var productAttribute56 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product10.Id
        };
        var productAttribute57 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product10.Id
        };
        var productAttribute58 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product10.Id
        };
        var productAttribute59 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product10.Id
        };
        var productAttribute60 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product10.Id
        };
        var productAttribute61 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product11.Id
        };
        var productAttribute62 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product11.Id
        };
        var productAttribute63 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product11.Id
        };
        var productAttribute64 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product11.Id
        };
        var productAttribute65 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product11.Id
        };
        var productAttribute66 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product11.Id
        };
        var productAttribute67 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product12.Id
        };
        var productAttribute68 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product12.Id
        };
        var productAttribute69 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product12.Id
        };
        var productAttribute70 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product12.Id
        };
        var productAttribute71 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product12.Id
        };
        var productAttribute72 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product12.Id
        };
        var productAttribute73 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product13.Id
        };
        var productAttribute74 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product13.Id
        };
        var productAttribute75 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product13.Id
        };
        var productAttribute76 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product13.Id
        };
        var productAttribute77 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product13.Id
        };
        var productAttribute78 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product13.Id
        };
        var productAttribute79 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product14.Id
        };
        var productAttribute80 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product14.Id
        };
        var productAttribute81 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product14.Id
        };
        var productAttribute82 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product14.Id
        };
        var productAttribute83 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product14.Id
        };
        var productAttribute84 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product14.Id
        };
        var productAttribute85 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product15.Id
        };
        var productAttribute86 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product15.Id
        };
        var productAttribute87 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product15.Id
        };
        var productAttribute88 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product15.Id
        };
        var productAttribute89 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product15.Id
        };
        var productAttribute90 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product15.Id
        };
        var productAttribute91 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product16.Id
        };
        var productAttribute92 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product16.Id
        };
        var productAttribute93 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product16.Id
        };
        var productAttribute94 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product16.Id
        };
        var productAttribute95 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product16.Id
        };
        var productAttribute96 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product16.Id
        };
        var productAttribute97 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product17.Id
        };
        var productAttribute98 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product17.Id
        };
        var productAttribute99 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product17.Id
        };
        var productAttribute100 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product17.Id
        };
        var productAttribute101 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product17.Id
        };
        var productAttribute102 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product17.Id
        };
        var productAttribute103 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product18.Id
        };
        var productAttribute104 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product18.Id
        };
        var productAttribute105 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product18.Id
        };
        var productAttribute106 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product18.Id
        };
        var productAttribute107 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product18.Id
        };
        var productAttribute108 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product18.Id
        };
        var productAttribute109 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product19.Id
        };
        var productAttribute110 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product19.Id
        };
        var productAttribute111 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product19.Id
        };
        var productAttribute112 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product19.Id
        };
        var productAttribute113 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product19.Id
        };
        var productAttribute114 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product19.Id
        };
        var productAttribute115 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product20.Id
        };
        var productAttribute116 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product20.Id
        };
        var productAttribute117 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product20.Id
        };
        var productAttribute118 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product20.Id
        };
        var productAttribute119 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product20.Id
        };
        var productAttribute120 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product20.Id
        };
        var productAttribute121 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product21.Id
        };
        var productAttribute122 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product21.Id
        };
        var productAttribute123 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product21.Id
        };
        var productAttribute124 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product21.Id
        };
        var productAttribute125 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product21.Id
        };
        var productAttribute126 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product21.Id
        };
        var productAttribute127 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product22.Id
        };
        var productAttribute128 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product22.Id
        };
        var productAttribute129 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product22.Id
        };
        var productAttribute130 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product22.Id
        };
        var productAttribute131 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product22.Id
        };
        var productAttribute132 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product22.Id
        };
        var productAttribute133 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product23.Id
        };
        var productAttribute134 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product23.Id
        };
        var productAttribute135 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product23.Id
        };
        var productAttribute136 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product23.Id
        };
        var productAttribute137 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product23.Id
        };
        var productAttribute138 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product23.Id
        };
        var productAttribute139 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product24.Id
        };
        var productAttribute140 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product24.Id
        };
        var productAttribute141 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product24.Id
        };
        var productAttribute142 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product24.Id
        };
        var productAttribute143 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product24.Id
        };
        var productAttribute144 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product24.Id
        };
        var productAttribute145 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product25.Id
        };
        var productAttribute146 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product25.Id
        };
        var productAttribute147 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product25.Id
        };
        var productAttribute148 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product25.Id
        };
        var productAttribute149 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product25.Id
        };
        var productAttribute150 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product25.Id
        };
        var productAttribute151 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product26.Id
        };
        var productAttribute152 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product26.Id
        };
        var productAttribute153 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product26.Id
        };
        var productAttribute154 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product26.Id
        };
        var productAttribute155 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product26.Id
        };
        var productAttribute156 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product26.Id
        };
        var productAttribute157 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product27.Id
        };
        var productAttribute158 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product27.Id
        };
        var productAttribute159 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product27.Id
        };
        var productAttribute160 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product27.Id
        };
        var productAttribute161 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product27.Id
        };
        var productAttribute162 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product27.Id
        };
        var productAttribute163 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product28.Id
        };
        var productAttribute164 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product28.Id
        };
        var productAttribute165 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product28.Id
        };
        var productAttribute166 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product28.Id
        };
        var productAttribute167 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product28.Id
        };
        var productAttribute168 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product28.Id
        };
        var productAttribute169 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product29.Id
        };
        var productAttribute170 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product29.Id
        };
        var productAttribute171 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product29.Id
        };
        var productAttribute172 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product29.Id
        };
        var productAttribute173 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product29.Id
        };
        var productAttribute174 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product29.Id
        };
        var productAttribute175 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product30.Id
        };
        var productAttribute176 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product30.Id
        };
        var productAttribute177 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product30.Id
        };
        var productAttribute178 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product30.Id
        };
        var productAttribute179 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product30.Id
        };
        var productAttribute180 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product30.Id
        };
        var productAttribute181 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product31.Id
        };
        var productAttribute182 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product31.Id
        };
        var productAttribute183 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product31.Id
        };
        var productAttribute184 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product31.Id
        };
        var productAttribute185 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product31.Id
        };
        var productAttribute186 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product31.Id
        };
        var productAttribute187 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product32.Id
        };
        var productAttribute188 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product32.Id
        };
        var productAttribute189 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product32.Id
        };
        var productAttribute190 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product32.Id
        };
        var productAttribute191 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product32.Id
        };
        var productAttribute192 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product32.Id
        };
        var productAttribute193 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product33.Id
        };
        var productAttribute194 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product33.Id
        };
        var productAttribute195 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product33.Id
        };
        var productAttribute196 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product33.Id
        };
        var productAttribute197 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product33.Id
        };
        var productAttribute198 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product33.Id
        };
        var productAttribute199 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product34.Id
        };
        var productAttribute200 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product34.Id
        };
        var productAttribute201 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product34.Id
        };
        var productAttribute202 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product34.Id
        };
        var productAttribute203 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product34.Id
        };
        var productAttribute204 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product34.Id
        };
        var productAttribute205 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product35.Id
        };
        var productAttribute206 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product35.Id
        };
        var productAttribute207 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product35.Id
        };
        var productAttribute208 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product35.Id
        };
        var productAttribute209 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product35.Id
        };
        var productAttribute210 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product35.Id
        };
        var productAttribute211 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product36.Id
        };
        var productAttribute212 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product36.Id
        };
        var productAttribute213 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product36.Id
        };
        var productAttribute214 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product36.Id
        };
        var productAttribute215 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product36.Id
        };
        var productAttribute216 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product36.Id
        };
        var productAttribute217 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product37.Id
        };
        var productAttribute218 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product37.Id
        };
        var productAttribute219 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product37.Id
        };
        var productAttribute220 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product37.Id
        };
        var productAttribute221 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product37.Id
        };
        var productAttribute222 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product37.Id
        };
        var productAttribute223 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product38.Id
        };
        var productAttribute224 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product38.Id
        };
        var productAttribute225 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product38.Id
        };
        var productAttribute226 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product38.Id
        };
        var productAttribute227 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product38.Id
        };
        var productAttribute228 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product38.Id
        };
        var productAttribute229 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product39.Id
        };
        var productAttribute230 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product39.Id
        };
        var productAttribute231 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product39.Id
        };
        var productAttribute232 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product39.Id
        };
        var productAttribute233 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product39.Id
        };
        var productAttribute234 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product39.Id
        };
        var productAttribute235 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product40.Id
        };
        var productAttribute236 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product40.Id
        };
        var productAttribute237 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product40.Id
        };
        var productAttribute238 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product40.Id
        };
        var productAttribute239 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product40.Id
        };
        var productAttribute240 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product40.Id
        };
        var productAttribute241 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product41.Id
        };
        var productAttribute242 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product41.Id
        };
        var productAttribute243 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product41.Id
        };
        var productAttribute244 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product41.Id
        };
        var productAttribute245 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product41.Id
        };
        var productAttribute246 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product41.Id
        };
        var productAttribute247 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product42.Id
        };
        var productAttribute248 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product42.Id
        };
        var productAttribute249 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product42.Id
        };
        var productAttribute250 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product42.Id
        };
        var productAttribute251 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product42.Id
        };
        var productAttribute252 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product42.Id
        };
        var productAttribute253 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product43.Id
        };
        var productAttribute254 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product43.Id
        };
        var productAttribute255 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product43.Id
        };
        var productAttribute256 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product43.Id
        };
        var productAttribute257 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product43.Id
        };
        var productAttribute258 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product43.Id
        };
        var productAttribute259 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product44.Id
        };
        var productAttribute260 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product44.Id
        };
        var productAttribute261 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product44.Id
        };
        var productAttribute262 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product44.Id
        };
        var productAttribute263 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product44.Id
        };
        var productAttribute264 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product44.Id
        };
        var productAttribute265 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product45.Id
        };
        var productAttribute266 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product45.Id
        };
        var productAttribute267 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product45.Id
        };
        var productAttribute268 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product45.Id
        };
        var productAttribute269 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product45.Id
        };
        var productAttribute270 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product45.Id
        };
        var productAttribute271 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product46.Id
        };
        var productAttribute272 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product46.Id
        };
        var productAttribute273 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product46.Id
        };
        var productAttribute274 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product46.Id
        };
        var productAttribute275 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product46.Id
        };
        var productAttribute276 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product46.Id
        };
        var productAttribute277 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product47.Id
        };
        var productAttribute278 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product47.Id
        };
        var productAttribute279 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product47.Id
        };
        var productAttribute280 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product47.Id
        };
        var productAttribute281 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product47.Id
        };
        var productAttribute282 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product47.Id
        };
        var productAttribute283 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product48.Id
        };
        var productAttribute284 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product48.Id
        };
        var productAttribute285 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product48.Id
        };
        var productAttribute286 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product48.Id
        };
        var productAttribute287 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product48.Id
        };
        var productAttribute288 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product48.Id
        };
        var productAttribute289 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product49.Id
        };
        var productAttribute290 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product49.Id
        };
        var productAttribute291 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product49.Id
        };
        var productAttribute292 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product49.Id
        };
        var productAttribute293 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product49.Id
        };
        var productAttribute294 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product49.Id
        };
        var productAttribute295 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product50.Id
        };
        var productAttribute296 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product50.Id
        };
        var productAttribute297 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product50.Id
        };
        var productAttribute298 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product50.Id
        };
        var productAttribute299 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product50.Id
        };
        var productAttribute300 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product50.Id
        };
        var productAttribute301 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product51.Id
        };
        var productAttribute302 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product51.Id
        };
        var productAttribute303 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product51.Id
        };
        var productAttribute304 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product51.Id
        };
        var productAttribute305 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product51.Id
        };
        var productAttribute306 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product51.Id
        };
        var productAttribute307 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product52.Id
        };
        var productAttribute308 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product52.Id
        };
        var productAttribute309 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product52.Id
        };
        var productAttribute310 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product52.Id
        };
        var productAttribute311 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product52.Id
        };
        var productAttribute312 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product52.Id
        };
        var productAttribute313 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product53.Id
        };
        var productAttribute314 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product53.Id
        };
        var productAttribute315 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product53.Id
        };
        var productAttribute316 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product53.Id
        };
        var productAttribute317 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product53.Id
        };
        var productAttribute318 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product53.Id
        };
        var productAttribute319 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product54.Id
        };
        var productAttribute320 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product54.Id
        };
        var productAttribute321 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product54.Id
        };
        var productAttribute322 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product54.Id
        };
        var productAttribute323 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product54.Id
        };
        var productAttribute324 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product54.Id
        };
        var productAttribute325 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product55.Id
        };
        var productAttribute326 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product55.Id
        };
        var productAttribute327 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product55.Id
        };
        var productAttribute328 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product55.Id
        };
        var productAttribute329 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product55.Id
        };
        var productAttribute330 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product55.Id
        };
        var productAttribute331 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product56.Id
        };
        var productAttribute332 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product56.Id
        };
        var productAttribute333 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product56.Id
        };
        var productAttribute334 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product56.Id
        };
        var productAttribute335 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product56.Id
        };
        var productAttribute336 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product56.Id
        };
        var productAttribute337 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product57.Id
        };
        var productAttribute338 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product57.Id
        };
        var productAttribute339 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product57.Id
        };
        var productAttribute340 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product57.Id
        };
        var productAttribute341 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product57.Id
        };
        var productAttribute342 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product57.Id
        };
        var productAttribute343 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product58.Id
        };
        var productAttribute344 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product58.Id
        };
        var productAttribute345 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product58.Id
        };
        var productAttribute346 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product58.Id
        };
        var productAttribute347 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product58.Id
        };
        var productAttribute348 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product58.Id
        };
        var productAttribute349 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product59.Id
        };
        var productAttribute350 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product59.Id
        };
        var productAttribute351 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product59.Id
        };
        var productAttribute352 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product59.Id
        };
        var productAttribute353 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product59.Id
        };
        var productAttribute354 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product59.Id
        };
        var productAttribute355 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product60.Id
        };
        var productAttribute356 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product60.Id
        };
        var productAttribute357 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product60.Id
        };
        var productAttribute358 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product60.Id
        };
        var productAttribute359 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product60.Id
        };
        var productAttribute360 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product60.Id
        };
        var productAttribute361 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product61.Id
        };
        var productAttribute362 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product61.Id
        };
        var productAttribute363 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product61.Id
        };
        var productAttribute364 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product61.Id
        };
        var productAttribute365 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product61.Id
        };
        var productAttribute366 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product61.Id
        };
        var productAttribute367 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product62.Id
        };
        var productAttribute368 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product62.Id
        };
        var productAttribute369 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product62.Id
        };
        var productAttribute370 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product62.Id
        };
        var productAttribute371 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product62.Id
        };
        var productAttribute372 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product62.Id
        };
        var productAttribute373 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product63.Id
        };
        var productAttribute374 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product63.Id
        };
        var productAttribute375 = new ProductAttribute()
        {
            Name = "height",
            Slug = "height",
            Featured = false,
            ValueName = "208 mm",
            ValueSlug = "208 mm",
            ProductId = product63.Id
        };
        var productAttribute376 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product63.Id
        };
        var productAttribute377 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product63.Id
        };
        var productAttribute378 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product63.Id
        };
        var productAttribute379 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product64.Id
        };
        var productAttribute380 = new ProductAttribute()
        {
            Name = "speed",
            Slug = "speed",
            Featured = true,
            ValueName = "750 RPM",
            ValueSlug = "750 RPM",
            ProductId = product64.Id
        };
        var productAttribute381 = new ProductAttribute()
        {
            Name = "voltage",
            Slug = "voltage",
            Featured = true,
            ValueName = "20 Volts",
            ValueSlug = "20 Volts",
            ProductId = product64.Id
        };
        var productAttribute382 = new ProductAttribute()
        {
            Name = "power-source",
            Slug = "power-source",
            Featured = true,
            ValueName = "Cordless-Electric",
            ValueSlug = "Cordless-Electric",
            ProductId = product64.Id
        };
        var productAttribute383 = new ProductAttribute()
        {
            Name = "length",
            Slug = "length",
            Featured = false,
            ValueName = "99 mm",
            ValueSlug = "99 mm",
            ProductId = product64.Id
        };
        var productAttribute384 = new ProductAttribute()
        {
            Name = "width",
            Slug = "width",
            Featured = false,
            ValueName = "207 mm",
            ValueSlug = "207 mm",
            ProductId = product64.Id
        };
        //For Options
        //var productAttribute385 = new ProductAttribute()
        //{
        //    Name = productOptionColor1.Name,
        //    Slug = productOptionColor1.Slug,
        //    Featured = true,
        //  
        //   Slug Values = new List<ProductAttributeValue>()
        //    {
        //        new ProductAttributeValue() { Name = productOptionValueColor1.Name, Slug = productOptionValueColor1.Slug },
        //        new ProductAttributeValue() { Name = productOptionValueColor2.Name, Slug = productOptionValueColor2.Slug },
        //        new ProductAttributeValue() { Name = productOptionValueColor3.Name, Slug = productOptionValueColor3.Slug },
        //        new ProductAttributeValue() { Name = productOptionValueColor4.Name, Slug = productOptionValueColor4.Slug }
        //    },
        //    ProductId = product1.Id
        //};
        //var productAttribute386 = new ProductAttribute()
        //{
        //    Name = productOptionMaterial1.Name,
        //    Slug = productOptionMaterial1.Slug,
        //    Featured = true,
        //  
        //   Slug Values = new List<ProductAttributeValue>()
        //    {
        //        new ProductAttributeValue() { Name = productOptionValueMaterial1.Name, Slug = productOptionValueMaterial1.Slug },
        //        new ProductAttributeValue() { Name = productOptionValueMaterial2.Name, Slug = productOptionValueMaterial2.Slug },
        //        new ProductAttributeValue() { Name = productOptionValueMaterial3.Name, Slug = productOptionValueMaterial3.Slug }

        //    },
        //    ProductId = product1.Id
        //};
        var productAttribute387 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product2.Id
        };
        var productAttribute388 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product2.Id
        };
        var productAttribute389 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product3.Id
        };
        var productAttribute390 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product3.Id
        };
        var productAttribute391 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product4.Id
        };
        var productAttribute392 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product4.Id
        };
        var productAttribute393 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product5.Id
        };
        var productAttribute394 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product5.Id
        };
        var productAttribute395 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product6.Id
        };
        var productAttribute396 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product6.Id
        };
        var productAttribute397 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product7.Id
        };
        var productAttribute398 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product7.Id
        };
        var productAttribute399 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product8.Id
        };
        var productAttribute400 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product8.Id
        };
        var productAttribute401 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product9.Id
        };
        var productAttribute402 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product9.Id
        };
        var productAttribute403 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product10.Id
        };
        var productAttribute404 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product10.Id
        };
        var productAttribute405 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product11.Id
        };
        var productAttribute406 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product11.Id
        };
        var productAttribute407 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product12.Id
        };
        var productAttribute408 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product12.Id
        };
        var productAttribute409 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product13.Id
        };
        var productAttribute410 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product13.Id
        };
        var productAttribute411 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product14.Id
        };
        var productAttribute412 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product14.Id
        };
        var productAttribute413 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product15.Id
        };
        var productAttribute414 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product15.Id
        };
        var productAttribute415 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product16.Id
        };
        var productAttribute416 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product16.Id
        };
        var productAttribute417 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product17.Id
        };
        var productAttribute418 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product17.Id
        };
        var productAttribute419 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product18.Id
        };
        var productAttribute420 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product18.Id
        };
        var productAttribute421 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product19.Id
        };
        var productAttribute422 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product19.Id
        };
        var productAttribute423 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product20.Id
        };
        var productAttribute424 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product20.Id
        };
        var productAttribute425 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product21.Id
        };
        var productAttribute426 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product21.Id
        };
        var productAttribute427 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product22.Id
        };
        var productAttribute428 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product22.Id
        };
        var productAttribute429 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product23.Id
        };
        var productAttribute430 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product23.Id
        };
        var productAttribute431 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product24.Id
        };
        var productAttribute432 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product24.Id
        };
        var productAttribute433 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product25.Id
        };
        var productAttribute434 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product25.Id
        };
        var productAttribute435 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product26.Id
        };
        var productAttribute436 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product26.Id
        };
        var productAttribute437 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product27.Id
        };
        var productAttribute438 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product27.Id
        };
        var productAttribute439 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product28.Id
        };
        var productAttribute440 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product28.Id
        };
        var productAttribute441 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product29.Id
        };
        var productAttribute442 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product29.Id
        };
        var productAttribute443 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product30.Id
        };
        var productAttribute444 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product30.Id
        };
        var productAttribute445 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product31.Id
        };
        var productAttribute446 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product31.Id
        };
        var productAttribute447 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product32.Id
        };
        var productAttribute448 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product32.Id
        };
        var productAttribute449 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product33.Id
        };
        var productAttribute450 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product33.Id
        };
        var productAttribute451 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product34.Id
        };
        var productAttribute452 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product34.Id
        };
        var productAttribute453 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product35.Id
        };
        var productAttribute454 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product35.Id
        };
        var productAttribute455 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product36.Id
        };
        var productAttribute456 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product36.Id
        };
        var productAttribute457 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product37.Id
        };
        var productAttribute458 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product37.Id
        };
        var productAttribute459 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product38.Id
        };
        var productAttribute460 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product38.Id
        };
        var productAttribute461 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product39.Id
        };
        var productAttribute462 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product39.Id
        };
        var productAttribute463 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product41.Id
        };
        var productAttribute464 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product41.Id
        };
        var productAttribute465 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product42.Id
        };
        var productAttribute466 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product42.Id
        };
        var productAttribute467 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product43.Id
        };
        var productAttribute468 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product43.Id
        };
        var productAttribute469 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product44.Id
        };
        var productAttribute470 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product44.Id
        };
        var productAttribute471 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product45.Id
        };
        var productAttribute472 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product45.Id
        };
        var productAttribute473 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product46.Id
        };
        var productAttribute474 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product46.Id
        };
        var productAttribute475 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product47.Id
        };
        var productAttribute476 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product47.Id
        };
        var productAttribute477 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product48.Id
        };
        var productAttribute478 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product48.Id
        };
        var productAttribute479 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product49.Id
        };
        var productAttribute480 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product49.Id
        };
        var productAttribute481 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product50.Id
        };
        var productAttribute482 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product50.Id
        };
        var productAttribute483 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product51.Id
        };
        var productAttribute484 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product51.Id
        };
        var productAttribute485 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product52.Id
        };
        var productAttribute486 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product52.Id
        };
        var productAttribute487 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product53.Id
        };
        var productAttribute488 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product53.Id
        };
        var productAttribute489 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product54.Id
        };
        var productAttribute490 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product54.Id
        };
        var productAttribute491 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product55.Id
        };
        var productAttribute492 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product55.Id
        };
        var productAttribute493 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product56.Id
        };
        var productAttribute494 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product56.Id
        };
        var productAttribute495 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product57.Id
        };
        var productAttribute496 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product57.Id
        };
        var productAttribute497 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product58.Id
        };
        var productAttribute498 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product58.Id
        };
        var productAttribute499 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product59.Id
        };
        var productAttribute500 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product59.Id
        };
        var productAttribute501 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product60.Id
        };
        var productAttribute502 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product60.Id
        };
        var productAttribute503 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product61.Id
        };
        var productAttribute504 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product61.Id
        };
        var productAttribute505 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product62.Id
        };
        var productAttribute506 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product62.Id
        };
        var productAttribute507 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product63.Id
        };
        var productAttribute508 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product63.Id
        };
        var productAttribute509 = new ProductAttribute()
        {
            Name = productOptionColor1.Name,
            Slug = productOptionColor1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product64.Id
        };
        var productAttribute510 = new ProductAttribute()
        {
            Name = productOptionMaterial1.Name,
            Slug = productOptionMaterial1.Slug,
            Featured = true,
            ValueName = "",
            ValueSlug = "",
            ProductId = product64.Id
        };


        if (!_context.ProductAttributes.Any())
        {
            _context.ProductAttributes.AddRange(new List<ProductAttribute>()
            {
                productAttribute1,productAttribute2,productAttribute3,productAttribute4,productAttribute5,productAttribute6,productAttribute7,productAttribute8,productAttribute9,productAttribute10,
                productAttribute11,productAttribute12,productAttribute13,productAttribute14,productAttribute15,productAttribute16,productAttribute17,productAttribute18,productAttribute19,productAttribute20,
                productAttribute21,productAttribute22,productAttribute23,productAttribute24,productAttribute25,productAttribute26,productAttribute27,productAttribute28,productAttribute29,productAttribute30,
                productAttribute31,productAttribute32,productAttribute33,productAttribute34,productAttribute35,productAttribute36,productAttribute37,productAttribute38,productAttribute39,productAttribute40,
                productAttribute41,productAttribute42,productAttribute43,productAttribute44,productAttribute45,productAttribute46,productAttribute47,productAttribute48,productAttribute49,productAttribute50,
                productAttribute51,productAttribute52,productAttribute53,productAttribute54,productAttribute55,productAttribute56,productAttribute57,productAttribute58,productAttribute59,productAttribute60,
                productAttribute61,productAttribute62,productAttribute63,productAttribute64,productAttribute65,productAttribute66,productAttribute67,productAttribute68,productAttribute69,productAttribute70,
                productAttribute71,productAttribute72,productAttribute73,productAttribute74,productAttribute75,productAttribute76,productAttribute77,productAttribute78,productAttribute79,productAttribute80,
                productAttribute81,productAttribute82,productAttribute83,productAttribute84,productAttribute85,productAttribute86,productAttribute87,productAttribute88,productAttribute89,productAttribute90,
                productAttribute91,productAttribute92,productAttribute93,productAttribute94,productAttribute95,productAttribute96,productAttribute97,productAttribute98,productAttribute99,productAttribute100,
                productAttribute101,productAttribute102,productAttribute103,productAttribute104,productAttribute105,productAttribute106,productAttribute107,productAttribute108,productAttribute109,productAttribute110,
                productAttribute111,productAttribute112,productAttribute113,productAttribute114,productAttribute115,productAttribute116,productAttribute117,productAttribute118,productAttribute119,productAttribute120,
                productAttribute121,productAttribute122,productAttribute123,productAttribute124,productAttribute125,productAttribute126,productAttribute127,productAttribute128,productAttribute129,productAttribute130,
                productAttribute131,productAttribute132,productAttribute133,productAttribute134,productAttribute135,productAttribute136,productAttribute137,productAttribute138,productAttribute139,productAttribute140,
                productAttribute141,productAttribute142,productAttribute143,productAttribute144,productAttribute145,productAttribute146,productAttribute147,productAttribute148,productAttribute149,productAttribute150,
                productAttribute151,productAttribute152,productAttribute153,productAttribute154,productAttribute155,productAttribute156,productAttribute157,productAttribute158,productAttribute159,productAttribute160,
                productAttribute161,productAttribute162,productAttribute163,productAttribute164,productAttribute165,productAttribute166,productAttribute167,productAttribute168,productAttribute169,productAttribute170,
                productAttribute171,productAttribute172,productAttribute173,productAttribute174,productAttribute175,productAttribute176,productAttribute177,productAttribute178,productAttribute179,productAttribute180,
                productAttribute181,productAttribute182,productAttribute183,productAttribute184,productAttribute185,productAttribute186,productAttribute187,productAttribute188,productAttribute189,productAttribute190,
                productAttribute191,productAttribute192,productAttribute193,productAttribute194,productAttribute195,productAttribute196,productAttribute197,productAttribute198,productAttribute199,productAttribute200,
                productAttribute201,productAttribute202,productAttribute203,productAttribute204,productAttribute205,productAttribute206,productAttribute207,productAttribute208,productAttribute209,productAttribute210,
                productAttribute211,productAttribute212,productAttribute213,productAttribute214,productAttribute215,productAttribute216,productAttribute217,productAttribute218,productAttribute219,productAttribute220,
                productAttribute221,productAttribute222,productAttribute223,productAttribute224,productAttribute225,productAttribute226,productAttribute227,productAttribute228,productAttribute229,productAttribute230,
                productAttribute231,productAttribute232,productAttribute233,productAttribute234,productAttribute235,productAttribute236,productAttribute237,productAttribute238,productAttribute239,productAttribute240,
                productAttribute241,productAttribute242,productAttribute243,productAttribute244,productAttribute245,productAttribute246,productAttribute247,productAttribute248,productAttribute249,productAttribute250,
                productAttribute251,productAttribute252,productAttribute253,productAttribute254,productAttribute255,productAttribute256,productAttribute257,productAttribute258,productAttribute259,productAttribute260,
                productAttribute261,productAttribute262,productAttribute263,productAttribute264,productAttribute265,productAttribute266,productAttribute267,productAttribute268,productAttribute269,productAttribute270,
                productAttribute271,productAttribute272,productAttribute273,productAttribute274,productAttribute275,productAttribute276,productAttribute277,productAttribute278,productAttribute279,productAttribute280,
                productAttribute281,productAttribute282,productAttribute283,productAttribute284,productAttribute285,productAttribute286,productAttribute287,productAttribute288,productAttribute289,productAttribute290,
                productAttribute291,productAttribute292,productAttribute293,productAttribute294,productAttribute295,productAttribute296,productAttribute297,productAttribute298,productAttribute299,productAttribute300,
                productAttribute301,productAttribute302,productAttribute303,productAttribute304,productAttribute305,productAttribute306,productAttribute307,productAttribute308,productAttribute309,productAttribute310,
                productAttribute311,productAttribute312,productAttribute313,productAttribute314,productAttribute315,productAttribute316,productAttribute317,productAttribute318,productAttribute319,productAttribute320,
                productAttribute321,productAttribute322,productAttribute323,productAttribute324,productAttribute325,productAttribute326,productAttribute327,productAttribute328,productAttribute329,productAttribute330,
                productAttribute331,productAttribute332,productAttribute333,productAttribute334,productAttribute335,productAttribute336,productAttribute337,productAttribute338,productAttribute339,productAttribute340,
                productAttribute341,productAttribute342,productAttribute343,productAttribute344,productAttribute345,productAttribute346,productAttribute347,productAttribute348,productAttribute349,productAttribute350,
                productAttribute351,productAttribute352,productAttribute353,productAttribute354,productAttribute355,productAttribute356,productAttribute357,productAttribute358,productAttribute359,productAttribute360,
                productAttribute361,productAttribute362,productAttribute363,productAttribute364,productAttribute365,productAttribute366,productAttribute367,productAttribute368,productAttribute369,productAttribute370,
                productAttribute371,productAttribute372,productAttribute373,productAttribute374,productAttribute375,productAttribute376,productAttribute377,productAttribute378,productAttribute379,productAttribute380,
                productAttribute381,productAttribute382,productAttribute383,productAttribute384,/*productAttribute385,productAttribute386,*/productAttribute387,productAttribute388,productAttribute389,productAttribute390,
                productAttribute391,productAttribute392,productAttribute393,productAttribute394,productAttribute395,productAttribute396,productAttribute397,productAttribute398,productAttribute399,productAttribute400,
                productAttribute401,productAttribute402,productAttribute403,productAttribute404,productAttribute405,productAttribute406,productAttribute407,productAttribute408,productAttribute409,productAttribute410,
                productAttribute411,productAttribute412,productAttribute413,productAttribute414,productAttribute415,productAttribute416,productAttribute417,productAttribute418,productAttribute419,productAttribute420,
                productAttribute421,productAttribute422,productAttribute423,productAttribute424,productAttribute425,productAttribute426,productAttribute427,productAttribute428,productAttribute429,productAttribute430,
                productAttribute431,productAttribute432,productAttribute433,productAttribute434,productAttribute435,productAttribute436,productAttribute437,productAttribute438,productAttribute439,productAttribute440,
                productAttribute441,productAttribute442,productAttribute443,productAttribute444,productAttribute445,productAttribute446,productAttribute447,productAttribute448,productAttribute449,productAttribute450,
                productAttribute451,productAttribute452,productAttribute453,productAttribute454,productAttribute455,productAttribute456,productAttribute457,productAttribute458,productAttribute459,productAttribute460,
                productAttribute461,productAttribute462,productAttribute463,productAttribute464,productAttribute465,productAttribute466,productAttribute467,productAttribute468,productAttribute469,productAttribute470,
                productAttribute471,productAttribute472,productAttribute473,productAttribute474,productAttribute475,productAttribute476,productAttribute477,productAttribute478,productAttribute479,productAttribute480,
                productAttribute481,productAttribute482,productAttribute483,productAttribute484,productAttribute485,productAttribute486,productAttribute487,productAttribute488,productAttribute489,productAttribute490,
                productAttribute491,productAttribute492,productAttribute493,productAttribute494,productAttribute495,productAttribute496,productAttribute497,productAttribute498,productAttribute499,productAttribute500,
                productAttribute501,productAttribute502,productAttribute503,productAttribute504,productAttribute505,productAttribute506,productAttribute507,productAttribute508,productAttribute509,productAttribute510
            });
        }
        #endregion
        #region ProductKind
        var productKind1 = new ProductKind() { KindId = kind1.Id, ProductId = product1.Id };
        var productKind2 = new ProductKind() { KindId = kind1.Id, ProductId = product2.Id };
        var productKind3 = new ProductKind() { KindId = kind2.Id, ProductId = product3.Id };
        var productKind4 = new ProductKind() { KindId = kind2.Id, ProductId = product4.Id };
        var productKind5 = new ProductKind() { KindId = kind3.Id, ProductId = product5.Id };
        var productKind6 = new ProductKind() { KindId = kind3.Id, ProductId = product6.Id };
        var productKind7 = new ProductKind() { KindId = kind4.Id, ProductId = product7.Id };
        var productKind8 = new ProductKind() { KindId = kind4.Id, ProductId = product8.Id };
        var productKind9 = new ProductKind() { KindId = kind5.Id, ProductId = product9.Id };
        var productKind10 = new ProductKind() { KindId = kind5.Id, ProductId = product10.Id };
        var productKind11 = new ProductKind() { KindId = kind6.Id, ProductId = product11.Id };
        var productKind12 = new ProductKind() { KindId = kind6.Id, ProductId = product12.Id };
        var productKind13 = new ProductKind() { KindId = kind7.Id, ProductId = product13.Id };
        var productKind14 = new ProductKind() { KindId = kind7.Id, ProductId = product14.Id };
        var productKind15 = new ProductKind() { KindId = kind8.Id, ProductId = product15.Id };
        var productKind16 = new ProductKind() { KindId = kind8.Id, ProductId = product16.Id };
        var productKind17 = new ProductKind() { KindId = kind9.Id, ProductId = product17.Id };
        var productKind18 = new ProductKind() { KindId = kind9.Id, ProductId = product18.Id };
        var productKind19 = new ProductKind() { KindId = kind10.Id, ProductId = product19.Id };
        var productKind20 = new ProductKind() { KindId = kind10.Id, ProductId = product20.Id };
        var productKind21 = new ProductKind() { KindId = kind11.Id, ProductId = product21.Id };
        var productKind22 = new ProductKind() { KindId = kind11.Id, ProductId = product22.Id };
        var productKind23 = new ProductKind() { KindId = kind12.Id, ProductId = product23.Id };
        var productKind24 = new ProductKind() { KindId = kind12.Id, ProductId = product24.Id };
        var productKind25 = new ProductKind() { KindId = kind13.Id, ProductId = product25.Id };
        var productKind26 = new ProductKind() { KindId = kind13.Id, ProductId = product26.Id };
        var productKind27 = new ProductKind() { KindId = kind14.Id, ProductId = product27.Id };
        var productKind28 = new ProductKind() { KindId = kind14.Id, ProductId = product28.Id };
        var productKind29 = new ProductKind() { KindId = kind15.Id, ProductId = product29.Id };
        var productKind30 = new ProductKind() { KindId = kind15.Id, ProductId = product30.Id };
        var productKind31 = new ProductKind() { KindId = kind16.Id, ProductId = product31.Id };
        var productKind32 = new ProductKind() { KindId = kind16.Id, ProductId = product32.Id };
        var productKind33 = new ProductKind() { KindId = kind17.Id, ProductId = product33.Id };
        var productKind34 = new ProductKind() { KindId = kind17.Id, ProductId = product34.Id };
        var productKind35 = new ProductKind() { KindId = kind18.Id, ProductId = product35.Id };
        var productKind36 = new ProductKind() { KindId = kind18.Id, ProductId = product36.Id };
        var productKind37 = new ProductKind() { KindId = kind19.Id, ProductId = product37.Id };
        var productKind38 = new ProductKind() { KindId = kind19.Id, ProductId = product38.Id };
        var productKind39 = new ProductKind() { KindId = kind20.Id, ProductId = product39.Id };
        var productKind40 = new ProductKind() { KindId = kind20.Id, ProductId = product40.Id };
        var productKind41 = new ProductKind() { KindId = kind21.Id, ProductId = product41.Id };
        var productKind42 = new ProductKind() { KindId = kind21.Id, ProductId = product42.Id };
        var productKind43 = new ProductKind() { KindId = kind22.Id, ProductId = product43.Id };
        var productKind44 = new ProductKind() { KindId = kind22.Id, ProductId = product44.Id };
        var productKind45 = new ProductKind() { KindId = kind23.Id, ProductId = product45.Id };
        var productKind46 = new ProductKind() { KindId = kind23.Id, ProductId = product46.Id };
        var productKind47 = new ProductKind() { KindId = kind24.Id, ProductId = product47.Id };
        var productKind48 = new ProductKind() { KindId = kind24.Id, ProductId = product48.Id };
        var productKind49 = new ProductKind() { KindId = kind25.Id, ProductId = product49.Id };
        var productKind50 = new ProductKind() { KindId = kind25.Id, ProductId = product50.Id };
        var productKind51 = new ProductKind() { KindId = kind26.Id, ProductId = product51.Id };
        var productKind52 = new ProductKind() { KindId = kind26.Id, ProductId = product52.Id };
        var productKind53 = new ProductKind() { KindId = kind27.Id, ProductId = product53.Id };
        var productKind54 = new ProductKind() { KindId = kind27.Id, ProductId = product54.Id };
        var productKind55 = new ProductKind() { KindId = kind28.Id, ProductId = product55.Id };
        var productKind56 = new ProductKind() { KindId = kind28.Id, ProductId = product56.Id };
        var productKind57 = new ProductKind() { KindId = kind29.Id, ProductId = product57.Id };
        var productKind58 = new ProductKind() { KindId = kind29.Id, ProductId = product58.Id };
        var productKind59 = new ProductKind() { KindId = kind30.Id, ProductId = product59.Id };
        var productKind60 = new ProductKind() { KindId = kind30.Id, ProductId = product60.Id };
        var productKind61 = new ProductKind() { KindId = kind31.Id, ProductId = product61.Id };
        var productKind62 = new ProductKind() { KindId = kind31.Id, ProductId = product62.Id };
        var productKind63 = new ProductKind() { KindId = kind32.Id, ProductId = product63.Id };
        var productKind64 = new ProductKind() { KindId = kind32.Id, ProductId = product64.Id };
        var productKind65 = new ProductKind() { KindId = kind32.Id, ProductId = product1.Id };
        var productKind66 = new ProductKind() { KindId = kind31.Id, ProductId = product2.Id };
        var productKind67 = new ProductKind() { KindId = kind30.Id, ProductId = product3.Id };
        var productKind68 = new ProductKind() { KindId = kind29.Id, ProductId = product4.Id };
        var productKind69 = new ProductKind() { KindId = kind28.Id, ProductId = product5.Id };
        var productKind70 = new ProductKind() { KindId = kind27.Id, ProductId = product6.Id };
        var productKind71 = new ProductKind() { KindId = kind26.Id, ProductId = product7.Id };
        var productKind72 = new ProductKind() { KindId = kind25.Id, ProductId = product8.Id };
        var productKind73 = new ProductKind() { KindId = kind24.Id, ProductId = product9.Id };
        var productKind74 = new ProductKind() { KindId = kind23.Id, ProductId = product10.Id };
        var productKind75 = new ProductKind() { KindId = kind22.Id, ProductId = product11.Id };
        var productKind76 = new ProductKind() { KindId = kind21.Id, ProductId = product12.Id };
        var productKind77 = new ProductKind() { KindId = kind20.Id, ProductId = product13.Id };
        var productKind78 = new ProductKind() { KindId = kind19.Id, ProductId = product14.Id };
        var productKind79 = new ProductKind() { KindId = kind18.Id, ProductId = product15.Id };
        var productKind80 = new ProductKind() { KindId = kind17.Id, ProductId = product16.Id };
        var productKind81 = new ProductKind() { KindId = kind16.Id, ProductId = product17.Id };
        var productKind82 = new ProductKind() { KindId = kind15.Id, ProductId = product18.Id };
        var productKind83 = new ProductKind() { KindId = kind14.Id, ProductId = product19.Id };
        var productKind84 = new ProductKind() { KindId = kind13.Id, ProductId = product20.Id };
        var productKind85 = new ProductKind() { KindId = kind12.Id, ProductId = product21.Id };
        var productKind86 = new ProductKind() { KindId = kind11.Id, ProductId = product22.Id };
        var productKind87 = new ProductKind() { KindId = kind10.Id, ProductId = product23.Id };
        var productKind88 = new ProductKind() { KindId = kind9.Id, ProductId = product24.Id };
        var productKind89 = new ProductKind() { KindId = kind8.Id, ProductId = product25.Id };
        var productKind90 = new ProductKind() { KindId = kind7.Id, ProductId = product26.Id };
        var productKind91 = new ProductKind() { KindId = kind6.Id, ProductId = product27.Id };
        var productKind92 = new ProductKind() { KindId = kind5.Id, ProductId = product28.Id };
        var productKind93 = new ProductKind() { KindId = kind4.Id, ProductId = product29.Id };
        var productKind94 = new ProductKind() { KindId = kind3.Id, ProductId = product30.Id };
        var productKind95 = new ProductKind() { KindId = kind2.Id, ProductId = product31.Id };
        var productKind96 = new ProductKind() { KindId = kind1.Id, ProductId = product32.Id };
        var productKind97 = new ProductKind() { KindId = kind32.Id, ProductId = product33.Id };
        var productKind98 = new ProductKind() { KindId = kind31.Id, ProductId = product34.Id };
        var productKind99 = new ProductKind() { KindId = kind30.Id, ProductId = product35.Id };
        var productKind100 = new ProductKind() { KindId = kind29.Id, ProductId = product36.Id };
        var productKind101 = new ProductKind() { KindId = kind28.Id, ProductId = product37.Id };
        var productKind102 = new ProductKind() { KindId = kind27.Id, ProductId = product38.Id };
        var productKind103 = new ProductKind() { KindId = kind26.Id, ProductId = product39.Id };
        var productKind104 = new ProductKind() { KindId = kind25.Id, ProductId = product40.Id };
        var productKind105 = new ProductKind() { KindId = kind24.Id, ProductId = product41.Id };
        var productKind106 = new ProductKind() { KindId = kind23.Id, ProductId = product42.Id };
        var productKind107 = new ProductKind() { KindId = kind22.Id, ProductId = product43.Id };
        var productKind108 = new ProductKind() { KindId = kind21.Id, ProductId = product44.Id };
        var productKind109 = new ProductKind() { KindId = kind20.Id, ProductId = product45.Id };
        var productKind110 = new ProductKind() { KindId = kind19.Id, ProductId = product46.Id };
        var productKind111 = new ProductKind() { KindId = kind18.Id, ProductId = product47.Id };
        var productKind112 = new ProductKind() { KindId = kind17.Id, ProductId = product48.Id };
        var productKind113 = new ProductKind() { KindId = kind16.Id, ProductId = product49.Id };
        var productKind114 = new ProductKind() { KindId = kind15.Id, ProductId = product50.Id };
        var productKind115 = new ProductKind() { KindId = kind14.Id, ProductId = product51.Id };
        var productKind116 = new ProductKind() { KindId = kind13.Id, ProductId = product52.Id };
        var productKind117 = new ProductKind() { KindId = kind12.Id, ProductId = product53.Id };
        var productKind118 = new ProductKind() { KindId = kind11.Id, ProductId = product54.Id };
        var productKind119 = new ProductKind() { KindId = kind10.Id, ProductId = product55.Id };
        var productKind120 = new ProductKind() { KindId = kind9.Id, ProductId = product56.Id };
        var productKind121 = new ProductKind() { KindId = kind8.Id, ProductId = product57.Id };
        var productKind122 = new ProductKind() { KindId = kind7.Id, ProductId = product58.Id };
        var productKind123 = new ProductKind() { KindId = kind6.Id, ProductId = product59.Id };
        var productKind124 = new ProductKind() { KindId = kind5.Id, ProductId = product60.Id };
        var productKind125 = new ProductKind() { KindId = kind4.Id, ProductId = product61.Id };
        var productKind126 = new ProductKind() { KindId = kind3.Id, ProductId = product62.Id };
        var productKind127 = new ProductKind() { KindId = kind2.Id, ProductId = product63.Id };
        var productKind128 = new ProductKind() { KindId = kind1.Id, ProductId = product64.Id };

        if (!_context.ProductKinds.Any())
        {
            _context.ProductKinds.AddRange(new List<ProductKind>()
            {
                productKind1,productKind2,productKind3,productKind4,productKind5,productKind6,productKind7,productKind8,productKind9,productKind10,
                productKind11,productKind12,productKind13,productKind14,productKind15,productKind16,productKind17,productKind18,productKind19,productKind20,
                productKind21,productKind22,productKind23,productKind24,productKind25,productKind26,productKind27,productKind28,productKind29,productKind30,
                productKind31,productKind32,productKind33,productKind34,productKind35,productKind36,productKind37,productKind38,productKind39,productKind40,
                productKind41,productKind42,productKind43,productKind44,productKind45,productKind46,productKind47,productKind48,productKind49,productKind50,
                productKind51,productKind52,productKind53,productKind54,productKind55,productKind56,productKind57,productKind58,productKind59,productKind60,
                productKind61,productKind62,productKind63,productKind64,productKind65,productKind66,productKind67,productKind68,productKind69,productKind70,
                productKind71,productKind72,productKind73,productKind74,productKind75,productKind76,productKind77,productKind78,productKind79,productKind80,
                productKind81,productKind82,productKind83,productKind84,productKind85,productKind86,productKind87,productKind88,productKind89,productKind90,
                productKind91,productKind92,productKind93,productKind94,productKind95,productKind96,productKind97,productKind98,productKind99,productKind100,
                productKind101,productKind102,productKind103,productKind104,productKind105,productKind106,productKind107,productKind108,productKind109,productKind110,
                productKind111,productKind112,productKind113,productKind114,productKind115,productKind116,productKind117,productKind118,productKind119,productKind120,
                productKind121,productKind122,productKind123,productKind124,productKind125,productKind126,productKind127,productKind128
            });
        }

        #endregion

        #region Option For Product

        #region Value

        #region ProductAttributeOption

        var productAttributeOption1 = new ProductAttributeOption()
        {
            ProductId = product1.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption1.SetTotalCount(7); 
        var productAttributeOption2 = new ProductAttributeOption()
        {
            ProductId = product1.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption2.SetTotalCount(12);
        var productAttributeOption3 = new ProductAttributeOption()
        {
            ProductId = product1.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption3.SetTotalCount(15);
        var productAttributeOption4 = new ProductAttributeOption()
        {
            ProductId = product1.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption4.SetTotalCount(11);
        var productAttributeOption5 = new ProductAttributeOption()
        {
            ProductId = product2.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption5.SetTotalCount(7);
        var productAttributeOption6 = new ProductAttributeOption()
        {
            ProductId = product2.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption6.SetTotalCount(12);
        var productAttributeOption7 = new ProductAttributeOption()
        {
            ProductId = product2.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption7.SetTotalCount(15);
        var productAttributeOption8 = new ProductAttributeOption()
        {
            ProductId = product2.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption8.SetTotalCount(11);
        var productAttributeOption9 = new ProductAttributeOption()
        {
            ProductId = product3.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption9.SetTotalCount(7);
        var productAttributeOption10 = new ProductAttributeOption()
        {
            ProductId = product3.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption10.SetTotalCount(12);
        var productAttributeOption11 = new ProductAttributeOption()
        {
            ProductId = product3.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption11.SetTotalCount(15);
        var productAttributeOption12 = new ProductAttributeOption()
        {
            ProductId = product3.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption12.SetTotalCount(11);
        var productAttributeOption13 = new ProductAttributeOption()
        {
            ProductId = product4.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption13.SetTotalCount(7);
        var productAttributeOption14 = new ProductAttributeOption()
        {
            ProductId = product4.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption14.SetTotalCount(12);
        var productAttributeOption15 = new ProductAttributeOption()
        {
            ProductId = product4.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption15.SetTotalCount(15);
        var productAttributeOption16 = new ProductAttributeOption()
        {
            ProductId = product4.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption16.SetTotalCount(11);
        var productAttributeOption17 = new ProductAttributeOption()
        {
            ProductId = product5.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption17.SetTotalCount(7);
        var productAttributeOption18 = new ProductAttributeOption()
        {
            ProductId = product5.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption18.SetTotalCount(12);
        var productAttributeOption19 = new ProductAttributeOption()
        {
            ProductId = product5.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption19.SetTotalCount(15);
        var productAttributeOption20 = new ProductAttributeOption()
        {
            ProductId = product5.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption20.SetTotalCount(11);
        var productAttributeOption21 = new ProductAttributeOption()
        {
            ProductId = product6.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption21.SetTotalCount(7);
        var productAttributeOption22 = new ProductAttributeOption()
        {
            ProductId = product6.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption22.SetTotalCount(12);
        var productAttributeOption23 = new ProductAttributeOption()
        {
            ProductId = product6.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption23.SetTotalCount(15);
        var productAttributeOption24 = new ProductAttributeOption()
        {
            ProductId = product6.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption24.SetTotalCount(11);
        var productAttributeOption25 = new ProductAttributeOption()
        {
            ProductId = product7.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption25.SetTotalCount(7);
        var productAttributeOption26 = new ProductAttributeOption()
        {
            ProductId = product7.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption26.SetTotalCount(12);
        var productAttributeOption27 = new ProductAttributeOption()
        {
            ProductId = product7.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption27.SetTotalCount(15);
        var productAttributeOption28 = new ProductAttributeOption()
        {
            ProductId = product7.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption28.SetTotalCount(11);
        var productAttributeOption29 = new ProductAttributeOption()
        {
            ProductId = product8.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption29.SetTotalCount(7);
        var productAttributeOption30 = new ProductAttributeOption()
        {
            ProductId = product8.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption30.SetTotalCount(12);
        var productAttributeOption31 = new ProductAttributeOption()
        {
            ProductId = product8.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption31.SetTotalCount(15);
        var productAttributeOption32 = new ProductAttributeOption()
        {
            ProductId = product8.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption32.SetTotalCount(11);
        var productAttributeOption33 = new ProductAttributeOption()
        {
            ProductId = product9.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption33.SetTotalCount(7);
        var productAttributeOption34 = new ProductAttributeOption()
        {
            ProductId = product9.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption34.SetTotalCount(12);
        var productAttributeOption35 = new ProductAttributeOption()
        {
            ProductId = product9.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption35.SetTotalCount(15);
        var productAttributeOption36 = new ProductAttributeOption()
        {
            ProductId = product9.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption36.SetTotalCount(11);
        var productAttributeOption37 = new ProductAttributeOption()
        {
            ProductId = product10.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption37.SetTotalCount(7);
        var productAttributeOption38 = new ProductAttributeOption()
        {
            ProductId = product10.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption38.SetTotalCount(12);
        var productAttributeOption39 = new ProductAttributeOption()
        {
            ProductId = product10.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption39.SetTotalCount(15);
        var productAttributeOption40 = new ProductAttributeOption()
        {
            ProductId = product10.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption40.SetTotalCount(11);
        var productAttributeOption41 = new ProductAttributeOption()
        {
            ProductId = product11.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption41.SetTotalCount(7);
        var productAttributeOption42 = new ProductAttributeOption()
        {
            ProductId = product11.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption42.SetTotalCount(12);
        var productAttributeOption43 = new ProductAttributeOption()
        {
            ProductId = product11.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption43.SetTotalCount(15);
        var productAttributeOption44 = new ProductAttributeOption()
        {
            ProductId = product11.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption44.SetTotalCount(11);
        var productAttributeOption45 = new ProductAttributeOption()
        {
            ProductId = product12.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption45.SetTotalCount(7);
        var productAttributeOption46 = new ProductAttributeOption()
        {
            ProductId = product12.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption46.SetTotalCount(12);
        var productAttributeOption47 = new ProductAttributeOption()
        {
            ProductId = product12.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption47.SetTotalCount(15);
        var productAttributeOption48 = new ProductAttributeOption()
        {
            ProductId = product12.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption48.SetTotalCount(11);
        var productAttributeOption49 = new ProductAttributeOption()
        {
            ProductId = product13.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption49.SetTotalCount(7);
        var productAttributeOption50 = new ProductAttributeOption()
        {
            ProductId = product13.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption50.SetTotalCount(12);
        var productAttributeOption51 = new ProductAttributeOption()
        {
            ProductId = product13.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption51.SetTotalCount(15);
        var productAttributeOption52 = new ProductAttributeOption()
        {
            ProductId = product13.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption52.SetTotalCount(11);
        var productAttributeOption53 = new ProductAttributeOption()
        {
            ProductId = product14.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption53.SetTotalCount(7);
        var productAttributeOption54 = new ProductAttributeOption()
        {
            ProductId = product14.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption54.SetTotalCount(12);
        var productAttributeOption55 = new ProductAttributeOption()
        {
            ProductId = product14.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption55.SetTotalCount(15);
        var productAttributeOption56 = new ProductAttributeOption()
        {
            ProductId = product14.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption56.SetTotalCount(11);
        var productAttributeOption57 = new ProductAttributeOption()
        {
            ProductId = product15.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption57.SetTotalCount(7);
        var productAttributeOption58 = new ProductAttributeOption()
        {
            ProductId = product15.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption58.SetTotalCount(12);
        var productAttributeOption59 = new ProductAttributeOption()
        {
            ProductId = product15.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption59.SetTotalCount(15);
        var productAttributeOption60 = new ProductAttributeOption()
        {
            ProductId = product15.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption60.SetTotalCount(11);
        var productAttributeOption61 = new ProductAttributeOption()
        {
            ProductId = product16.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption61.SetTotalCount(7);
        var productAttributeOption62 = new ProductAttributeOption()
        {
            ProductId = product16.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption62.SetTotalCount(12);
        var productAttributeOption63 = new ProductAttributeOption()
        {
            ProductId = product16.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption63.SetTotalCount(15);
        var productAttributeOption64 = new ProductAttributeOption()
        {
            ProductId = product16.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption64.SetTotalCount(11);
        var productAttributeOption65 = new ProductAttributeOption()
        {
            ProductId = product17.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption65.SetTotalCount(7);
        var productAttributeOption66 = new ProductAttributeOption()
        {
            ProductId = product17.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption66.SetTotalCount(12);
        var productAttributeOption67 = new ProductAttributeOption()
        {
            ProductId = product17.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption67.SetTotalCount(15);
        var productAttributeOption68 = new ProductAttributeOption()
        {
            ProductId = product17.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption68.SetTotalCount(11);
        var productAttributeOption69 = new ProductAttributeOption()
        {
            ProductId = product18.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption69.SetTotalCount(7);
        var productAttributeOption70 = new ProductAttributeOption()
        {
            ProductId = product18.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption70.SetTotalCount(12);
        var productAttributeOption71 = new ProductAttributeOption()
        {
            ProductId = product18.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption71.SetTotalCount(15);
        var productAttributeOption72 = new ProductAttributeOption()
        {
            ProductId = product18.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption72.SetTotalCount(11);
        var productAttributeOption73 = new ProductAttributeOption()
        {
            ProductId = product19.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption73.SetTotalCount(7);
        var productAttributeOption74 = new ProductAttributeOption()
        {
            ProductId = product19.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption74.SetTotalCount(12);
        var productAttributeOption75 = new ProductAttributeOption()
        {
            ProductId = product19.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption75.SetTotalCount(15);
        var productAttributeOption76 = new ProductAttributeOption()
        {
            ProductId = product19.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption76.SetTotalCount(11);
        var productAttributeOption77 = new ProductAttributeOption()
        {
            ProductId = product20.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption77.SetTotalCount(7);
        var productAttributeOption78 = new ProductAttributeOption()
        {
            ProductId = product20.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption78.SetTotalCount(12);
        var productAttributeOption79 = new ProductAttributeOption()
        {
            ProductId = product20.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption79.SetTotalCount(15);
        var productAttributeOption80 = new ProductAttributeOption()
        {
            ProductId = product20.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption80.SetTotalCount(11);
        var productAttributeOption81 = new ProductAttributeOption()
        {
            ProductId = product21.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption81.SetTotalCount(7);
        var productAttributeOption82 = new ProductAttributeOption()
        {
            ProductId = product21.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption82.SetTotalCount(12);
        var productAttributeOption83 = new ProductAttributeOption()
        {
            ProductId = product21.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption83.SetTotalCount(15);
        var productAttributeOption84 = new ProductAttributeOption()
        {
            ProductId = product21.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption84.SetTotalCount(11);
        var productAttributeOption85 = new ProductAttributeOption()
        {
            ProductId = product22.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption85.SetTotalCount(7);
        var productAttributeOption86 = new ProductAttributeOption()
        {
            ProductId = product22.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption86.SetTotalCount(12);
        var productAttributeOption87 = new ProductAttributeOption()
        {
            ProductId = product22.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption87.SetTotalCount(15);
        var productAttributeOption88 = new ProductAttributeOption()
        {
            ProductId = product22.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption88.SetTotalCount(11);
        var productAttributeOption89 = new ProductAttributeOption()
        {
            ProductId = product23.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption89.SetTotalCount(7);
        var productAttributeOption90 = new ProductAttributeOption()
        {
            ProductId = product23.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption90.SetTotalCount(12);
        var productAttributeOption91 = new ProductAttributeOption()
        {
            ProductId = product23.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption91.SetTotalCount(15);
        var productAttributeOption92 = new ProductAttributeOption()
        {
            ProductId = product23.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption92.SetTotalCount(11);
        var productAttributeOption93 = new ProductAttributeOption()
        {
            ProductId = product24.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption93.SetTotalCount(7);
        var productAttributeOption94 = new ProductAttributeOption()
        {
            ProductId = product24.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption94.SetTotalCount(12);
        var productAttributeOption95 = new ProductAttributeOption()
        {
            ProductId = product24.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption95.SetTotalCount(15);
        var productAttributeOption96 = new ProductAttributeOption()
        {
            ProductId = product24.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption96.SetTotalCount(11);
        var productAttributeOption97 = new ProductAttributeOption()
        {
            ProductId = product25.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption97.SetTotalCount(7);
        var productAttributeOption98 = new ProductAttributeOption()
        {
            ProductId = product25.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption98.SetTotalCount(12);
        var productAttributeOption99 = new ProductAttributeOption()
        {
            ProductId = product25.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption99.SetTotalCount(15);
        var productAttributeOption100 = new ProductAttributeOption()
        {
            ProductId = product25.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption100.SetTotalCount(11);
        var productAttributeOption101 = new ProductAttributeOption()
        {
            ProductId = product26.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption101.SetTotalCount(7);
        var productAttributeOption102 = new ProductAttributeOption()
        {
            ProductId = product26.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption102.SetTotalCount(12);
        var productAttributeOption103 = new ProductAttributeOption()
        {
            ProductId = product26.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption103.SetTotalCount(15);
        var productAttributeOption104 = new ProductAttributeOption()
        {
            ProductId = product26.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption104.SetTotalCount(11);
        var productAttributeOption105 = new ProductAttributeOption()
        {
            ProductId = product27.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption105.SetTotalCount(7);
        var productAttributeOption106 = new ProductAttributeOption()
        {
            ProductId = product27.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption106.SetTotalCount(12);
        var productAttributeOption107 = new ProductAttributeOption()
        {
            ProductId = product27.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption107.SetTotalCount(15);
        var productAttributeOption108 = new ProductAttributeOption()
        {
            ProductId = product27.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption108.SetTotalCount(11);
        var productAttributeOption109 = new ProductAttributeOption()
        {
            ProductId = product28.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption109.SetTotalCount(7);
        var productAttributeOption110 = new ProductAttributeOption()
        {
            ProductId = product28.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption110.SetTotalCount(12);
        var productAttributeOption111 = new ProductAttributeOption()
        {
            ProductId = product28.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption111.SetTotalCount(15);
        var productAttributeOption112 = new ProductAttributeOption()
        {
            ProductId = product28.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption112.SetTotalCount(11);
        var productAttributeOption113 = new ProductAttributeOption()
        {
            ProductId = product29.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption113.SetTotalCount(7);
        var productAttributeOption114 = new ProductAttributeOption()
        {
            ProductId = product29.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption114.SetTotalCount(12);
        var productAttributeOption115 = new ProductAttributeOption()
        {
            ProductId = product29.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption115.SetTotalCount(15);
        var productAttributeOption116 = new ProductAttributeOption()
        {
            ProductId = product29.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption116.SetTotalCount(11);
        var productAttributeOption117 = new ProductAttributeOption()
        {
            ProductId = product30.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption117.SetTotalCount(7);
        var productAttributeOption118 = new ProductAttributeOption()
        {
            ProductId = product30.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption118.SetTotalCount(12);
        var productAttributeOption119 = new ProductAttributeOption()
        {
            ProductId = product30.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption119.SetTotalCount(15);
        var productAttributeOption120 = new ProductAttributeOption()
        {
            ProductId = product30.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption120.SetTotalCount(11);
        var productAttributeOption121 = new ProductAttributeOption()
        {
            ProductId = product31.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption121.SetTotalCount(7);
        var productAttributeOption122 = new ProductAttributeOption()
        {
            ProductId = product31.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption122.SetTotalCount(12);
        var productAttributeOption123 = new ProductAttributeOption()
        {
            ProductId = product31.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption123.SetTotalCount(15);
        var productAttributeOption124 = new ProductAttributeOption()
        {
            ProductId = product31.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption124.SetTotalCount(11);
        var productAttributeOption125 = new ProductAttributeOption()
        {
            ProductId = product32.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption125.SetTotalCount(7);
        var productAttributeOption126 = new ProductAttributeOption()
        {
            ProductId = product32.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption126.SetTotalCount(12);
        var productAttributeOption127 = new ProductAttributeOption()
        {
            ProductId = product32.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption127.SetTotalCount(15);
        var productAttributeOption128 = new ProductAttributeOption()
        {
            ProductId = product32.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption128.SetTotalCount(11);
        var productAttributeOption129 = new ProductAttributeOption()
        {
            ProductId = product33.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption129.SetTotalCount(7);
        var productAttributeOption130 = new ProductAttributeOption()
        {
            ProductId = product33.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption130.SetTotalCount(12);
        var productAttributeOption131 = new ProductAttributeOption()
        {
            ProductId = product33.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption131.SetTotalCount(15);
        var productAttributeOption132 = new ProductAttributeOption()
        {
            ProductId = product33.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption132.SetTotalCount(11);
        var productAttributeOption133 = new ProductAttributeOption()
        {
            ProductId = product34.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption133.SetTotalCount(7);
        var productAttributeOption134 = new ProductAttributeOption()
        {
            ProductId = product34.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption134.SetTotalCount(12);
        var productAttributeOption135 = new ProductAttributeOption()
        {
            ProductId = product34.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption135.SetTotalCount(15);
        var productAttributeOption136 = new ProductAttributeOption()
        {
            ProductId = product34.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption136.SetTotalCount(11);
        var productAttributeOption137 = new ProductAttributeOption()
        {
            ProductId = product35.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption137.SetTotalCount(7);
        var productAttributeOption138 = new ProductAttributeOption()
        {
            ProductId = product35.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption138.SetTotalCount(12);
        var productAttributeOption139 = new ProductAttributeOption()
        {
            ProductId = product35.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption139.SetTotalCount(15);
        var productAttributeOption140 = new ProductAttributeOption()
        {
            ProductId = product35.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption140.SetTotalCount(11);
        var productAttributeOption141 = new ProductAttributeOption()
        {
            ProductId = product36.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption141.SetTotalCount(7);
        var productAttributeOption142 = new ProductAttributeOption()
        {
            ProductId = product36.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption142.SetTotalCount(12);
        var productAttributeOption143 = new ProductAttributeOption()
        {
            ProductId = product36.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption143.SetTotalCount(15);
        var productAttributeOption144 = new ProductAttributeOption()
        {
            ProductId = product36.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption144.SetTotalCount(11);
        var productAttributeOption145 = new ProductAttributeOption()
        {
            ProductId = product37.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption145.SetTotalCount(7);
        var productAttributeOption146 = new ProductAttributeOption()
        {
            ProductId = product37.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption146.SetTotalCount(12);
        var productAttributeOption147 = new ProductAttributeOption()
        {
            ProductId = product37.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption147.SetTotalCount(15);
        var productAttributeOption148 = new ProductAttributeOption()
        {
            ProductId = product37.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption148.SetTotalCount(11);
        var productAttributeOption149 = new ProductAttributeOption()
        {
            ProductId = product38.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption149.SetTotalCount(7);
        var productAttributeOption150 = new ProductAttributeOption()
        {
            ProductId = product38.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption150.SetTotalCount(12);
        var productAttributeOption151 = new ProductAttributeOption()
        {
            ProductId = product38.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption151.SetTotalCount(15);
        var productAttributeOption152 = new ProductAttributeOption()
        {
            ProductId = product38.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption152.SetTotalCount(11);
        var productAttributeOption153 = new ProductAttributeOption()
        {
            ProductId = product39.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption153.SetTotalCount(7);
        var productAttributeOption154 = new ProductAttributeOption()
        {
            ProductId = product39.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption154.SetTotalCount(12);
        var productAttributeOption155 = new ProductAttributeOption()
        {
            ProductId = product39.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption155.SetTotalCount(15);
        var productAttributeOption156 = new ProductAttributeOption()
        {
            ProductId = product39.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption156.SetTotalCount(11);
        var productAttributeOption157 = new ProductAttributeOption()
        {
            ProductId = product40.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption157.SetTotalCount(7);
        var productAttributeOption158 = new ProductAttributeOption()
        {
            ProductId = product40.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption158.SetTotalCount(12);
        var productAttributeOption159 = new ProductAttributeOption()
        {
            ProductId = product40.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption159.SetTotalCount(15);
        var productAttributeOption160 = new ProductAttributeOption()
        {
            ProductId = product40.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption160.SetTotalCount(11);
        var productAttributeOption161 = new ProductAttributeOption()
        {
            ProductId = product41.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption161.SetTotalCount(7);
        var productAttributeOption162 = new ProductAttributeOption()
        {
            ProductId = product41.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption162.SetTotalCount(12);
        var productAttributeOption163 = new ProductAttributeOption()
        {
            ProductId = product41.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption163.SetTotalCount(15);
        var productAttributeOption164 = new ProductAttributeOption()
        {
            ProductId = product41.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption164.SetTotalCount(11);
        var productAttributeOption165 = new ProductAttributeOption()
        {
            ProductId = product42.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption165.SetTotalCount(7);
        var productAttributeOption166 = new ProductAttributeOption()
        {
            ProductId = product42.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption166.SetTotalCount(12);
        var productAttributeOption167 = new ProductAttributeOption()
        {
            ProductId = product42.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption167.SetTotalCount(15);
        var productAttributeOption168 = new ProductAttributeOption()
        {
            ProductId = product42.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption168.SetTotalCount(11);
        var productAttributeOption169 = new ProductAttributeOption()
        {
            ProductId = product43.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption169.SetTotalCount(7);
        var productAttributeOption170 = new ProductAttributeOption()
        {
            ProductId = product43.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption170.SetTotalCount(12);
        var productAttributeOption171 = new ProductAttributeOption()
        {
            ProductId = product43.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption171.SetTotalCount(15);
        var productAttributeOption172 = new ProductAttributeOption()
        {
            ProductId = product43.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption172.SetTotalCount(11);
        var productAttributeOption173 = new ProductAttributeOption()
        {
            ProductId = product44.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption173.SetTotalCount(7);
        var productAttributeOption174 = new ProductAttributeOption()
        {
            ProductId = product44.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption174.SetTotalCount(12);
        var productAttributeOption175 = new ProductAttributeOption()
        {
            ProductId = product44.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption175.SetTotalCount(15);
        var productAttributeOption176 = new ProductAttributeOption()
        {
            ProductId = product44.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption176.SetTotalCount(11);
        var productAttributeOption177 = new ProductAttributeOption()
        {
            ProductId = product45.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption177.SetTotalCount(7);
        var productAttributeOption178 = new ProductAttributeOption()
        {
            ProductId = product45.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption178.SetTotalCount(12);
        var productAttributeOption179 = new ProductAttributeOption()
        {
            ProductId = product45.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption179.SetTotalCount(15);
        var productAttributeOption180 = new ProductAttributeOption()
        {
            ProductId = product45.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption180.SetTotalCount(11);
        var productAttributeOption181 = new ProductAttributeOption()
        {
            ProductId = product46.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption181.SetTotalCount(7);
        var productAttributeOption182 = new ProductAttributeOption()
        {
            ProductId = product46.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption182.SetTotalCount(12);
        var productAttributeOption183 = new ProductAttributeOption()
        {
            ProductId = product46.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption183.SetTotalCount(15);
        var productAttributeOption184 = new ProductAttributeOption()
        {
            ProductId = product46.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption184.SetTotalCount(11);
        var productAttributeOption185 = new ProductAttributeOption()
        {
            ProductId = product47.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption185.SetTotalCount(7);
        var productAttributeOption186 = new ProductAttributeOption()
        {
            ProductId = product47.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption186.SetTotalCount(12);
        var productAttributeOption187 = new ProductAttributeOption()
        {
            ProductId = product47.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption187.SetTotalCount(15);
        var productAttributeOption188 = new ProductAttributeOption()
        {
            ProductId = product47.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption188.SetTotalCount(11);
        var productAttributeOption189 = new ProductAttributeOption()
        {
            ProductId = product48.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption189.SetTotalCount(7);
        var productAttributeOption190 = new ProductAttributeOption()
        {
            ProductId = product48.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption190.SetTotalCount(12);
        var productAttributeOption191 = new ProductAttributeOption()
        {
            ProductId = product48.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption191.SetTotalCount(15);
        var productAttributeOption192 = new ProductAttributeOption()
        {
            ProductId = product48.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption192.SetTotalCount(11);
        var productAttributeOption193 = new ProductAttributeOption()
        {
            ProductId = product49.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption193.SetTotalCount(7);
        var productAttributeOption194 = new ProductAttributeOption()
        {
            ProductId = product49.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption194.SetTotalCount(12);
        var productAttributeOption195 = new ProductAttributeOption()
        {
            ProductId = product49.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption195.SetTotalCount(15);
        var productAttributeOption196 = new ProductAttributeOption()
        {
            ProductId = product49.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption196.SetTotalCount(11);
        var productAttributeOption197 = new ProductAttributeOption()
        {
            ProductId = product50.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption197.SetTotalCount(7);
        var productAttributeOption198 = new ProductAttributeOption()
        {
            ProductId = product50.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption198.SetTotalCount(12);
        var productAttributeOption199 = new ProductAttributeOption()
        {
            ProductId = product50.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption199.SetTotalCount(15);
        var productAttributeOption200 = new ProductAttributeOption()
        {
            ProductId = product50.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption200.SetTotalCount(11);
        var productAttributeOption201 = new ProductAttributeOption()
        {
            ProductId = product51.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption201.SetTotalCount(12);
        var productAttributeOption202 = new ProductAttributeOption()
        {
            ProductId = product51.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption202.SetTotalCount(15);
        var productAttributeOption203 = new ProductAttributeOption()
        {
            ProductId = product51.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption203.SetTotalCount(11);
        var productAttributeOption204 = new ProductAttributeOption()
        {
            ProductId = product51.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption204.SetTotalCount(7);
        var productAttributeOption205 = new ProductAttributeOption()
        {
            ProductId = product52.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption205.SetTotalCount(12);
        var productAttributeOption206 = new ProductAttributeOption()
        {
            ProductId = product52.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption206.SetTotalCount(15);
        var productAttributeOption207 = new ProductAttributeOption()
        {
            ProductId = product52.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption207.SetTotalCount(11);
        var productAttributeOption208 = new ProductAttributeOption()
        {
            ProductId = product52.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption208.SetTotalCount(7);
        var productAttributeOption209 = new ProductAttributeOption()
        {
            ProductId = product53.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption209.SetTotalCount(12);
        var productAttributeOption210 = new ProductAttributeOption()
        {
            ProductId = product53.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption210.SetTotalCount(15);
        var productAttributeOption211 = new ProductAttributeOption()
        {
            ProductId = product53.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption211.SetTotalCount(11);
        var productAttributeOption212 = new ProductAttributeOption()
        {
            ProductId = product53.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption212.SetTotalCount(7);
        var productAttributeOption213 = new ProductAttributeOption()
        {
            ProductId = product54.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption213.SetTotalCount(12);
        var productAttributeOption214 = new ProductAttributeOption()
        {
            ProductId = product54.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption214.SetTotalCount(15);
        var productAttributeOption215 = new ProductAttributeOption()
        {
            ProductId = product54.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption215.SetTotalCount(11);
        var productAttributeOption216 = new ProductAttributeOption()
        {
            ProductId = product54.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption216.SetTotalCount(7);
        var productAttributeOption217 = new ProductAttributeOption()
        {
            ProductId = product55.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption217.SetTotalCount(12);
        var productAttributeOption218 = new ProductAttributeOption()
        {
            ProductId = product55.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption218.SetTotalCount(15);
        var productAttributeOption219 = new ProductAttributeOption()
        {
            ProductId = product55.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption219.SetTotalCount(11);
        var productAttributeOption220 = new ProductAttributeOption()
        {
            ProductId = product55.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption220.SetTotalCount(7);
        var productAttributeOption221 = new ProductAttributeOption()
        {
            ProductId = product56.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption221.SetTotalCount(12);
        var productAttributeOption222 = new ProductAttributeOption()
        {
            ProductId = product56.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption222.SetTotalCount(15);
        var productAttributeOption223 = new ProductAttributeOption()
        {
            ProductId = product56.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption223.SetTotalCount(11);
        var productAttributeOption224 = new ProductAttributeOption()
        {
            ProductId = product56.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption224.SetTotalCount(7);
        var productAttributeOption225 = new ProductAttributeOption()
        {
            ProductId = product57.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption225.SetTotalCount(12);
        var productAttributeOption226 = new ProductAttributeOption()
        {
            ProductId = product57.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption226.SetTotalCount(15);
        var productAttributeOption227 = new ProductAttributeOption()
        {
            ProductId = product57.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption227.SetTotalCount(11);
        var productAttributeOption228 = new ProductAttributeOption()
        {
            ProductId = product57.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption228.SetTotalCount(7);
        var productAttributeOption229 = new ProductAttributeOption()
        {
            ProductId = product58.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption229.SetTotalCount(12);
        var productAttributeOption230 = new ProductAttributeOption()
        {
            ProductId = product58.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption230.SetTotalCount(15);
        var productAttributeOption231 = new ProductAttributeOption()
        {
            ProductId = product58.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption231.SetTotalCount(11);
        var productAttributeOption232 = new ProductAttributeOption()
        {
            ProductId = product58.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption232.SetTotalCount(7);
        var productAttributeOption233 = new ProductAttributeOption()
        {
            ProductId = product59.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption233.SetTotalCount(12);
        var productAttributeOption234 = new ProductAttributeOption()
        {
            ProductId = product59.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption234.SetTotalCount(15);
        var productAttributeOption235 = new ProductAttributeOption()
        {
            ProductId = product59.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption235.SetTotalCount(11);
        var productAttributeOption236 = new ProductAttributeOption()
        {
            ProductId = product59.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption236.SetTotalCount(7);
        var productAttributeOption237 = new ProductAttributeOption()
        {
            ProductId = product60.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption237.SetTotalCount(12);
        var productAttributeOption238 = new ProductAttributeOption()
        {
            ProductId = product60.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption238.SetTotalCount(15);
        var productAttributeOption239 = new ProductAttributeOption()
        {
            ProductId = product60.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption239.SetTotalCount(11);
        var productAttributeOption240 = new ProductAttributeOption()
        {
            ProductId = product60.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption240.SetTotalCount(7);
        var productAttributeOption241 = new ProductAttributeOption()
        {
            ProductId = product61.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption241.SetTotalCount(12);
        var productAttributeOption242 = new ProductAttributeOption()
        {
            ProductId = product61.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption242.SetTotalCount(15);
        var productAttributeOption243 = new ProductAttributeOption()
        {
            ProductId = product61.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption243.SetTotalCount(11);
        var productAttributeOption244 = new ProductAttributeOption()
        {
            ProductId = product61.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption244.SetTotalCount(7);
        var productAttributeOption245 = new ProductAttributeOption()
        {
            ProductId = product62.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption245.SetTotalCount(12);
        var productAttributeOption246 = new ProductAttributeOption()
        {
            ProductId = product62.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption246.SetTotalCount(15);
        var productAttributeOption247 = new ProductAttributeOption()
        {
            ProductId = product62.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption247.SetTotalCount(11);
        var productAttributeOption248 = new ProductAttributeOption()
        {
            ProductId = product62.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption248.SetTotalCount(7);
        var productAttributeOption249 = new ProductAttributeOption()
        {
            ProductId = product63.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption249.SetTotalCount(12);
        var productAttributeOption250 = new ProductAttributeOption()
        {
            ProductId = product63.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption250.SetTotalCount(15);
        var productAttributeOption251 = new ProductAttributeOption()
        {
            ProductId = product63.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption251.SetTotalCount(11);
        var productAttributeOption252 = new ProductAttributeOption()
        {
            ProductId = product63.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
            IsDefault = true
        };
        productAttributeOption252.SetTotalCount(7);
        var productAttributeOption253 = new ProductAttributeOption()
        {
            ProductId = product64.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption253.SetTotalCount(12);
        var productAttributeOption254 = new ProductAttributeOption()
        {
            ProductId = product64.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption254.SetTotalCount(15);
        var productAttributeOption255 = new ProductAttributeOption()
        {
            ProductId = product64.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption255.SetTotalCount(11);
        var productAttributeOption256 = new ProductAttributeOption()
        {
            ProductId = product64.Id,
            Badges = new List<Badge>() { badge1, badge2, badge3 },
            SafetyStockQty = 0.00,
            MinStockQty = 0.00,
            MaxStockQty = 10.00,
        };
        productAttributeOption256.SetTotalCount(11);
        
        if (!_context.ProductAttributeOptions.Any())
        {
            _context.ProductAttributeOptions.AddRange(new List<ProductAttributeOption>()
            {
                productAttributeOption1,productAttributeOption2,productAttributeOption3,productAttributeOption4,productAttributeOption5,productAttributeOption6,productAttributeOption7,productAttributeOption8,productAttributeOption9,productAttributeOption10,productAttributeOption11,productAttributeOption12,productAttributeOption13,productAttributeOption14,productAttributeOption15,productAttributeOption16,
                productAttributeOption17,productAttributeOption18,productAttributeOption19,productAttributeOption20,productAttributeOption21,productAttributeOption22,productAttributeOption23,productAttributeOption24,productAttributeOption25,productAttributeOption26,productAttributeOption27,productAttributeOption28,productAttributeOption29,productAttributeOption30,productAttributeOption31,productAttributeOption32,
                productAttributeOption33,productAttributeOption34,productAttributeOption35,productAttributeOption36,productAttributeOption37,productAttributeOption38,productAttributeOption39,productAttributeOption40,productAttributeOption41,productAttributeOption42,productAttributeOption43,productAttributeOption44,productAttributeOption45,productAttributeOption46,productAttributeOption47,productAttributeOption48,
                productAttributeOption49,productAttributeOption50,productAttributeOption51,productAttributeOption52,productAttributeOption53,productAttributeOption54,productAttributeOption55,productAttributeOption56,productAttributeOption57,productAttributeOption58,productAttributeOption59,productAttributeOption60,productAttributeOption61,productAttributeOption62,productAttributeOption63,productAttributeOption64,
                productAttributeOption65,productAttributeOption66,productAttributeOption67,productAttributeOption68,productAttributeOption69,productAttributeOption70,productAttributeOption71,productAttributeOption72,productAttributeOption73,productAttributeOption74,productAttributeOption75,productAttributeOption76,productAttributeOption77,productAttributeOption78,productAttributeOption79,productAttributeOption80,
                productAttributeOption81,productAttributeOption82,productAttributeOption83,productAttributeOption84,productAttributeOption85,productAttributeOption86,productAttributeOption87,productAttributeOption88,productAttributeOption89,productAttributeOption90,productAttributeOption91,productAttributeOption92,productAttributeOption93,productAttributeOption94,productAttributeOption95,productAttributeOption96,
                productAttributeOption97,productAttributeOption98,productAttributeOption99,productAttributeOption100,productAttributeOption101,productAttributeOption102,productAttributeOption103,productAttributeOption104,productAttributeOption105,productAttributeOption106,productAttributeOption107,productAttributeOption108,productAttributeOption109,productAttributeOption110,productAttributeOption111,productAttributeOption112,
                productAttributeOption113,productAttributeOption114,productAttributeOption115,productAttributeOption116,productAttributeOption117,productAttributeOption118,productAttributeOption119,productAttributeOption120,productAttributeOption121,productAttributeOption122,productAttributeOption123,productAttributeOption124,productAttributeOption125,productAttributeOption126,productAttributeOption127,productAttributeOption128,
                productAttributeOption129,productAttributeOption130,productAttributeOption131,productAttributeOption132,productAttributeOption133,productAttributeOption134,productAttributeOption135,productAttributeOption136,productAttributeOption137,productAttributeOption138,productAttributeOption139,productAttributeOption140,productAttributeOption141,productAttributeOption142,productAttributeOption143,productAttributeOption144,
                productAttributeOption145,productAttributeOption146,productAttributeOption147,productAttributeOption148,productAttributeOption149,productAttributeOption150,productAttributeOption151,productAttributeOption152,productAttributeOption153,productAttributeOption154,productAttributeOption155,productAttributeOption156,productAttributeOption157,productAttributeOption158,productAttributeOption159,productAttributeOption160,
                productAttributeOption161,productAttributeOption162,productAttributeOption163,productAttributeOption164,productAttributeOption165,productAttributeOption166,productAttributeOption167,productAttributeOption168,productAttributeOption169,productAttributeOption170,productAttributeOption171,productAttributeOption172,productAttributeOption173,productAttributeOption174,productAttributeOption175,productAttributeOption176,
                productAttributeOption177,productAttributeOption178,productAttributeOption179,productAttributeOption180,productAttributeOption181,productAttributeOption182,productAttributeOption183,productAttributeOption184,productAttributeOption185,productAttributeOption186,productAttributeOption187,productAttributeOption188,productAttributeOption189,productAttributeOption190,productAttributeOption191,productAttributeOption192,
                productAttributeOption193,productAttributeOption194,productAttributeOption195,productAttributeOption196,productAttributeOption197,productAttributeOption198,productAttributeOption199,productAttributeOption200,productAttributeOption201,productAttributeOption202,productAttributeOption203,productAttributeOption204,productAttributeOption205,productAttributeOption206,productAttributeOption207,productAttributeOption208,
                productAttributeOption209,productAttributeOption210,productAttributeOption211,productAttributeOption212,productAttributeOption213,productAttributeOption214,productAttributeOption215,productAttributeOption216,productAttributeOption217,productAttributeOption218,productAttributeOption219,productAttributeOption220,productAttributeOption221,productAttributeOption222,productAttributeOption223,productAttributeOption224,
                productAttributeOption225,productAttributeOption226,productAttributeOption227,productAttributeOption228,productAttributeOption229,productAttributeOption230,productAttributeOption231,productAttributeOption232,productAttributeOption233,productAttributeOption234,productAttributeOption235,productAttributeOption236,productAttributeOption237,productAttributeOption238,productAttributeOption239,productAttributeOption240,
                productAttributeOption241,productAttributeOption242,productAttributeOption243,productAttributeOption244,productAttributeOption245,productAttributeOption246,productAttributeOption247,productAttributeOption248,productAttributeOption249,
                productAttributeOption250,productAttributeOption251,productAttributeOption252,productAttributeOption253,productAttributeOption254,productAttributeOption255,productAttributeOption256
            });
        }
        #endregion

        #endregion

        #endregion

        await _context.SaveChangesAsync();

        #endregion

        #region Eighth Step

        #region ProductAttributeOptionRole

        
        var productAttributeOptionRole1 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption1.Id
        };
        productAttributeOptionRole1.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole1.SetMainMaxOrderQty(10);
        productAttributeOptionRole1.SetMainMinOrderQty(2);
        var productAttributeOptionRole2 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption1.Id
        };
        productAttributeOptionRole2.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole2.SetMainMaxOrderQty(10);
        productAttributeOptionRole2.SetMainMinOrderQty(2);
        var productAttributeOptionRole3 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption2.Id
        };
        productAttributeOptionRole3.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole3.SetMainMaxOrderQty(10);
        productAttributeOptionRole3.SetMainMinOrderQty(2);
        var productAttributeOptionRole4 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption2.Id
        };
        productAttributeOptionRole4.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole4.SetMainMaxOrderQty(10);
        productAttributeOptionRole4.SetMainMinOrderQty(2);
        var productAttributeOptionRole5 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption3.Id
        };
        productAttributeOptionRole5.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole5.SetMainMaxOrderQty(10);
        productAttributeOptionRole5.SetMainMinOrderQty(2);
        var productAttributeOptionRole6 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption3.Id
        };
        productAttributeOptionRole6.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole6.SetMainMaxOrderQty(10);
        productAttributeOptionRole6.SetMainMinOrderQty(2);
        var productAttributeOptionRole7 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption4.Id
        };
        productAttributeOptionRole7.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole7.SetMainMaxOrderQty(10);
        productAttributeOptionRole7.SetMainMinOrderQty(2);
        var productAttributeOptionRole8 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption4.Id
        };
        productAttributeOptionRole8.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole8.SetMainMaxOrderQty(10);
        productAttributeOptionRole8.SetMainMinOrderQty(2);
        var productAttributeOptionRole9 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption5.Id
        };
        productAttributeOptionRole9.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole9.SetMainMaxOrderQty(10);
        productAttributeOptionRole9.SetMainMinOrderQty(2);
        var productAttributeOptionRole10 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption5.Id
        };
        productAttributeOptionRole10.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole10.SetMainMaxOrderQty(10);
        productAttributeOptionRole10.SetMainMinOrderQty(2);
        var productAttributeOptionRole11 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption6.Id
        };
        productAttributeOptionRole11.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole11.SetMainMaxOrderQty(10);
        productAttributeOptionRole11.SetMainMinOrderQty(2);
        var productAttributeOptionRole12 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption6.Id
        };
        productAttributeOptionRole12.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole12.SetMainMaxOrderQty(10);
        productAttributeOptionRole12.SetMainMinOrderQty(2);
        var productAttributeOptionRole13 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption7.Id
        };
        productAttributeOptionRole13.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole13.SetMainMaxOrderQty(10);
        productAttributeOptionRole13.SetMainMinOrderQty(2);
        var productAttributeOptionRole14 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption7.Id
        };
        productAttributeOptionRole14.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole14.SetMainMaxOrderQty(10);
        productAttributeOptionRole14.SetMainMinOrderQty(2);
        var productAttributeOptionRole15 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption8.Id
        };
        productAttributeOptionRole15.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole15.SetMainMaxOrderQty(10);
        productAttributeOptionRole15.SetMainMinOrderQty(2);
        var productAttributeOptionRole16 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption8.Id
        };
        productAttributeOptionRole16.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole16.SetMainMaxOrderQty(10);
        productAttributeOptionRole16.SetMainMinOrderQty(2);
        var productAttributeOptionRole17 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption9.Id
        };
        productAttributeOptionRole17.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole17.SetMainMaxOrderQty(10);
        productAttributeOptionRole17.SetMainMinOrderQty(2);
        var productAttributeOptionRole18 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption9.Id
        };
        productAttributeOptionRole18.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole18.SetMainMaxOrderQty(10);
        productAttributeOptionRole18.SetMainMinOrderQty(2);
        var productAttributeOptionRole19 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption10.Id
        };
        productAttributeOptionRole19.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole19.SetMainMaxOrderQty(10);
        productAttributeOptionRole19.SetMainMinOrderQty(2);
        var productAttributeOptionRole20 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption10.Id
        };
        productAttributeOptionRole20.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole20.SetMainMaxOrderQty(10);
        productAttributeOptionRole20.SetMainMinOrderQty(2);
        var productAttributeOptionRole21 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption11.Id
        };
        productAttributeOptionRole21.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole21.SetMainMaxOrderQty(10);
        productAttributeOptionRole21.SetMainMinOrderQty(2);
        var productAttributeOptionRole22 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption11.Id
        };
        productAttributeOptionRole22.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole22.SetMainMaxOrderQty(10);
        productAttributeOptionRole22.SetMainMinOrderQty(2);
        var productAttributeOptionRole23 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption12.Id
        };
        productAttributeOptionRole23.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole23.SetMainMaxOrderQty(10);
        productAttributeOptionRole23.SetMainMinOrderQty(2);
        var productAttributeOptionRole24 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption12.Id
        };
        productAttributeOptionRole24.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole24.SetMainMaxOrderQty(10);
        productAttributeOptionRole24.SetMainMinOrderQty(2);
        var productAttributeOptionRole25 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption13.Id
        };
        productAttributeOptionRole25.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole25.SetMainMaxOrderQty(10);
        productAttributeOptionRole25.SetMainMinOrderQty(2);
        var productAttributeOptionRole26 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption13.Id
        };
        productAttributeOptionRole26.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole26.SetMainMaxOrderQty(10);
        productAttributeOptionRole26.SetMainMinOrderQty(2);
        var productAttributeOptionRole27 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption14.Id
        };
        productAttributeOptionRole27.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole27.SetMainMaxOrderQty(10);
        productAttributeOptionRole27.SetMainMinOrderQty(2);
        var productAttributeOptionRole28 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption14.Id
        };
        productAttributeOptionRole28.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole28.SetMainMaxOrderQty(10);
        productAttributeOptionRole28.SetMainMinOrderQty(2);
        var productAttributeOptionRole29 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption15.Id
        };
        productAttributeOptionRole29.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole29.SetMainMaxOrderQty(10);
        productAttributeOptionRole29.SetMainMinOrderQty(2);
        var productAttributeOptionRole30 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption15.Id
        };
        productAttributeOptionRole30.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole30.SetMainMaxOrderQty(10);
        productAttributeOptionRole30.SetMainMinOrderQty(2);
        var productAttributeOptionRole31 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption16.Id
        };
        productAttributeOptionRole31.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole31.SetMainMaxOrderQty(10);
        productAttributeOptionRole31.SetMainMinOrderQty(2);
        var productAttributeOptionRole32 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption16.Id
        };
        productAttributeOptionRole32.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole32.SetMainMaxOrderQty(10);
        productAttributeOptionRole32.SetMainMinOrderQty(2);
        var productAttributeOptionRole33 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption17.Id
        };
        productAttributeOptionRole33.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole33.SetMainMaxOrderQty(10);
        productAttributeOptionRole33.SetMainMinOrderQty(2);
        var productAttributeOptionRole34 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption17.Id
        };
        productAttributeOptionRole34.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole34.SetMainMaxOrderQty(10);
        productAttributeOptionRole34.SetMainMinOrderQty(2);
        var productAttributeOptionRole35 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption18.Id
        };
        productAttributeOptionRole35.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole35.SetMainMaxOrderQty(10);
        productAttributeOptionRole35.SetMainMinOrderQty(2);
        var productAttributeOptionRole36 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption18.Id
        };
        productAttributeOptionRole36.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole36.SetMainMaxOrderQty(10);
        productAttributeOptionRole36.SetMainMinOrderQty(2);
        var productAttributeOptionRole37 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption19.Id
        };
        productAttributeOptionRole37.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole37.SetMainMaxOrderQty(10);
        productAttributeOptionRole37.SetMainMinOrderQty(2);
        var productAttributeOptionRole38 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption19.Id
        };
        productAttributeOptionRole38.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole38.SetMainMaxOrderQty(10);
        productAttributeOptionRole38.SetMainMinOrderQty(2);
        var productAttributeOptionRole39 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption20.Id
        };
        productAttributeOptionRole39.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole39.SetMainMaxOrderQty(10);
        productAttributeOptionRole39.SetMainMinOrderQty(2);
        var productAttributeOptionRole40 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption20.Id
        };
        productAttributeOptionRole40.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole40.SetMainMaxOrderQty(10);
        productAttributeOptionRole40.SetMainMinOrderQty(2);
        var productAttributeOptionRole41 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption21.Id
        };
        productAttributeOptionRole41.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole41.SetMainMaxOrderQty(10);
        productAttributeOptionRole41.SetMainMinOrderQty(2);
        var productAttributeOptionRole42 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption21.Id
        };
        productAttributeOptionRole42.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole42.SetMainMaxOrderQty(10);
        productAttributeOptionRole42.SetMainMinOrderQty(2);
        var productAttributeOptionRole43 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption22.Id
        };
        productAttributeOptionRole43.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole43.SetMainMaxOrderQty(10);
        productAttributeOptionRole43.SetMainMinOrderQty(2);
        var productAttributeOptionRole44 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption22.Id
        };
        productAttributeOptionRole44.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole44.SetMainMaxOrderQty(10);
        productAttributeOptionRole44.SetMainMinOrderQty(2);
        var productAttributeOptionRole45 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption23.Id
        };
        productAttributeOptionRole45.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole45.SetMainMaxOrderQty(10);
        productAttributeOptionRole45.SetMainMinOrderQty(2);
        var productAttributeOptionRole46 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption23.Id
        };
        productAttributeOptionRole46.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole46.SetMainMaxOrderQty(10);
        productAttributeOptionRole46.SetMainMinOrderQty(2);
        var productAttributeOptionRole47 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption24.Id
        };
        productAttributeOptionRole47.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole47.SetMainMaxOrderQty(10);
        productAttributeOptionRole47.SetMainMinOrderQty(2);
        var productAttributeOptionRole48 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption24.Id
        };
        productAttributeOptionRole48.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole48.SetMainMaxOrderQty(10);
        productAttributeOptionRole48.SetMainMinOrderQty(2);
        var productAttributeOptionRole49 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption25.Id
        };
        productAttributeOptionRole49.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole49.SetMainMaxOrderQty(10);
        productAttributeOptionRole49.SetMainMinOrderQty(2);
        var productAttributeOptionRole50 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption25.Id
        };
        productAttributeOptionRole50.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole50.SetMainMaxOrderQty(10);
        productAttributeOptionRole50.SetMainMinOrderQty(2);
        var productAttributeOptionRole51 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption26.Id
        };
        productAttributeOptionRole51.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole51.SetMainMaxOrderQty(10);
        productAttributeOptionRole51.SetMainMinOrderQty(2);
        var productAttributeOptionRole52 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption26.Id
        };
        productAttributeOptionRole52.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole52.SetMainMaxOrderQty(10);
        productAttributeOptionRole52.SetMainMinOrderQty(2);
        var productAttributeOptionRole53 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption27.Id
        };
        productAttributeOptionRole53.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole53.SetMainMaxOrderQty(10);
        productAttributeOptionRole53.SetMainMinOrderQty(2);
        var productAttributeOptionRole54 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption27.Id
        };
        productAttributeOptionRole54.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole54.SetMainMaxOrderQty(10);
        productAttributeOptionRole54.SetMainMinOrderQty(2);
        var productAttributeOptionRole55 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption28.Id
        };
        productAttributeOptionRole55.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole55.SetMainMaxOrderQty(10);
        productAttributeOptionRole55.SetMainMinOrderQty(2);
        var productAttributeOptionRole56 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption28.Id
        };
        productAttributeOptionRole56.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole56.SetMainMaxOrderQty(10);
        productAttributeOptionRole56.SetMainMinOrderQty(2);
        var productAttributeOptionRole57 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption29.Id
        };
        productAttributeOptionRole57.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole57.SetMainMaxOrderQty(10);
        productAttributeOptionRole57.SetMainMinOrderQty(2);
        var productAttributeOptionRole58 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption29.Id
        };
        productAttributeOptionRole58.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole58.SetMainMaxOrderQty(10);
        productAttributeOptionRole58.SetMainMinOrderQty(2);
        var productAttributeOptionRole59 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption30.Id
        };
        productAttributeOptionRole59.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole59.SetMainMaxOrderQty(10);
        productAttributeOptionRole59.SetMainMinOrderQty(2);
        var productAttributeOptionRole60 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption30.Id
        };
        productAttributeOptionRole60.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole60.SetMainMaxOrderQty(10);
        productAttributeOptionRole60.SetMainMinOrderQty(2);
        var productAttributeOptionRole61 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption31.Id
        };
        productAttributeOptionRole61.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole61.SetMainMaxOrderQty(10);
        productAttributeOptionRole61.SetMainMinOrderQty(2);
        var productAttributeOptionRole62 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption31.Id
        };
        productAttributeOptionRole62.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole62.SetMainMaxOrderQty(10);
        productAttributeOptionRole62.SetMainMinOrderQty(2);
        var productAttributeOptionRole63 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption32.Id
        };
        productAttributeOptionRole63.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole63.SetMainMaxOrderQty(10);
        productAttributeOptionRole63.SetMainMinOrderQty(2);
        var productAttributeOptionRole64 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption32.Id
        };
        productAttributeOptionRole64.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole64.SetMainMaxOrderQty(10);
        productAttributeOptionRole64.SetMainMinOrderQty(2);
        var productAttributeOptionRole65 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption33.Id
        };
        productAttributeOptionRole65.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole65.SetMainMaxOrderQty(10);
        productAttributeOptionRole65.SetMainMinOrderQty(2);
        var productAttributeOptionRole66 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption33.Id
        };
        productAttributeOptionRole66.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole66.SetMainMaxOrderQty(10);
        productAttributeOptionRole66.SetMainMinOrderQty(2);
        var productAttributeOptionRole67 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption34.Id
        };
        productAttributeOptionRole67.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole67.SetMainMaxOrderQty(10);
        productAttributeOptionRole67.SetMainMinOrderQty(2);
        var productAttributeOptionRole68 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption34.Id
        };
        productAttributeOptionRole68.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole68.SetMainMaxOrderQty(10);
        productAttributeOptionRole68.SetMainMinOrderQty(2);
        var productAttributeOptionRole69 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption35.Id
        };
        productAttributeOptionRole69.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole69.SetMainMaxOrderQty(10);
        productAttributeOptionRole69.SetMainMinOrderQty(2);
        var productAttributeOptionRole70 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption35.Id
        };
        productAttributeOptionRole70.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole70.SetMainMaxOrderQty(10);
        productAttributeOptionRole70.SetMainMinOrderQty(2);
        var productAttributeOptionRole71 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption36.Id
        };
        productAttributeOptionRole71.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole71.SetMainMaxOrderQty(10);
        productAttributeOptionRole71.SetMainMinOrderQty(2);
        var productAttributeOptionRole72 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption36.Id
        };
        productAttributeOptionRole72.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole72.SetMainMaxOrderQty(10);
        productAttributeOptionRole72.SetMainMinOrderQty(2);
        var productAttributeOptionRole73 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption37.Id
        };
        productAttributeOptionRole73.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole73.SetMainMaxOrderQty(10);
        productAttributeOptionRole73.SetMainMinOrderQty(2);
        var productAttributeOptionRole74 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption37.Id
        };
        productAttributeOptionRole74.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole74.SetMainMaxOrderQty(10);
        productAttributeOptionRole74.SetMainMinOrderQty(2);
        var productAttributeOptionRole75 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption38.Id
        };
        productAttributeOptionRole75.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole75.SetMainMaxOrderQty(10);
        productAttributeOptionRole75.SetMainMinOrderQty(2);
        var productAttributeOptionRole76 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption38.Id
        };
        productAttributeOptionRole76.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole76.SetMainMaxOrderQty(10);
        productAttributeOptionRole76.SetMainMinOrderQty(2);
        var productAttributeOptionRole77 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption39.Id
        };
        productAttributeOptionRole77.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole77.SetMainMaxOrderQty(10);
        productAttributeOptionRole77.SetMainMinOrderQty(2);
        var productAttributeOptionRole78 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption39.Id
        };
        productAttributeOptionRole78.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole78.SetMainMaxOrderQty(10);
        productAttributeOptionRole78.SetMainMinOrderQty(2);
        var productAttributeOptionRole79 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption40.Id
        };
        productAttributeOptionRole79.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole79.SetMainMaxOrderQty(10);
        productAttributeOptionRole79.SetMainMinOrderQty(2);
        var productAttributeOptionRole80 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption40.Id
        };
        productAttributeOptionRole80.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole80.SetMainMaxOrderQty(10);
        productAttributeOptionRole80.SetMainMinOrderQty(2);
        var productAttributeOptionRole81 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption41.Id
        };
        productAttributeOptionRole81.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole81.SetMainMaxOrderQty(10);
        productAttributeOptionRole81.SetMainMinOrderQty(2);
        var productAttributeOptionRole82 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption41.Id
        };
        productAttributeOptionRole82.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole82.SetMainMaxOrderQty(10);
        productAttributeOptionRole82.SetMainMinOrderQty(2);
        var productAttributeOptionRole83 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption42.Id
        };
        productAttributeOptionRole83.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole83.SetMainMaxOrderQty(10);
        productAttributeOptionRole83.SetMainMinOrderQty(2);
        var productAttributeOptionRole84 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption42.Id
        };
        productAttributeOptionRole84.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole84.SetMainMaxOrderQty(10);
        productAttributeOptionRole84.SetMainMinOrderQty(2);
        var productAttributeOptionRole85 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption43.Id
        };
        productAttributeOptionRole85.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole85.SetMainMaxOrderQty(10);
        productAttributeOptionRole85.SetMainMinOrderQty(2);
        var productAttributeOptionRole86 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption43.Id
        };
        productAttributeOptionRole86.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole86.SetMainMaxOrderQty(10);
        productAttributeOptionRole86.SetMainMinOrderQty(2);
        var productAttributeOptionRole87 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption44.Id
        };
        productAttributeOptionRole87.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole87.SetMainMaxOrderQty(10);
        productAttributeOptionRole87.SetMainMinOrderQty(2);
        var productAttributeOptionRole88 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption44.Id
        };
        productAttributeOptionRole88.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole88.SetMainMaxOrderQty(10);
        productAttributeOptionRole88.SetMainMinOrderQty(2);
        var productAttributeOptionRole89 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption45.Id
        };
        productAttributeOptionRole89.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole89.SetMainMaxOrderQty(10);
        productAttributeOptionRole89.SetMainMinOrderQty(2);
        var productAttributeOptionRole90 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption45.Id
        };
        productAttributeOptionRole90.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole90.SetMainMaxOrderQty(10);
        productAttributeOptionRole90.SetMainMinOrderQty(2);
        var productAttributeOptionRole91 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption46.Id
        };
        productAttributeOptionRole91.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole91.SetMainMaxOrderQty(10);
        productAttributeOptionRole91.SetMainMinOrderQty(2);
        var productAttributeOptionRole92 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption46.Id
        };
        productAttributeOptionRole92.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole92.SetMainMaxOrderQty(10);
        productAttributeOptionRole92.SetMainMinOrderQty(2);
        var productAttributeOptionRole93 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption47.Id
        };
        productAttributeOptionRole93.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole93.SetMainMaxOrderQty(10);
        productAttributeOptionRole93.SetMainMinOrderQty(2);
        var productAttributeOptionRole94 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption47.Id
        };
        productAttributeOptionRole94.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole94.SetMainMaxOrderQty(10);
        productAttributeOptionRole94.SetMainMinOrderQty(2);
        var productAttributeOptionRole95 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption48.Id
        };
        productAttributeOptionRole95.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole95.SetMainMaxOrderQty(10);
        productAttributeOptionRole95.SetMainMinOrderQty(2);
        var productAttributeOptionRole96 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption48.Id
        };
        productAttributeOptionRole96.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole96.SetMainMaxOrderQty(10);
        productAttributeOptionRole96.SetMainMinOrderQty(2);
        var productAttributeOptionRole97 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption49.Id
        };
        productAttributeOptionRole97.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole97.SetMainMaxOrderQty(10);
        productAttributeOptionRole97.SetMainMinOrderQty(2);
        var productAttributeOptionRole98 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption49.Id
        };
        productAttributeOptionRole98.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole98.SetMainMaxOrderQty(10);
        productAttributeOptionRole98.SetMainMinOrderQty(2);
        var productAttributeOptionRole99 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption50.Id
        };
        productAttributeOptionRole99.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole99.SetMainMaxOrderQty(10);
        productAttributeOptionRole99.SetMainMinOrderQty(2);
        var productAttributeOptionRole100 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption50.Id
        };
        productAttributeOptionRole100.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole100.SetMainMaxOrderQty(10);
        productAttributeOptionRole100.SetMainMinOrderQty(2);
        var productAttributeOptionRole101 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption51.Id
        };
        productAttributeOptionRole101.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole101.SetMainMaxOrderQty(10);
        productAttributeOptionRole101.SetMainMinOrderQty(2);
        var productAttributeOptionRole102 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption51.Id
        };
        productAttributeOptionRole102.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole102.SetMainMaxOrderQty(10);
        productAttributeOptionRole102.SetMainMinOrderQty(2);
        var productAttributeOptionRole103 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption52.Id
        };
        productAttributeOptionRole103.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole103.SetMainMaxOrderQty(10);
        productAttributeOptionRole103.SetMainMinOrderQty(2);
        var productAttributeOptionRole104 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption52.Id
        };
        productAttributeOptionRole104.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole104.SetMainMaxOrderQty(10);
        productAttributeOptionRole104.SetMainMinOrderQty(2);
        var productAttributeOptionRole105 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption53.Id
        };
        productAttributeOptionRole105.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole105.SetMainMaxOrderQty(10);
        productAttributeOptionRole105.SetMainMinOrderQty(2);
        var productAttributeOptionRole106 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption53.Id
        };
        productAttributeOptionRole106.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole106.SetMainMaxOrderQty(10);
        productAttributeOptionRole106.SetMainMinOrderQty(2);
        var productAttributeOptionRole107 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption54.Id
        };
        productAttributeOptionRole107.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole107.SetMainMaxOrderQty(10);
        productAttributeOptionRole107.SetMainMinOrderQty(2);
        var productAttributeOptionRole108 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption54.Id
        };
        productAttributeOptionRole108.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole108.SetMainMaxOrderQty(10);
        productAttributeOptionRole108.SetMainMinOrderQty(2);
        var productAttributeOptionRole109 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption55.Id
        };
        productAttributeOptionRole109.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole109.SetMainMaxOrderQty(10);
        productAttributeOptionRole109.SetMainMinOrderQty(2);
        var productAttributeOptionRole110 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption55.Id
        };
        productAttributeOptionRole110.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole110.SetMainMaxOrderQty(10);
        productAttributeOptionRole110.SetMainMinOrderQty(2);
        var productAttributeOptionRole111 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption56.Id
        };
        productAttributeOptionRole111.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole111.SetMainMaxOrderQty(10);
        productAttributeOptionRole111.SetMainMinOrderQty(2);
        var productAttributeOptionRole112 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption56.Id
        };
        productAttributeOptionRole112.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole112.SetMainMaxOrderQty(10);
        productAttributeOptionRole112.SetMainMinOrderQty(2);
        var productAttributeOptionRole113 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption57.Id
        };
        productAttributeOptionRole113.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole113.SetMainMaxOrderQty(10);
        productAttributeOptionRole113.SetMainMinOrderQty(2);
        var productAttributeOptionRole114 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption57.Id
        };
        productAttributeOptionRole114.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole114.SetMainMaxOrderQty(10);
        productAttributeOptionRole114.SetMainMinOrderQty(2);
        var productAttributeOptionRole115 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption58.Id
        };
        productAttributeOptionRole115.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole115.SetMainMaxOrderQty(10);
        productAttributeOptionRole115.SetMainMinOrderQty(2);
        var productAttributeOptionRole116 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption58.Id
        };
        productAttributeOptionRole116.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole116.SetMainMaxOrderQty(10);
        productAttributeOptionRole116.SetMainMinOrderQty(2);
        var productAttributeOptionRole117 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption59.Id
        };
        productAttributeOptionRole117.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole117.SetMainMaxOrderQty(10);
        productAttributeOptionRole117.SetMainMinOrderQty(2);
        var productAttributeOptionRole118 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption59.Id
        };
        productAttributeOptionRole118.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole118.SetMainMaxOrderQty(10);
        productAttributeOptionRole118.SetMainMinOrderQty(2);
        var productAttributeOptionRole119 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption60.Id
        };
        productAttributeOptionRole119.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole119.SetMainMaxOrderQty(10);
        productAttributeOptionRole119.SetMainMinOrderQty(2);
        var productAttributeOptionRole120 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption60.Id
        };
        productAttributeOptionRole120.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole120.SetMainMaxOrderQty(10);
        productAttributeOptionRole120.SetMainMinOrderQty(2);
        var productAttributeOptionRole121 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption61.Id
        };
        productAttributeOptionRole121.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole121.SetMainMaxOrderQty(10);
        productAttributeOptionRole121.SetMainMinOrderQty(2);
        var productAttributeOptionRole122 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption61.Id
        };
        productAttributeOptionRole122.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole122.SetMainMaxOrderQty(10);
        productAttributeOptionRole122.SetMainMinOrderQty(2);
        var productAttributeOptionRole123 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption62.Id
        };
        productAttributeOptionRole123.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole123.SetMainMaxOrderQty(10);
        productAttributeOptionRole123.SetMainMinOrderQty(2);
        var productAttributeOptionRole124 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption62.Id
        };
        productAttributeOptionRole124.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole124.SetMainMaxOrderQty(10);
        productAttributeOptionRole124.SetMainMinOrderQty(2);
        var productAttributeOptionRole125 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption63.Id
        };
        productAttributeOptionRole125.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole125.SetMainMaxOrderQty(10);
        productAttributeOptionRole125.SetMainMinOrderQty(2);
        var productAttributeOptionRole126 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption63.Id
        };
        productAttributeOptionRole126.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole126.SetMainMaxOrderQty(10);
        productAttributeOptionRole126.SetMainMinOrderQty(2);
        var productAttributeOptionRole127 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption64.Id
        };
        productAttributeOptionRole127.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole127.SetMainMaxOrderQty(10);
        productAttributeOptionRole127.SetMainMinOrderQty(2);
        var productAttributeOptionRole128 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption64.Id
        };
        productAttributeOptionRole128.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole128.SetMainMaxOrderQty(10);
        productAttributeOptionRole128.SetMainMinOrderQty(2);
        var productAttributeOptionRole129 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption65.Id
        };
        productAttributeOptionRole129.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole129.SetMainMaxOrderQty(10);
        productAttributeOptionRole129.SetMainMinOrderQty(2);
        var productAttributeOptionRole130 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption65.Id
        };
        productAttributeOptionRole130.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole130.SetMainMaxOrderQty(10);
        productAttributeOptionRole130.SetMainMinOrderQty(2);
        var productAttributeOptionRole131 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption66.Id
        };
        productAttributeOptionRole131.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole131.SetMainMaxOrderQty(10);
        productAttributeOptionRole131.SetMainMinOrderQty(2);
        var productAttributeOptionRole132 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption66.Id
        };
        productAttributeOptionRole132.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole132.SetMainMaxOrderQty(10);
        productAttributeOptionRole132.SetMainMinOrderQty(2);
        var productAttributeOptionRole133 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption67.Id
        };
        productAttributeOptionRole133.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole133.SetMainMaxOrderQty(10);
        productAttributeOptionRole133.SetMainMinOrderQty(2);
        var productAttributeOptionRole134 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption67.Id
        };
        productAttributeOptionRole134.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole134.SetMainMaxOrderQty(10);
        productAttributeOptionRole134.SetMainMinOrderQty(2);
        var productAttributeOptionRole135 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption68.Id
        };
        productAttributeOptionRole135.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole135.SetMainMaxOrderQty(10);
        productAttributeOptionRole135.SetMainMinOrderQty(2);
        var productAttributeOptionRole136 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption68.Id
        };
        productAttributeOptionRole136.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole136.SetMainMaxOrderQty(10);
        productAttributeOptionRole136.SetMainMinOrderQty(2);
        var productAttributeOptionRole137 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption69.Id
        };
        productAttributeOptionRole137.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole137.SetMainMaxOrderQty(10);
        productAttributeOptionRole137.SetMainMinOrderQty(2);
        var productAttributeOptionRole138 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption69.Id
        };
        productAttributeOptionRole138.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole138.SetMainMaxOrderQty(10);
        productAttributeOptionRole138.SetMainMinOrderQty(2);
        var productAttributeOptionRole139 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption70.Id
        };
        productAttributeOptionRole139.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole139.SetMainMaxOrderQty(10);
        productAttributeOptionRole139.SetMainMinOrderQty(2);
        var productAttributeOptionRole140 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption70.Id
        };
        productAttributeOptionRole140.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole140.SetMainMaxOrderQty(10);
        productAttributeOptionRole140.SetMainMinOrderQty(2);
        var productAttributeOptionRole141 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption71.Id
        };
        productAttributeOptionRole141.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole141.SetMainMaxOrderQty(10);
        productAttributeOptionRole141.SetMainMinOrderQty(2);
        var productAttributeOptionRole142 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption71.Id
        };
        productAttributeOptionRole142.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole142.SetMainMaxOrderQty(10);
        productAttributeOptionRole142.SetMainMinOrderQty(2);
        var productAttributeOptionRole143 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption72.Id
        };
        productAttributeOptionRole143.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole143.SetMainMaxOrderQty(10);
        productAttributeOptionRole143.SetMainMinOrderQty(2);
        var productAttributeOptionRole144 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption72.Id
        };
        productAttributeOptionRole144.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole144.SetMainMaxOrderQty(10);
        productAttributeOptionRole144.SetMainMinOrderQty(2);
        var productAttributeOptionRole145 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption73.Id
        };
        productAttributeOptionRole145.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole145.SetMainMaxOrderQty(10);
        productAttributeOptionRole145.SetMainMinOrderQty(2);
        var productAttributeOptionRole146 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption73.Id
        };
        productAttributeOptionRole146.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole146.SetMainMaxOrderQty(10);
        productAttributeOptionRole146.SetMainMinOrderQty(2);
        var productAttributeOptionRole147 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption74.Id
        };
        productAttributeOptionRole147.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole147.SetMainMaxOrderQty(10);
        productAttributeOptionRole147.SetMainMinOrderQty(2);
        var productAttributeOptionRole148 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption74.Id
        };
        productAttributeOptionRole148.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole148.SetMainMaxOrderQty(10);
        productAttributeOptionRole148.SetMainMinOrderQty(2);
        var productAttributeOptionRole149 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption75.Id
        };
        productAttributeOptionRole149.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole149.SetMainMaxOrderQty(10);
        productAttributeOptionRole149.SetMainMinOrderQty(2);
        var productAttributeOptionRole150 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption75.Id
        };
        productAttributeOptionRole150.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole150.SetMainMaxOrderQty(10);
        productAttributeOptionRole150.SetMainMinOrderQty(2);
        var productAttributeOptionRole151 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption76.Id
        };
        productAttributeOptionRole151.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole151.SetMainMaxOrderQty(10);
        productAttributeOptionRole151.SetMainMinOrderQty(2);
        var productAttributeOptionRole152 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption76.Id
        };
        productAttributeOptionRole152.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole152.SetMainMaxOrderQty(10);
        productAttributeOptionRole152.SetMainMinOrderQty(2);
        var productAttributeOptionRole153 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption77.Id
        };
        productAttributeOptionRole153.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole153.SetMainMaxOrderQty(10);
        productAttributeOptionRole153.SetMainMinOrderQty(2);
        var productAttributeOptionRole154 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption77.Id
        };
        productAttributeOptionRole154.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole154.SetMainMaxOrderQty(10);
        productAttributeOptionRole154.SetMainMinOrderQty(2);
        var productAttributeOptionRole155 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption78.Id
        };
        productAttributeOptionRole155.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole155.SetMainMaxOrderQty(10);
        productAttributeOptionRole155.SetMainMinOrderQty(2);
        var productAttributeOptionRole156 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption78.Id
        };
        productAttributeOptionRole156.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole156.SetMainMaxOrderQty(10);
        productAttributeOptionRole156.SetMainMinOrderQty(2);
        var productAttributeOptionRole157 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption79.Id
        };
        productAttributeOptionRole157.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole157.SetMainMaxOrderQty(10);
        productAttributeOptionRole157.SetMainMinOrderQty(2);
        var productAttributeOptionRole158 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption79.Id
        };
        productAttributeOptionRole158.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole158.SetMainMaxOrderQty(10);
        productAttributeOptionRole158.SetMainMinOrderQty(2);
        var productAttributeOptionRole159 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption80.Id
        };
        productAttributeOptionRole159.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole159.SetMainMaxOrderQty(10);
        productAttributeOptionRole159.SetMainMinOrderQty(2);
        var productAttributeOptionRole160 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption80.Id
        };
        productAttributeOptionRole160.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole160.SetMainMaxOrderQty(10);
        productAttributeOptionRole160.SetMainMinOrderQty(2);
        var productAttributeOptionRole161 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption81.Id
        };
        productAttributeOptionRole161.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole161.SetMainMaxOrderQty(10);
        productAttributeOptionRole161.SetMainMinOrderQty(2);
        var productAttributeOptionRole162 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption81.Id
        };
        productAttributeOptionRole162.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole162.SetMainMaxOrderQty(10);
        productAttributeOptionRole162.SetMainMinOrderQty(2);
        var productAttributeOptionRole163 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption82.Id
        };
        productAttributeOptionRole163.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole163.SetMainMaxOrderQty(10);
        productAttributeOptionRole163.SetMainMinOrderQty(2);
        var productAttributeOptionRole164 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption82.Id
        };
        productAttributeOptionRole164.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole164.SetMainMaxOrderQty(10);
        productAttributeOptionRole164.SetMainMinOrderQty(2);
        var productAttributeOptionRole165 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption83.Id
        };
        productAttributeOptionRole165.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole165.SetMainMaxOrderQty(10);
        productAttributeOptionRole165.SetMainMinOrderQty(2);
        var productAttributeOptionRole166 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption83.Id
        };
        productAttributeOptionRole166.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole166.SetMainMaxOrderQty(10);
        productAttributeOptionRole166.SetMainMinOrderQty(2);
        var productAttributeOptionRole167 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption84.Id
        };
        productAttributeOptionRole167.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole167.SetMainMaxOrderQty(10);
        productAttributeOptionRole167.SetMainMinOrderQty(2);
        var productAttributeOptionRole168 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption84.Id
        };
        productAttributeOptionRole168.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole168.SetMainMaxOrderQty(10);
        productAttributeOptionRole168.SetMainMinOrderQty(2);
        var productAttributeOptionRole169 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption85.Id
        };
        productAttributeOptionRole169.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole169.SetMainMaxOrderQty(10);
        productAttributeOptionRole169.SetMainMinOrderQty(2);
        var productAttributeOptionRole170 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption85.Id
        };
        productAttributeOptionRole170.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole170.SetMainMaxOrderQty(10);
        productAttributeOptionRole170.SetMainMinOrderQty(2);
        var productAttributeOptionRole171 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption86.Id
        };
        productAttributeOptionRole171.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole171.SetMainMaxOrderQty(10);
        productAttributeOptionRole171.SetMainMinOrderQty(2);
        var productAttributeOptionRole172 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption86.Id
        };
        productAttributeOptionRole172.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole172.SetMainMaxOrderQty(10);
        productAttributeOptionRole172.SetMainMinOrderQty(2);
        var productAttributeOptionRole173 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption87.Id
        };
        productAttributeOptionRole173.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole173.SetMainMaxOrderQty(10);
        productAttributeOptionRole173.SetMainMinOrderQty(2);
        var productAttributeOptionRole174 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption87.Id
        };
        productAttributeOptionRole174.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole174.SetMainMaxOrderQty(10);
        productAttributeOptionRole174.SetMainMinOrderQty(2);
        var productAttributeOptionRole175 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption88.Id
        };
        productAttributeOptionRole175.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole175.SetMainMaxOrderQty(10);
        productAttributeOptionRole175.SetMainMinOrderQty(2);
        var productAttributeOptionRole176 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption88.Id
        };
        productAttributeOptionRole176.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole176.SetMainMaxOrderQty(10);
        productAttributeOptionRole176.SetMainMinOrderQty(2);
        var productAttributeOptionRole177 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption89.Id
        };
        productAttributeOptionRole177.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole177.SetMainMaxOrderQty(10);
        productAttributeOptionRole177.SetMainMinOrderQty(2);
        var productAttributeOptionRole178 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption89.Id
        };
        productAttributeOptionRole178.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole178.SetMainMaxOrderQty(10);
        productAttributeOptionRole178.SetMainMinOrderQty(2);
        var productAttributeOptionRole179 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption90.Id
        };
        productAttributeOptionRole179.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole179.SetMainMaxOrderQty(10);
        productAttributeOptionRole179.SetMainMinOrderQty(2);
        var productAttributeOptionRole180 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption90.Id
        };
        productAttributeOptionRole180.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole180.SetMainMaxOrderQty(10);
        productAttributeOptionRole180.SetMainMinOrderQty(2);
        var productAttributeOptionRole181 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption91.Id
        };
        productAttributeOptionRole181.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole181.SetMainMaxOrderQty(10);
        productAttributeOptionRole181.SetMainMinOrderQty(2);
        var productAttributeOptionRole182 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption91.Id
        };
        productAttributeOptionRole182.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole182.SetMainMaxOrderQty(10);
        productAttributeOptionRole182.SetMainMinOrderQty(2);
        var productAttributeOptionRole183 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption92.Id
        };
        productAttributeOptionRole183.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole183.SetMainMaxOrderQty(10);
        productAttributeOptionRole183.SetMainMinOrderQty(2);
        var productAttributeOptionRole184 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption92.Id
        };
        productAttributeOptionRole184.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole184.SetMainMaxOrderQty(10);
        productAttributeOptionRole184.SetMainMinOrderQty(2);
        var productAttributeOptionRole185 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption93.Id
        };
        productAttributeOptionRole185.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole185.SetMainMaxOrderQty(10);
        productAttributeOptionRole185.SetMainMinOrderQty(2);
        var productAttributeOptionRole186 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption93.Id
        };
        productAttributeOptionRole186.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole186.SetMainMaxOrderQty(10);
        productAttributeOptionRole186.SetMainMinOrderQty(2);
        var productAttributeOptionRole187 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption94.Id
        };
        productAttributeOptionRole187.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole187.SetMainMaxOrderQty(10);
        productAttributeOptionRole187.SetMainMinOrderQty(2);
        var productAttributeOptionRole188 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption94.Id
        };
        productAttributeOptionRole188.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole188.SetMainMaxOrderQty(10);
        productAttributeOptionRole188.SetMainMinOrderQty(2);
        var productAttributeOptionRole189 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption95.Id
        };
        productAttributeOptionRole189.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole189.SetMainMaxOrderQty(10);
        productAttributeOptionRole189.SetMainMinOrderQty(2);
        var productAttributeOptionRole190 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption95.Id
        };
        productAttributeOptionRole190.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole190.SetMainMaxOrderQty(10);
        productAttributeOptionRole190.SetMainMinOrderQty(2);
        var productAttributeOptionRole191 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption96.Id
        };
        productAttributeOptionRole191.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole191.SetMainMaxOrderQty(10);
        productAttributeOptionRole191.SetMainMinOrderQty(2);
        var productAttributeOptionRole192 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption96.Id
        };
        productAttributeOptionRole192.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole192.SetMainMaxOrderQty(10);
        productAttributeOptionRole192.SetMainMinOrderQty(2);
        var productAttributeOptionRole193 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption97.Id
        };
        productAttributeOptionRole193.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole193.SetMainMaxOrderQty(10);
        productAttributeOptionRole193.SetMainMinOrderQty(2);
        var productAttributeOptionRole194 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption97.Id
        };
        productAttributeOptionRole194.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole194.SetMainMaxOrderQty(10);
        productAttributeOptionRole194.SetMainMinOrderQty(2);
        var productAttributeOptionRole195 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption98.Id
        };
        productAttributeOptionRole195.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole195.SetMainMaxOrderQty(10);
        productAttributeOptionRole195.SetMainMinOrderQty(2);
        var productAttributeOptionRole196 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption98.Id
        };
        productAttributeOptionRole196.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole196.SetMainMaxOrderQty(10);
        productAttributeOptionRole196.SetMainMinOrderQty(2);
        var productAttributeOptionRole197 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption99.Id
        };
        productAttributeOptionRole197.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole197.SetMainMaxOrderQty(10);
        productAttributeOptionRole197.SetMainMinOrderQty(2);
        var productAttributeOptionRole198 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption99.Id
        };
        productAttributeOptionRole198.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole198.SetMainMaxOrderQty(10);
        productAttributeOptionRole198.SetMainMinOrderQty(2);
        var productAttributeOptionRole199 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption100.Id
        };
        productAttributeOptionRole199.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole199.SetMainMaxOrderQty(10);
        productAttributeOptionRole199.SetMainMinOrderQty(2);
        var productAttributeOptionRole200 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption100.Id
        };
        productAttributeOptionRole200.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole200.SetMainMaxOrderQty(10);
        productAttributeOptionRole200.SetMainMinOrderQty(2);
        var productAttributeOptionRole201 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption101.Id
        };
        productAttributeOptionRole201.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole201.SetMainMaxOrderQty(10);
        productAttributeOptionRole201.SetMainMinOrderQty(2);
        var productAttributeOptionRole202 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption101.Id
        };
        productAttributeOptionRole202.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole202.SetMainMaxOrderQty(10);
        productAttributeOptionRole202.SetMainMinOrderQty(2);
        var productAttributeOptionRole203 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption102.Id
        };
        productAttributeOptionRole203.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole203.SetMainMaxOrderQty(10);
        productAttributeOptionRole203.SetMainMinOrderQty(2);
        var productAttributeOptionRole204 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption102.Id
        };
        productAttributeOptionRole204.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole204.SetMainMaxOrderQty(10);
        productAttributeOptionRole204.SetMainMinOrderQty(2);
        var productAttributeOptionRole205 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption103.Id
        };
        productAttributeOptionRole205.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole205.SetMainMaxOrderQty(10);
        productAttributeOptionRole205.SetMainMinOrderQty(2);
        var productAttributeOptionRole206 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption103.Id
        };
        productAttributeOptionRole206.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole206.SetMainMaxOrderQty(10);
        productAttributeOptionRole206.SetMainMinOrderQty(2);
        var productAttributeOptionRole207 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption104.Id
        };
        productAttributeOptionRole207.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole207.SetMainMaxOrderQty(10);
        productAttributeOptionRole207.SetMainMinOrderQty(2);
        var productAttributeOptionRole208 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption104.Id
        };
        productAttributeOptionRole208.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole208.SetMainMaxOrderQty(10);
        productAttributeOptionRole208.SetMainMinOrderQty(2);
        var productAttributeOptionRole209 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption105.Id
        };
        productAttributeOptionRole209.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole209.SetMainMaxOrderQty(10);
        productAttributeOptionRole209.SetMainMinOrderQty(2);
        var productAttributeOptionRole210 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption105.Id
        };
        productAttributeOptionRole210.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole210.SetMainMaxOrderQty(10);
        productAttributeOptionRole210.SetMainMinOrderQty(2);
        var productAttributeOptionRole211 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption106.Id
        };
        productAttributeOptionRole211.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole211.SetMainMaxOrderQty(10);
        productAttributeOptionRole211.SetMainMinOrderQty(2);
        var productAttributeOptionRole212 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption106.Id
        };
        productAttributeOptionRole212.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole212.SetMainMaxOrderQty(10);
        productAttributeOptionRole212.SetMainMinOrderQty(2);
        var productAttributeOptionRole213 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption107.Id
        };
        productAttributeOptionRole213.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole213.SetMainMaxOrderQty(10);
        productAttributeOptionRole213.SetMainMinOrderQty(2);
        var productAttributeOptionRole214 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption107.Id
        };
        productAttributeOptionRole214.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole214.SetMainMaxOrderQty(10);
        productAttributeOptionRole214.SetMainMinOrderQty(2);
        var productAttributeOptionRole215 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption108.Id
        };
        productAttributeOptionRole215.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole215.SetMainMaxOrderQty(10);
        productAttributeOptionRole215.SetMainMinOrderQty(2);
        var productAttributeOptionRole216 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption108.Id
        };
        productAttributeOptionRole216.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole216.SetMainMaxOrderQty(10);
        productAttributeOptionRole216.SetMainMinOrderQty(2);
        var productAttributeOptionRole217 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption109.Id
        };
        productAttributeOptionRole217.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole217.SetMainMaxOrderQty(10);
        productAttributeOptionRole217.SetMainMinOrderQty(2);
        var productAttributeOptionRole218 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption109.Id
        };
        productAttributeOptionRole218.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole218.SetMainMaxOrderQty(10);
        productAttributeOptionRole218.SetMainMinOrderQty(2);
        var productAttributeOptionRole219 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption110.Id
        };
        productAttributeOptionRole219.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole219.SetMainMaxOrderQty(10);
        productAttributeOptionRole219.SetMainMinOrderQty(2);
        var productAttributeOptionRole220 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption110.Id
        };
        productAttributeOptionRole220.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole220.SetMainMaxOrderQty(10);
        productAttributeOptionRole220.SetMainMinOrderQty(2);
        var productAttributeOptionRole221 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption111.Id
        };
        productAttributeOptionRole221.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole221.SetMainMaxOrderQty(10);
        productAttributeOptionRole221.SetMainMinOrderQty(2);
        var productAttributeOptionRole222 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption111.Id
        };
        productAttributeOptionRole222.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole222.SetMainMaxOrderQty(10);
        productAttributeOptionRole222.SetMainMinOrderQty(2);
        var productAttributeOptionRole223 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption112.Id
        };
        productAttributeOptionRole223.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole223.SetMainMaxOrderQty(10);
        productAttributeOptionRole223.SetMainMinOrderQty(2);
        var productAttributeOptionRole224 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption112.Id
        };
        productAttributeOptionRole224.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole224.SetMainMaxOrderQty(10);
        productAttributeOptionRole224.SetMainMinOrderQty(2);
        var productAttributeOptionRole225 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption113.Id
        };
        productAttributeOptionRole225.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole225.SetMainMaxOrderQty(10);
        productAttributeOptionRole225.SetMainMinOrderQty(2);
        var productAttributeOptionRole226 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption113.Id
        };
        productAttributeOptionRole226.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole226.SetMainMaxOrderQty(10);
        productAttributeOptionRole226.SetMainMinOrderQty(2);
        var productAttributeOptionRole227 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption114.Id
        };
        productAttributeOptionRole227.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole227.SetMainMaxOrderQty(10);
        productAttributeOptionRole227.SetMainMinOrderQty(2);
        var productAttributeOptionRole228 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption114.Id
        };
        productAttributeOptionRole228.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole228.SetMainMaxOrderQty(10);
        productAttributeOptionRole228.SetMainMinOrderQty(2);
        var productAttributeOptionRole229 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption115.Id
        };
        productAttributeOptionRole229.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole229.SetMainMaxOrderQty(10);
        productAttributeOptionRole229.SetMainMinOrderQty(2);
        var productAttributeOptionRole230 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption115.Id
        };
        productAttributeOptionRole230.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole230.SetMainMaxOrderQty(10);
        productAttributeOptionRole230.SetMainMinOrderQty(2);
        var productAttributeOptionRole231 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption116.Id
        };
        productAttributeOptionRole231.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole231.SetMainMaxOrderQty(10);
        productAttributeOptionRole231.SetMainMinOrderQty(2);
        var productAttributeOptionRole232 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption116.Id
        };
        productAttributeOptionRole232.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole232.SetMainMaxOrderQty(10);
        productAttributeOptionRole232.SetMainMinOrderQty(2);
        var productAttributeOptionRole233 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption117.Id
        };
        productAttributeOptionRole233.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole233.SetMainMaxOrderQty(10);
        productAttributeOptionRole233.SetMainMinOrderQty(2);
        var productAttributeOptionRole234 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption117.Id
        };
        productAttributeOptionRole234.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole234.SetMainMaxOrderQty(10);
        productAttributeOptionRole234.SetMainMinOrderQty(2);
        var productAttributeOptionRole235 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption118.Id
        };
        productAttributeOptionRole235.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole235.SetMainMaxOrderQty(10);
        productAttributeOptionRole235.SetMainMinOrderQty(2);
        var productAttributeOptionRole236 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption118.Id
        };
        productAttributeOptionRole236.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole236.SetMainMaxOrderQty(10);
        productAttributeOptionRole236.SetMainMinOrderQty(2);
        var productAttributeOptionRole237 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption119.Id
        };
        productAttributeOptionRole237.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole237.SetMainMaxOrderQty(10);
        productAttributeOptionRole237.SetMainMinOrderQty(2);
        var productAttributeOptionRole238 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption119.Id
        };
        productAttributeOptionRole238.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole238.SetMainMaxOrderQty(10);
        productAttributeOptionRole238.SetMainMinOrderQty(2);
        var productAttributeOptionRole239 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption120.Id
        };
        productAttributeOptionRole239.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole239.SetMainMaxOrderQty(10);
        productAttributeOptionRole239.SetMainMinOrderQty(2);
        var productAttributeOptionRole240 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption120.Id
        };
        productAttributeOptionRole240.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole240.SetMainMaxOrderQty(10);
        productAttributeOptionRole240.SetMainMinOrderQty(2);
        var productAttributeOptionRole241 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption121.Id
        };
        productAttributeOptionRole241.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole241.SetMainMaxOrderQty(10);
        productAttributeOptionRole241.SetMainMinOrderQty(2);
        var productAttributeOptionRole242 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption121.Id
        };
        productAttributeOptionRole242.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole242.SetMainMaxOrderQty(10);
        productAttributeOptionRole242.SetMainMinOrderQty(2);
        var productAttributeOptionRole243 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption122.Id
        };
        productAttributeOptionRole243.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole243.SetMainMaxOrderQty(10);
        productAttributeOptionRole243.SetMainMinOrderQty(2);
        var productAttributeOptionRole244 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption122.Id
        };
        productAttributeOptionRole244.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole244.SetMainMaxOrderQty(10);
        productAttributeOptionRole244.SetMainMinOrderQty(2);
        var productAttributeOptionRole245 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption123.Id
        };
        productAttributeOptionRole245.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole245.SetMainMaxOrderQty(10);
        productAttributeOptionRole245.SetMainMinOrderQty(2);
        var productAttributeOptionRole246 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption123.Id
        };
        productAttributeOptionRole246.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole246.SetMainMaxOrderQty(10);
        productAttributeOptionRole246.SetMainMinOrderQty(2);
        var productAttributeOptionRole247 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption124.Id
        };
        productAttributeOptionRole247.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole247.SetMainMaxOrderQty(10);
        productAttributeOptionRole247.SetMainMinOrderQty(2);
        var productAttributeOptionRole248 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption124.Id
        };
        productAttributeOptionRole248.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole248.SetMainMaxOrderQty(10);
        productAttributeOptionRole248.SetMainMinOrderQty(2);
        var productAttributeOptionRole249 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption125.Id
        };
        productAttributeOptionRole249.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole249.SetMainMaxOrderQty(10);
        productAttributeOptionRole249.SetMainMinOrderQty(2);
        var productAttributeOptionRole250 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption125.Id
        };
        productAttributeOptionRole250.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole250.SetMainMaxOrderQty(10);
        productAttributeOptionRole250.SetMainMinOrderQty(2);
        var productAttributeOptionRole251 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption126.Id
        };
        productAttributeOptionRole251.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole251.SetMainMaxOrderQty(10);
        productAttributeOptionRole251.SetMainMinOrderQty(2);
        var productAttributeOptionRole252 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption126.Id
        };
        productAttributeOptionRole252.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole252.SetMainMaxOrderQty(10);
        productAttributeOptionRole252.SetMainMinOrderQty(2);
        var productAttributeOptionRole253 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption127.Id
        };
        productAttributeOptionRole253.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole253.SetMainMaxOrderQty(10);
        productAttributeOptionRole253.SetMainMinOrderQty(2);
        var productAttributeOptionRole254 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption127.Id
        };
        productAttributeOptionRole254.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole254.SetMainMaxOrderQty(10);
        productAttributeOptionRole254.SetMainMinOrderQty(2);
        var productAttributeOptionRole255 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption128.Id
        };
        productAttributeOptionRole255.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole255.SetMainMaxOrderQty(10);
        productAttributeOptionRole255.SetMainMinOrderQty(2);
        var productAttributeOptionRole256 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption128.Id
        };
        productAttributeOptionRole256.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole256.SetMainMaxOrderQty(10);
        productAttributeOptionRole256.SetMainMinOrderQty(2);
        var productAttributeOptionRole257 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption129.Id
        };
        productAttributeOptionRole257.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole257.SetMainMaxOrderQty(10);
        productAttributeOptionRole257.SetMainMinOrderQty(2);
        var productAttributeOptionRole258 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption129.Id
        };
        productAttributeOptionRole258.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole258.SetMainMaxOrderQty(10);
        productAttributeOptionRole258.SetMainMinOrderQty(2);
        var productAttributeOptionRole259 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption130.Id
        };
        productAttributeOptionRole259.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole259.SetMainMaxOrderQty(10);
        productAttributeOptionRole259.SetMainMinOrderQty(2);
        var productAttributeOptionRole260 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption130.Id
        };
        productAttributeOptionRole260.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole260.SetMainMaxOrderQty(10);
        productAttributeOptionRole260.SetMainMinOrderQty(2);
        var productAttributeOptionRole261 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption131.Id
        };
        productAttributeOptionRole261.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole261.SetMainMaxOrderQty(10);
        productAttributeOptionRole261.SetMainMinOrderQty(2);
        var productAttributeOptionRole262 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption131.Id
        };
        productAttributeOptionRole262.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole262.SetMainMaxOrderQty(10);
        productAttributeOptionRole262.SetMainMinOrderQty(2);
        var productAttributeOptionRole263 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption132.Id
        };
        productAttributeOptionRole263.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole263.SetMainMaxOrderQty(10);
        productAttributeOptionRole263.SetMainMinOrderQty(2);
        var productAttributeOptionRole264 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption132.Id
        };
        productAttributeOptionRole264.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole264.SetMainMaxOrderQty(10);
        productAttributeOptionRole264.SetMainMinOrderQty(2);
        var productAttributeOptionRole265 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption133.Id
        };
        productAttributeOptionRole265.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole265.SetMainMaxOrderQty(10);
        productAttributeOptionRole265.SetMainMinOrderQty(2);
        var productAttributeOptionRole266 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption133.Id
        };
        productAttributeOptionRole266.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole266.SetMainMaxOrderQty(10);
        productAttributeOptionRole266.SetMainMinOrderQty(2);
        var productAttributeOptionRole267 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption134.Id
        };
        productAttributeOptionRole267.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole267.SetMainMaxOrderQty(10);
        productAttributeOptionRole267.SetMainMinOrderQty(2);
        var productAttributeOptionRole268 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption134.Id
        };
        productAttributeOptionRole268.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole268.SetMainMaxOrderQty(10);
        productAttributeOptionRole268.SetMainMinOrderQty(2);
        var productAttributeOptionRole269 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption135.Id
        };
        productAttributeOptionRole269.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole269.SetMainMaxOrderQty(10);
        productAttributeOptionRole269.SetMainMinOrderQty(2);
        var productAttributeOptionRole270 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption135.Id
        };
        productAttributeOptionRole270.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole270.SetMainMaxOrderQty(10);
        productAttributeOptionRole270.SetMainMinOrderQty(2);
        var productAttributeOptionRole271 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption136.Id
        };
        productAttributeOptionRole271.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole271.SetMainMaxOrderQty(10);
        productAttributeOptionRole271.SetMainMinOrderQty(2);
        var productAttributeOptionRole272 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption136.Id
        };
        productAttributeOptionRole272.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole272.SetMainMaxOrderQty(10);
        productAttributeOptionRole272.SetMainMinOrderQty(2);
        var productAttributeOptionRole273 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption137.Id
        };
        productAttributeOptionRole273.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole273.SetMainMaxOrderQty(10);
        productAttributeOptionRole273.SetMainMinOrderQty(2);
        var productAttributeOptionRole274 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption137.Id
        };
        productAttributeOptionRole274.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole274.SetMainMaxOrderQty(10);
        productAttributeOptionRole274.SetMainMinOrderQty(2);
        var productAttributeOptionRole275 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption138.Id
        };
        productAttributeOptionRole275.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole275.SetMainMaxOrderQty(10);
        productAttributeOptionRole275.SetMainMinOrderQty(2);
        var productAttributeOptionRole276 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption138.Id
        };
        productAttributeOptionRole276.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole276.SetMainMaxOrderQty(10);
        productAttributeOptionRole276.SetMainMinOrderQty(2);
        var productAttributeOptionRole277 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption139.Id
        };
        productAttributeOptionRole277.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole277.SetMainMaxOrderQty(10);
        productAttributeOptionRole277.SetMainMinOrderQty(2);
        var productAttributeOptionRole278 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption139.Id
        };
        productAttributeOptionRole278.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole278.SetMainMaxOrderQty(10);
        productAttributeOptionRole278.SetMainMinOrderQty(2);
        var productAttributeOptionRole279 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption140.Id
        };
        productAttributeOptionRole279.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole279.SetMainMaxOrderQty(10);
        productAttributeOptionRole279.SetMainMinOrderQty(2);
        var productAttributeOptionRole280 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption140.Id
        };
        productAttributeOptionRole280.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole280.SetMainMaxOrderQty(10);
        productAttributeOptionRole280.SetMainMinOrderQty(2);
        var productAttributeOptionRole281 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption141.Id
        };
        productAttributeOptionRole281.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole281.SetMainMaxOrderQty(10);
        productAttributeOptionRole281.SetMainMinOrderQty(2);
        var productAttributeOptionRole282 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption141.Id
        };
        productAttributeOptionRole282.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole282.SetMainMaxOrderQty(10);
        productAttributeOptionRole282.SetMainMinOrderQty(2);
        var productAttributeOptionRole283 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption142.Id
        };
        productAttributeOptionRole283.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole283.SetMainMaxOrderQty(10);
        productAttributeOptionRole283.SetMainMinOrderQty(2);
        var productAttributeOptionRole284 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption142.Id
        };
        productAttributeOptionRole284.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole284.SetMainMaxOrderQty(10);
        productAttributeOptionRole284.SetMainMinOrderQty(2);
        var productAttributeOptionRole285 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption143.Id
        };
        productAttributeOptionRole285.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole285.SetMainMaxOrderQty(10);
        productAttributeOptionRole285.SetMainMinOrderQty(2);
        var productAttributeOptionRole286 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption143.Id
        };
        productAttributeOptionRole286.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole286.SetMainMaxOrderQty(10);
        productAttributeOptionRole286.SetMainMinOrderQty(2);
        var productAttributeOptionRole287 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption144.Id
        };
        productAttributeOptionRole287.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole287.SetMainMaxOrderQty(10);
        productAttributeOptionRole287.SetMainMinOrderQty(2);
        var productAttributeOptionRole288 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption144.Id
        };
        productAttributeOptionRole288.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole288.SetMainMaxOrderQty(10);
        productAttributeOptionRole288.SetMainMinOrderQty(2);
        var productAttributeOptionRole289 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption145.Id
        };
        productAttributeOptionRole289.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole289.SetMainMaxOrderQty(10);
        productAttributeOptionRole289.SetMainMinOrderQty(2);
        var productAttributeOptionRole290 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption145.Id
        };
        productAttributeOptionRole290.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole290.SetMainMaxOrderQty(10);
        productAttributeOptionRole290.SetMainMinOrderQty(2);
        var productAttributeOptionRole291 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption146.Id
        };
        productAttributeOptionRole291.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole291.SetMainMaxOrderQty(10);
        productAttributeOptionRole291.SetMainMinOrderQty(2);
        var productAttributeOptionRole292 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption146.Id
        };
        productAttributeOptionRole292.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole292.SetMainMaxOrderQty(10);
        productAttributeOptionRole292.SetMainMinOrderQty(2);
        var productAttributeOptionRole293 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption147.Id
        };
        productAttributeOptionRole293.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole293.SetMainMaxOrderQty(10);
        productAttributeOptionRole293.SetMainMinOrderQty(2);
        var productAttributeOptionRole294 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption147.Id
        };
        productAttributeOptionRole294.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole294.SetMainMaxOrderQty(10);
        productAttributeOptionRole294.SetMainMinOrderQty(2);
        var productAttributeOptionRole295 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption148.Id
        };
        productAttributeOptionRole295.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole295.SetMainMaxOrderQty(10);
        productAttributeOptionRole295.SetMainMinOrderQty(2);
        var productAttributeOptionRole296 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption148.Id
        };
        productAttributeOptionRole296.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole296.SetMainMaxOrderQty(10);
        productAttributeOptionRole296.SetMainMinOrderQty(2);
        var productAttributeOptionRole297 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption149.Id
        };
        productAttributeOptionRole297.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole297.SetMainMaxOrderQty(10);
        productAttributeOptionRole297.SetMainMinOrderQty(2);
        var productAttributeOptionRole298 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption149.Id
        };
        productAttributeOptionRole298.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole298.SetMainMaxOrderQty(10);
        productAttributeOptionRole298.SetMainMinOrderQty(2);
        var productAttributeOptionRole299 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption150.Id
        };
        productAttributeOptionRole299.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole299.SetMainMaxOrderQty(10);
        productAttributeOptionRole299.SetMainMinOrderQty(2);
        var productAttributeOptionRole300 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption150.Id
        };
        productAttributeOptionRole300.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole300.SetMainMaxOrderQty(10);
        productAttributeOptionRole300.SetMainMinOrderQty(2);
        var productAttributeOptionRole301 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption151.Id
        };
        productAttributeOptionRole301.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole301.SetMainMaxOrderQty(10);
        productAttributeOptionRole301.SetMainMinOrderQty(2);
        var productAttributeOptionRole302 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption151.Id
        };
        productAttributeOptionRole302.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole302.SetMainMaxOrderQty(10);
        productAttributeOptionRole302.SetMainMinOrderQty(2);
        var productAttributeOptionRole303 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption152.Id
        };
        productAttributeOptionRole303.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole303.SetMainMaxOrderQty(10);
        productAttributeOptionRole303.SetMainMinOrderQty(2);
        var productAttributeOptionRole304 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption152.Id
        };
        productAttributeOptionRole304.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole304.SetMainMaxOrderQty(10);
        productAttributeOptionRole304.SetMainMinOrderQty(2);
        var productAttributeOptionRole305 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption153.Id
        };
        productAttributeOptionRole305.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole305.SetMainMaxOrderQty(10);
        productAttributeOptionRole305.SetMainMinOrderQty(2);
        var productAttributeOptionRole306 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption153.Id
        };
        productAttributeOptionRole306.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole306.SetMainMaxOrderQty(10);
        productAttributeOptionRole306.SetMainMinOrderQty(2);
        var productAttributeOptionRole307 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption154.Id
        };
        productAttributeOptionRole307.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole307.SetMainMaxOrderQty(10);
        productAttributeOptionRole307.SetMainMinOrderQty(2);
        var productAttributeOptionRole308 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption154.Id
        };
        productAttributeOptionRole308.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole308.SetMainMaxOrderQty(10);
        productAttributeOptionRole308.SetMainMinOrderQty(2);
        var productAttributeOptionRole309 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption155.Id
        };
        productAttributeOptionRole309.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole309.SetMainMaxOrderQty(10);
        productAttributeOptionRole309.SetMainMinOrderQty(2);
        var productAttributeOptionRole310 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption155.Id
        };
        productAttributeOptionRole310.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole310.SetMainMaxOrderQty(10);
        productAttributeOptionRole310.SetMainMinOrderQty(2);
        var productAttributeOptionRole311 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption156.Id
        };
        productAttributeOptionRole311.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole311.SetMainMaxOrderQty(10);
        productAttributeOptionRole311.SetMainMinOrderQty(2);
        var productAttributeOptionRole312 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption156.Id
        };
        productAttributeOptionRole312.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole312.SetMainMaxOrderQty(10);
        productAttributeOptionRole312.SetMainMinOrderQty(2);
        var productAttributeOptionRole313 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption157.Id
        };
        productAttributeOptionRole313.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole313.SetMainMaxOrderQty(10);
        productAttributeOptionRole313.SetMainMinOrderQty(2);
        var productAttributeOptionRole314 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption157.Id
        };
        productAttributeOptionRole314.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole314.SetMainMaxOrderQty(10);
        productAttributeOptionRole314.SetMainMinOrderQty(2);
        var productAttributeOptionRole315 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption158.Id
        };
        productAttributeOptionRole315.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole315.SetMainMaxOrderQty(10);
        productAttributeOptionRole315.SetMainMinOrderQty(2);
        var productAttributeOptionRole316 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption158.Id
        };
        productAttributeOptionRole316.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole316.SetMainMaxOrderQty(10);
        productAttributeOptionRole316.SetMainMinOrderQty(2);
        var productAttributeOptionRole317 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption159.Id
        };
        productAttributeOptionRole317.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole317.SetMainMaxOrderQty(10);
        productAttributeOptionRole317.SetMainMinOrderQty(2);
        var productAttributeOptionRole318 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption159.Id
        };
        productAttributeOptionRole318.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole318.SetMainMaxOrderQty(10);
        productAttributeOptionRole318.SetMainMinOrderQty(2);
        var productAttributeOptionRole319 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption160.Id
        };
        productAttributeOptionRole319.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole319.SetMainMaxOrderQty(10);
        productAttributeOptionRole319.SetMainMinOrderQty(2);
        var productAttributeOptionRole320 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption160.Id
        };
        productAttributeOptionRole320.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole320.SetMainMaxOrderQty(10);
        productAttributeOptionRole320.SetMainMinOrderQty(2);
        var productAttributeOptionRole321 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption161.Id
        };
        productAttributeOptionRole321.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole321.SetMainMaxOrderQty(10);
        productAttributeOptionRole321.SetMainMinOrderQty(2);
        var productAttributeOptionRole322 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption161.Id
        };
        productAttributeOptionRole322.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole322.SetMainMaxOrderQty(10);
        productAttributeOptionRole322.SetMainMinOrderQty(2);
        var productAttributeOptionRole323 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption162.Id
        };
        productAttributeOptionRole323.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole323.SetMainMaxOrderQty(10);
        productAttributeOptionRole323.SetMainMinOrderQty(2);
        var productAttributeOptionRole324 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption162.Id
        };
        productAttributeOptionRole324.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole324.SetMainMaxOrderQty(10);
        productAttributeOptionRole324.SetMainMinOrderQty(2);
        var productAttributeOptionRole325 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption163.Id
        };
        productAttributeOptionRole325.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole325.SetMainMaxOrderQty(10);
        productAttributeOptionRole325.SetMainMinOrderQty(2);
        var productAttributeOptionRole326 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption163.Id
        };
        productAttributeOptionRole326.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole326.SetMainMaxOrderQty(10);
        productAttributeOptionRole326.SetMainMinOrderQty(2);
        var productAttributeOptionRole327 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption164.Id
        };
        productAttributeOptionRole327.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole327.SetMainMaxOrderQty(10);
        productAttributeOptionRole327.SetMainMinOrderQty(2);
        var productAttributeOptionRole328 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption164.Id
        };
        productAttributeOptionRole328.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole328.SetMainMaxOrderQty(10);
        productAttributeOptionRole328.SetMainMinOrderQty(2);
        var productAttributeOptionRole329 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption165.Id
        };
        productAttributeOptionRole329.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole329.SetMainMaxOrderQty(10);
        productAttributeOptionRole329.SetMainMinOrderQty(2);
        var productAttributeOptionRole330 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption165.Id
        };
        productAttributeOptionRole330.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole330.SetMainMaxOrderQty(10);
        productAttributeOptionRole330.SetMainMinOrderQty(2);
        var productAttributeOptionRole331 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption166.Id
        };
        productAttributeOptionRole331.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole331.SetMainMaxOrderQty(10);
        productAttributeOptionRole331.SetMainMinOrderQty(2);
        var productAttributeOptionRole332 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption166.Id
        };
        productAttributeOptionRole332.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole332.SetMainMaxOrderQty(10);
        productAttributeOptionRole332.SetMainMinOrderQty(2);
        var productAttributeOptionRole333 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption167.Id
        };
        productAttributeOptionRole333.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole333.SetMainMaxOrderQty(10);
        productAttributeOptionRole333.SetMainMinOrderQty(2);
        var productAttributeOptionRole334 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption167.Id
        };
        productAttributeOptionRole334.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole334.SetMainMaxOrderQty(10);
        productAttributeOptionRole334.SetMainMinOrderQty(2);
        var productAttributeOptionRole335 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption168.Id
        };
        productAttributeOptionRole335.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole335.SetMainMaxOrderQty(10);
        productAttributeOptionRole335.SetMainMinOrderQty(2);
        var productAttributeOptionRole336 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption168.Id
        };
        productAttributeOptionRole336.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole336.SetMainMaxOrderQty(10);
        productAttributeOptionRole336.SetMainMinOrderQty(2);
        var productAttributeOptionRole337 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption169.Id
        };
        productAttributeOptionRole337.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole337.SetMainMaxOrderQty(10);
        productAttributeOptionRole337.SetMainMinOrderQty(2);
        var productAttributeOptionRole338 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption169.Id
        };
        productAttributeOptionRole338.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole338.SetMainMaxOrderQty(10);
        productAttributeOptionRole338.SetMainMinOrderQty(2);
        var productAttributeOptionRole339 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption170.Id
        };
        productAttributeOptionRole339.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole339.SetMainMaxOrderQty(10);
        productAttributeOptionRole339.SetMainMinOrderQty(2);
        var productAttributeOptionRole340 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption170.Id
        };
        productAttributeOptionRole340.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole340.SetMainMaxOrderQty(10);
        productAttributeOptionRole340.SetMainMinOrderQty(2);
        var productAttributeOptionRole341 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption171.Id
        };
        productAttributeOptionRole341.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole341.SetMainMaxOrderQty(10);
        productAttributeOptionRole341.SetMainMinOrderQty(2);
        var productAttributeOptionRole342 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption171.Id
        };
        productAttributeOptionRole342.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole342.SetMainMaxOrderQty(10);
        productAttributeOptionRole342.SetMainMinOrderQty(2);
        var productAttributeOptionRole343 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption172.Id
        };
        productAttributeOptionRole343.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole343.SetMainMaxOrderQty(10);
        productAttributeOptionRole343.SetMainMinOrderQty(2);
        var productAttributeOptionRole344 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption172.Id
        };
        productAttributeOptionRole344.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole344.SetMainMaxOrderQty(10);
        productAttributeOptionRole344.SetMainMinOrderQty(2);
        var productAttributeOptionRole345 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption173.Id
        };
        productAttributeOptionRole345.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole345.SetMainMaxOrderQty(10);
        productAttributeOptionRole345.SetMainMinOrderQty(2);
        var productAttributeOptionRole346 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption173.Id
        };
        productAttributeOptionRole346.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole346.SetMainMaxOrderQty(10);
        productAttributeOptionRole346.SetMainMinOrderQty(2);
        var productAttributeOptionRole347 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption174.Id
        };
        productAttributeOptionRole347.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole347.SetMainMaxOrderQty(10);
        productAttributeOptionRole347.SetMainMinOrderQty(2);
        var productAttributeOptionRole348 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption174.Id
        };
        productAttributeOptionRole348.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole348.SetMainMaxOrderQty(10);
        productAttributeOptionRole348.SetMainMinOrderQty(2);
        var productAttributeOptionRole349 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption175.Id
        };
        productAttributeOptionRole349.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole349.SetMainMaxOrderQty(10);
        productAttributeOptionRole349.SetMainMinOrderQty(2);
        var productAttributeOptionRole350 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption175.Id
        };
        productAttributeOptionRole350.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole350.SetMainMaxOrderQty(10);
        productAttributeOptionRole350.SetMainMinOrderQty(2);
        var productAttributeOptionRole351 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption176.Id
        };
        productAttributeOptionRole351.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole351.SetMainMaxOrderQty(10);
        productAttributeOptionRole351.SetMainMinOrderQty(2);
        var productAttributeOptionRole352 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption176.Id
        };
        productAttributeOptionRole352.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole352.SetMainMaxOrderQty(10);
        productAttributeOptionRole352.SetMainMinOrderQty(2);
        var productAttributeOptionRole353 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption177.Id
        };
        productAttributeOptionRole353.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole353.SetMainMaxOrderQty(10);
        productAttributeOptionRole353.SetMainMinOrderQty(2);
        var productAttributeOptionRole354 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption177.Id
        };
        productAttributeOptionRole354.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole354.SetMainMaxOrderQty(10);
        productAttributeOptionRole354.SetMainMinOrderQty(2);
        var productAttributeOptionRole355 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption178.Id
        };
        productAttributeOptionRole355.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole355.SetMainMaxOrderQty(10);
        productAttributeOptionRole355.SetMainMinOrderQty(2);
        var productAttributeOptionRole356 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption178.Id
        };
        productAttributeOptionRole356.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole356.SetMainMaxOrderQty(10);
        productAttributeOptionRole356.SetMainMinOrderQty(2);
        var productAttributeOptionRole357 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption179.Id
        };
        productAttributeOptionRole357.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole357.SetMainMaxOrderQty(10);
        productAttributeOptionRole357.SetMainMinOrderQty(2);
        var productAttributeOptionRole358 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption179.Id
        };
        productAttributeOptionRole358.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole358.SetMainMaxOrderQty(10);
        productAttributeOptionRole358.SetMainMinOrderQty(2);
        var productAttributeOptionRole359 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption180.Id
        };
        productAttributeOptionRole359.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole359.SetMainMaxOrderQty(10);
        productAttributeOptionRole359.SetMainMinOrderQty(2);
        var productAttributeOptionRole360 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption180.Id
        };
        productAttributeOptionRole360.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole360.SetMainMaxOrderQty(10);
        productAttributeOptionRole360.SetMainMinOrderQty(2);
        var productAttributeOptionRole361 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption181.Id
        };
        productAttributeOptionRole361.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole361.SetMainMaxOrderQty(10);
        productAttributeOptionRole361.SetMainMinOrderQty(2);
        var productAttributeOptionRole362 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption181.Id
        };
        productAttributeOptionRole362.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole362.SetMainMaxOrderQty(10);
        productAttributeOptionRole362.SetMainMinOrderQty(2);
        var productAttributeOptionRole363 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption182.Id
        };
        productAttributeOptionRole363.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole363.SetMainMaxOrderQty(10);
        productAttributeOptionRole363.SetMainMinOrderQty(2);
        var productAttributeOptionRole364 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption182.Id
        };
        productAttributeOptionRole364.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole364.SetMainMaxOrderQty(10);
        productAttributeOptionRole364.SetMainMinOrderQty(2);
        var productAttributeOptionRole365 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption183.Id
        };
        productAttributeOptionRole365.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole365.SetMainMaxOrderQty(10);
        productAttributeOptionRole365.SetMainMinOrderQty(2);
        var productAttributeOptionRole366 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption183.Id
        };
        productAttributeOptionRole366.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole366.SetMainMaxOrderQty(10);
        productAttributeOptionRole366.SetMainMinOrderQty(2);
        var productAttributeOptionRole367 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption184.Id
        };
        productAttributeOptionRole367.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole367.SetMainMaxOrderQty(10);
        productAttributeOptionRole367.SetMainMinOrderQty(2);
        var productAttributeOptionRole368 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption184.Id
        };
        productAttributeOptionRole368.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole368.SetMainMaxOrderQty(10);
        productAttributeOptionRole368.SetMainMinOrderQty(2);
        var productAttributeOptionRole369 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption185.Id
        };
        productAttributeOptionRole369.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole369.SetMainMaxOrderQty(10);
        productAttributeOptionRole369.SetMainMinOrderQty(2);
        var productAttributeOptionRole370 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption185.Id
        };
        productAttributeOptionRole370.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole370.SetMainMaxOrderQty(10);
        productAttributeOptionRole370.SetMainMinOrderQty(2);
        var productAttributeOptionRole371 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption186.Id
        };
        productAttributeOptionRole371.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole371.SetMainMaxOrderQty(10);
        productAttributeOptionRole371.SetMainMinOrderQty(2);
        var productAttributeOptionRole372 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption186.Id
        };
        productAttributeOptionRole372.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole372.SetMainMaxOrderQty(10);
        productAttributeOptionRole372.SetMainMinOrderQty(2);
        var productAttributeOptionRole373 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption187.Id
        };
        productAttributeOptionRole373.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole373.SetMainMaxOrderQty(10);
        productAttributeOptionRole373.SetMainMinOrderQty(2);
        var productAttributeOptionRole374 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption187.Id
        };
        productAttributeOptionRole374.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole374.SetMainMaxOrderQty(10);
        productAttributeOptionRole374.SetMainMinOrderQty(2);
        var productAttributeOptionRole375 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption188.Id
        };
        productAttributeOptionRole375.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole375.SetMainMaxOrderQty(10);
        productAttributeOptionRole375.SetMainMinOrderQty(2);
        var productAttributeOptionRole376 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption188.Id
        };
        productAttributeOptionRole376.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole376.SetMainMaxOrderQty(10);
        productAttributeOptionRole376.SetMainMinOrderQty(2);
        var productAttributeOptionRole377 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption189.Id
        };
        productAttributeOptionRole377.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole377.SetMainMaxOrderQty(10);
        productAttributeOptionRole377.SetMainMinOrderQty(2);
        var productAttributeOptionRole378 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption189.Id
        };
        productAttributeOptionRole378.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole378.SetMainMaxOrderQty(10);
        productAttributeOptionRole378.SetMainMinOrderQty(2);
        var productAttributeOptionRole379 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption190.Id
        };
        productAttributeOptionRole379.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole379.SetMainMaxOrderQty(10);
        productAttributeOptionRole379.SetMainMinOrderQty(2);
        var productAttributeOptionRole380 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption190.Id
        };
        productAttributeOptionRole380.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole380.SetMainMaxOrderQty(10);
        productAttributeOptionRole380.SetMainMinOrderQty(2);
        var productAttributeOptionRole381 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption191.Id
        };
        productAttributeOptionRole381.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole381.SetMainMaxOrderQty(10);
        productAttributeOptionRole381.SetMainMinOrderQty(2);
        var productAttributeOptionRole382 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption191.Id
        };
        productAttributeOptionRole382.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole382.SetMainMaxOrderQty(10);
        productAttributeOptionRole382.SetMainMinOrderQty(2);
        var productAttributeOptionRole383 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption192.Id
        };
        productAttributeOptionRole383.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole383.SetMainMaxOrderQty(10);
        productAttributeOptionRole383.SetMainMinOrderQty(2);
        var productAttributeOptionRole384 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption192.Id
        };
        productAttributeOptionRole384.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole384.SetMainMaxOrderQty(10);
        productAttributeOptionRole384.SetMainMinOrderQty(2);
        var productAttributeOptionRole385 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption193.Id
        };
        productAttributeOptionRole385.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole385.SetMainMaxOrderQty(10);
        productAttributeOptionRole385.SetMainMinOrderQty(2);
        var productAttributeOptionRole386 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption193.Id
        };
        productAttributeOptionRole386.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole386.SetMainMaxOrderQty(10);
        productAttributeOptionRole386.SetMainMinOrderQty(2);
        var productAttributeOptionRole387 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption194.Id
        };
        productAttributeOptionRole387.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole387.SetMainMaxOrderQty(10);
        productAttributeOptionRole387.SetMainMinOrderQty(2);
        var productAttributeOptionRole388 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption194.Id
        };
        productAttributeOptionRole388.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole388.SetMainMaxOrderQty(10);
        productAttributeOptionRole388.SetMainMinOrderQty(2);
        var productAttributeOptionRole389 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption195.Id
        };
        productAttributeOptionRole389.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole389.SetMainMaxOrderQty(10);
        productAttributeOptionRole389.SetMainMinOrderQty(2);
        var productAttributeOptionRole390 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption195.Id
        };
        productAttributeOptionRole390.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole390.SetMainMaxOrderQty(10);
        productAttributeOptionRole390.SetMainMinOrderQty(2);
        var productAttributeOptionRole391 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption196.Id
        };
        productAttributeOptionRole391.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole391.SetMainMaxOrderQty(10);
        productAttributeOptionRole391.SetMainMinOrderQty(2);
        var productAttributeOptionRole392 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption196.Id
        };
        productAttributeOptionRole392.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole392.SetMainMaxOrderQty(10);
        productAttributeOptionRole392.SetMainMinOrderQty(2);
        var productAttributeOptionRole393 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption197.Id
        };
        productAttributeOptionRole393.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole393.SetMainMaxOrderQty(10);
        productAttributeOptionRole393.SetMainMinOrderQty(2);
        var productAttributeOptionRole394 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption197.Id
        };
        productAttributeOptionRole394.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole394.SetMainMaxOrderQty(10);
        productAttributeOptionRole394.SetMainMinOrderQty(2);
        var productAttributeOptionRole395 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption198.Id
        };
        productAttributeOptionRole395.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole395.SetMainMaxOrderQty(10);
        productAttributeOptionRole395.SetMainMinOrderQty(2);
        var productAttributeOptionRole396 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption198.Id
        };
        productAttributeOptionRole396.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole396.SetMainMaxOrderQty(10);
        productAttributeOptionRole396.SetMainMinOrderQty(2);
        var productAttributeOptionRole397 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption199.Id
        };
        productAttributeOptionRole397.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole397.SetMainMaxOrderQty(10);
        productAttributeOptionRole397.SetMainMinOrderQty(2);
        var productAttributeOptionRole398 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption199.Id
        };
        productAttributeOptionRole398.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole398.SetMainMaxOrderQty(10);
        productAttributeOptionRole398.SetMainMinOrderQty(2);
        var productAttributeOptionRole399 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption200.Id
        };
        productAttributeOptionRole399.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole399.SetMainMaxOrderQty(10);
        productAttributeOptionRole399.SetMainMinOrderQty(2);
        var productAttributeOptionRole400 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption200.Id
        };
        productAttributeOptionRole400.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole400.SetMainMaxOrderQty(10);
        productAttributeOptionRole400.SetMainMinOrderQty(2);
        var productAttributeOptionRole401 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption201.Id
        };
        productAttributeOptionRole401.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole401.SetMainMaxOrderQty(10);
        productAttributeOptionRole401.SetMainMinOrderQty(2);
        var productAttributeOptionRole402 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption201.Id
        };
        productAttributeOptionRole402.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole402.SetMainMaxOrderQty(10);
        productAttributeOptionRole402.SetMainMinOrderQty(2);
        var productAttributeOptionRole403 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption202.Id
        };
        productAttributeOptionRole403.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole403.SetMainMaxOrderQty(10);
        productAttributeOptionRole403.SetMainMinOrderQty(2);
        var productAttributeOptionRole404 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption202.Id
        };
        productAttributeOptionRole404.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole404.SetMainMaxOrderQty(10);
        productAttributeOptionRole404.SetMainMinOrderQty(2);
        var productAttributeOptionRole405 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption203.Id
        };
        productAttributeOptionRole405.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole405.SetMainMaxOrderQty(10);
        productAttributeOptionRole405.SetMainMinOrderQty(2);
        var productAttributeOptionRole406 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption203.Id
        };
        productAttributeOptionRole406.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole406.SetMainMaxOrderQty(10);
        productAttributeOptionRole406.SetMainMinOrderQty(2);
        var productAttributeOptionRole407 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption204.Id
        };
        productAttributeOptionRole407.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole407.SetMainMaxOrderQty(10);
        productAttributeOptionRole407.SetMainMinOrderQty(2);
        var productAttributeOptionRole408 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption204.Id
        };
        productAttributeOptionRole408.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole408.SetMainMaxOrderQty(10);
        productAttributeOptionRole408.SetMainMinOrderQty(2);
        var productAttributeOptionRole409 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption205.Id
        };
        productAttributeOptionRole409.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole409.SetMainMaxOrderQty(10);
        productAttributeOptionRole409.SetMainMinOrderQty(2);
        var productAttributeOptionRole410 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption205.Id
        };
        productAttributeOptionRole410.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole410.SetMainMaxOrderQty(10);
        productAttributeOptionRole410.SetMainMinOrderQty(2);
        var productAttributeOptionRole411 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption206.Id
        };
        productAttributeOptionRole411.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole411.SetMainMaxOrderQty(10);
        productAttributeOptionRole411.SetMainMinOrderQty(2);
        var productAttributeOptionRole412 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption206.Id
        };
        productAttributeOptionRole412.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole412.SetMainMaxOrderQty(10);
        productAttributeOptionRole412.SetMainMinOrderQty(2);
        var productAttributeOptionRole413 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption207.Id
        };
        productAttributeOptionRole413.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole413.SetMainMaxOrderQty(10);
        productAttributeOptionRole413.SetMainMinOrderQty(2);
        var productAttributeOptionRole414 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption207.Id
        };
        productAttributeOptionRole414.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole414.SetMainMaxOrderQty(10);
        productAttributeOptionRole414.SetMainMinOrderQty(2);
        var productAttributeOptionRole415 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption208.Id
        };
        productAttributeOptionRole415.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole415.SetMainMaxOrderQty(10);
        productAttributeOptionRole415.SetMainMinOrderQty(2);
        var productAttributeOptionRole416 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption208.Id
        };
        productAttributeOptionRole416.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole416.SetMainMaxOrderQty(10);
        productAttributeOptionRole416.SetMainMinOrderQty(2);
        var productAttributeOptionRole417 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption209.Id
        };
        productAttributeOptionRole417.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole417.SetMainMaxOrderQty(10);
        productAttributeOptionRole417.SetMainMinOrderQty(2);
        var productAttributeOptionRole418 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption209.Id
        };
        productAttributeOptionRole418.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole418.SetMainMaxOrderQty(10);
        productAttributeOptionRole418.SetMainMinOrderQty(2);
        var productAttributeOptionRole419 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption210.Id
        };
        productAttributeOptionRole419.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole419.SetMainMaxOrderQty(10);
        productAttributeOptionRole419.SetMainMinOrderQty(2);
        var productAttributeOptionRole420 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption210.Id
        };
        productAttributeOptionRole420.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole420.SetMainMaxOrderQty(10);
        productAttributeOptionRole420.SetMainMinOrderQty(2);
        var productAttributeOptionRole421 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption211.Id
        };
        productAttributeOptionRole421.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole421.SetMainMaxOrderQty(10);
        productAttributeOptionRole421.SetMainMinOrderQty(2);
        var productAttributeOptionRole422 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption211.Id
        };
        productAttributeOptionRole422.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole422.SetMainMaxOrderQty(10);
        productAttributeOptionRole422.SetMainMinOrderQty(2);
        var productAttributeOptionRole423 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption212.Id
        };
        productAttributeOptionRole423.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole423.SetMainMaxOrderQty(10);
        productAttributeOptionRole423.SetMainMinOrderQty(2);
        var productAttributeOptionRole424 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption212.Id
        };
        productAttributeOptionRole424.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole424.SetMainMaxOrderQty(10);
        productAttributeOptionRole424.SetMainMinOrderQty(2);
        var productAttributeOptionRole425 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption213.Id
        };
        productAttributeOptionRole425.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole425.SetMainMaxOrderQty(10);
        productAttributeOptionRole425.SetMainMinOrderQty(2);
        var productAttributeOptionRole426 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption213.Id
        };
        productAttributeOptionRole426.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole426.SetMainMaxOrderQty(10);
        productAttributeOptionRole426.SetMainMinOrderQty(2);
        var productAttributeOptionRole427 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption214.Id
        };
        productAttributeOptionRole427.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole427.SetMainMaxOrderQty(10);
        productAttributeOptionRole427.SetMainMinOrderQty(2);
        var productAttributeOptionRole428 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption214.Id
        };
        productAttributeOptionRole428.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole428.SetMainMaxOrderQty(10);
        productAttributeOptionRole428.SetMainMinOrderQty(2);
        var productAttributeOptionRole429 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption215.Id
        };
        productAttributeOptionRole429.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole429.SetMainMaxOrderQty(10);
        productAttributeOptionRole429.SetMainMinOrderQty(2);
        var productAttributeOptionRole430 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption215.Id
        };
        productAttributeOptionRole430.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole430.SetMainMaxOrderQty(10);
        productAttributeOptionRole430.SetMainMinOrderQty(2);
        var productAttributeOptionRole431 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption216.Id
        };
        productAttributeOptionRole431.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole431.SetMainMaxOrderQty(10);
        productAttributeOptionRole431.SetMainMinOrderQty(2);
        var productAttributeOptionRole432 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption216.Id
        };
        productAttributeOptionRole432.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole432.SetMainMaxOrderQty(10);
        productAttributeOptionRole432.SetMainMinOrderQty(2);
        var productAttributeOptionRole433 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption217.Id
        };
        productAttributeOptionRole433.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole433.SetMainMaxOrderQty(10);
        productAttributeOptionRole433.SetMainMinOrderQty(2);
        var productAttributeOptionRole434 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption217.Id
        };
        productAttributeOptionRole434.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole434.SetMainMaxOrderQty(10);
        productAttributeOptionRole434.SetMainMinOrderQty(2);
        var productAttributeOptionRole435 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption218.Id
        };
        productAttributeOptionRole435.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole435.SetMainMaxOrderQty(10);
        productAttributeOptionRole435.SetMainMinOrderQty(2);
        var productAttributeOptionRole436 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption218.Id
        };
        productAttributeOptionRole436.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole436.SetMainMaxOrderQty(10);
        productAttributeOptionRole436.SetMainMinOrderQty(2);
        var productAttributeOptionRole437 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption219.Id
        };
        productAttributeOptionRole437.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole437.SetMainMaxOrderQty(10);
        productAttributeOptionRole437.SetMainMinOrderQty(2);
        var productAttributeOptionRole438 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption219.Id
        };
        productAttributeOptionRole438.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole438.SetMainMaxOrderQty(10);
        productAttributeOptionRole438.SetMainMinOrderQty(2);
        var productAttributeOptionRole439 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption220.Id
        };
        productAttributeOptionRole439.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole439.SetMainMaxOrderQty(10);
        productAttributeOptionRole439.SetMainMinOrderQty(2);
        var productAttributeOptionRole440 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption220.Id
        };
        productAttributeOptionRole440.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole440.SetMainMaxOrderQty(10);
        productAttributeOptionRole440.SetMainMinOrderQty(2);
        var productAttributeOptionRole441 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption221.Id
        };
        productAttributeOptionRole441.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole441.SetMainMaxOrderQty(10);
        productAttributeOptionRole441.SetMainMinOrderQty(2);
        var productAttributeOptionRole442 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption221.Id
        };
        productAttributeOptionRole442.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole442.SetMainMaxOrderQty(10);
        productAttributeOptionRole442.SetMainMinOrderQty(2);
        var productAttributeOptionRole443 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption222.Id
        };
        productAttributeOptionRole443.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole443.SetMainMaxOrderQty(10);
        productAttributeOptionRole443.SetMainMinOrderQty(2);
        var productAttributeOptionRole444 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption222.Id
        };
        productAttributeOptionRole444.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole444.SetMainMaxOrderQty(10);
        productAttributeOptionRole444.SetMainMinOrderQty(2);
        var productAttributeOptionRole445 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption223.Id
        };
        productAttributeOptionRole445.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole445.SetMainMaxOrderQty(10);
        productAttributeOptionRole445.SetMainMinOrderQty(2);
        var productAttributeOptionRole446 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption223.Id
        };
        productAttributeOptionRole446.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole446.SetMainMaxOrderQty(10);
        productAttributeOptionRole446.SetMainMinOrderQty(2);
        var productAttributeOptionRole447 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption224.Id
        };
        productAttributeOptionRole447.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole447.SetMainMaxOrderQty(10);
        productAttributeOptionRole447.SetMainMinOrderQty(2);
        var productAttributeOptionRole448 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption224.Id
        };
        productAttributeOptionRole448.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole448.SetMainMaxOrderQty(10);
        productAttributeOptionRole448.SetMainMinOrderQty(2);
        var productAttributeOptionRole449 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption225.Id
        };
        productAttributeOptionRole449.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole449.SetMainMaxOrderQty(10);
        productAttributeOptionRole449.SetMainMinOrderQty(2);
        var productAttributeOptionRole450 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption225.Id
        };
        productAttributeOptionRole450.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole450.SetMainMaxOrderQty(10);
        productAttributeOptionRole450.SetMainMinOrderQty(2);
        var productAttributeOptionRole451 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption226.Id
        };
        productAttributeOptionRole451.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole451.SetMainMaxOrderQty(10);
        productAttributeOptionRole451.SetMainMinOrderQty(2);
        var productAttributeOptionRole452 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption226.Id
        };
        productAttributeOptionRole452.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole452.SetMainMaxOrderQty(10);
        productAttributeOptionRole452.SetMainMinOrderQty(2);
        var productAttributeOptionRole453 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption227.Id
        };
        productAttributeOptionRole453.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole453.SetMainMaxOrderQty(10);
        productAttributeOptionRole453.SetMainMinOrderQty(2);
        var productAttributeOptionRole454 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption227.Id
        };
        productAttributeOptionRole454.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole454.SetMainMaxOrderQty(10);
        productAttributeOptionRole454.SetMainMinOrderQty(2);
        var productAttributeOptionRole455 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption228.Id
        };
        productAttributeOptionRole455.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole455.SetMainMaxOrderQty(10);
        productAttributeOptionRole455.SetMainMinOrderQty(2);
        var productAttributeOptionRole456 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption228.Id
        };
        productAttributeOptionRole456.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole456.SetMainMaxOrderQty(10);
        productAttributeOptionRole456.SetMainMinOrderQty(2);
        var productAttributeOptionRole457 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption229.Id
        };
        productAttributeOptionRole457.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole457.SetMainMaxOrderQty(10);
        productAttributeOptionRole457.SetMainMinOrderQty(2);
        var productAttributeOptionRole458 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption229.Id
        };
        productAttributeOptionRole458.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole458.SetMainMaxOrderQty(10);
        productAttributeOptionRole458.SetMainMinOrderQty(2);
        var productAttributeOptionRole459 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption230.Id
        };
        productAttributeOptionRole459.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole459.SetMainMaxOrderQty(10);
        productAttributeOptionRole459.SetMainMinOrderQty(2);
        var productAttributeOptionRole460 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption230.Id
        };
        productAttributeOptionRole460.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole460.SetMainMaxOrderQty(10);
        productAttributeOptionRole460.SetMainMinOrderQty(2);
        var productAttributeOptionRole461 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption231.Id
        };
        productAttributeOptionRole461.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole461.SetMainMaxOrderQty(10);
        productAttributeOptionRole461.SetMainMinOrderQty(2);
        var productAttributeOptionRole462 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption231.Id
        };
        productAttributeOptionRole462.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole462.SetMainMaxOrderQty(10);
        productAttributeOptionRole462.SetMainMinOrderQty(2);
        var productAttributeOptionRole463 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption232.Id
        };
        productAttributeOptionRole463.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole463.SetMainMaxOrderQty(10);
        productAttributeOptionRole463.SetMainMinOrderQty(2);
        var productAttributeOptionRole464 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption232.Id
        };
        productAttributeOptionRole464.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole464.SetMainMaxOrderQty(10);
        productAttributeOptionRole464.SetMainMinOrderQty(2);
        var productAttributeOptionRole465 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption233.Id
        };
        productAttributeOptionRole465.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole465.SetMainMaxOrderQty(10);
        productAttributeOptionRole465.SetMainMinOrderQty(2);
        var productAttributeOptionRole466 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption233.Id
        };
        productAttributeOptionRole466.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole466.SetMainMaxOrderQty(10);
        productAttributeOptionRole466.SetMainMinOrderQty(2);
        var productAttributeOptionRole467 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption234.Id
        };
        productAttributeOptionRole467.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole467.SetMainMaxOrderQty(10);
        productAttributeOptionRole467.SetMainMinOrderQty(2);
        var productAttributeOptionRole468 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption234.Id
        };
        productAttributeOptionRole468.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole468.SetMainMaxOrderQty(10);
        productAttributeOptionRole468.SetMainMinOrderQty(2);
        var productAttributeOptionRole469 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption235.Id
        };
        productAttributeOptionRole469.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole469.SetMainMaxOrderQty(10);
        productAttributeOptionRole469.SetMainMinOrderQty(2);
        var productAttributeOptionRole470 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption235.Id
        };
        productAttributeOptionRole470.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole470.SetMainMaxOrderQty(10);
        productAttributeOptionRole470.SetMainMinOrderQty(2);
        var productAttributeOptionRole471 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption236.Id
        };
        productAttributeOptionRole471.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole471.SetMainMaxOrderQty(10);
        productAttributeOptionRole471.SetMainMinOrderQty(2);
        var productAttributeOptionRole472 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption236.Id
        };
        productAttributeOptionRole472.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole472.SetMainMaxOrderQty(10);
        productAttributeOptionRole472.SetMainMinOrderQty(2);
        var productAttributeOptionRole473 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption237.Id
        };
        productAttributeOptionRole473.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole473.SetMainMaxOrderQty(10);
        productAttributeOptionRole473.SetMainMinOrderQty(2);
        var productAttributeOptionRole474 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption237.Id
        };
        productAttributeOptionRole474.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole474.SetMainMaxOrderQty(10);
        productAttributeOptionRole474.SetMainMinOrderQty(2);
        var productAttributeOptionRole475 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption238.Id
        };
        productAttributeOptionRole475.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole475.SetMainMaxOrderQty(10);
        productAttributeOptionRole475.SetMainMinOrderQty(2);
        var productAttributeOptionRole476 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption238.Id
        };
        productAttributeOptionRole476.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole476.SetMainMaxOrderQty(10);
        productAttributeOptionRole476.SetMainMinOrderQty(2);
        var productAttributeOptionRole477 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption239.Id
        };
        productAttributeOptionRole477.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole477.SetMainMaxOrderQty(10);
        productAttributeOptionRole477.SetMainMinOrderQty(2);
        var productAttributeOptionRole478 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption239.Id
        };
        productAttributeOptionRole478.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole478.SetMainMaxOrderQty(10);
        productAttributeOptionRole478.SetMainMinOrderQty(2);
        var productAttributeOptionRole479 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption240.Id
        };
        productAttributeOptionRole479.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole479.SetMainMaxOrderQty(10);
        productAttributeOptionRole479.SetMainMinOrderQty(2);
        var productAttributeOptionRole480 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption240.Id
        };
        productAttributeOptionRole480.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole480.SetMainMaxOrderQty(10);
        productAttributeOptionRole480.SetMainMinOrderQty(2);
        var productAttributeOptionRole481 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption241.Id
        };
        productAttributeOptionRole481.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole481.SetMainMaxOrderQty(10);
        productAttributeOptionRole481.SetMainMinOrderQty(2);
        var productAttributeOptionRole482 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption241.Id
        };
        productAttributeOptionRole482.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole482.SetMainMaxOrderQty(10);
        productAttributeOptionRole482.SetMainMinOrderQty(2);
        var productAttributeOptionRole483 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption242.Id
        };
        productAttributeOptionRole483.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole483.SetMainMaxOrderQty(10);
        productAttributeOptionRole483.SetMainMinOrderQty(2);
        var productAttributeOptionRole484 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption242.Id
        };
        productAttributeOptionRole484.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole484.SetMainMaxOrderQty(10);
        productAttributeOptionRole484.SetMainMinOrderQty(2);
        var productAttributeOptionRole485 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption243.Id
        };
        productAttributeOptionRole485.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole485.SetMainMaxOrderQty(10);
        productAttributeOptionRole485.SetMainMinOrderQty(2);
        var productAttributeOptionRole486 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption243.Id
        };
        productAttributeOptionRole486.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole486.SetMainMaxOrderQty(10);
        productAttributeOptionRole486.SetMainMinOrderQty(2);
        var productAttributeOptionRole487 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption244.Id
        };
        productAttributeOptionRole487.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole487.SetMainMaxOrderQty(10);
        productAttributeOptionRole487.SetMainMinOrderQty(2);
        var productAttributeOptionRole488 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption244.Id
        };
        productAttributeOptionRole488.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole488.SetMainMaxOrderQty(10);
        productAttributeOptionRole488.SetMainMinOrderQty(2);
        var productAttributeOptionRole489 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption245.Id
        };
        productAttributeOptionRole489.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole489.SetMainMaxOrderQty(10);
        productAttributeOptionRole489.SetMainMinOrderQty(2);
        var productAttributeOptionRole490 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption245.Id
        };
        productAttributeOptionRole490.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole490.SetMainMaxOrderQty(10);
        productAttributeOptionRole490.SetMainMinOrderQty(2);
        var productAttributeOptionRole491 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption246.Id
        };
        productAttributeOptionRole491.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole491.SetMainMaxOrderQty(10);
        productAttributeOptionRole491.SetMainMinOrderQty(2);
        var productAttributeOptionRole492 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption246.Id
        };
        productAttributeOptionRole492.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole492.SetMainMaxOrderQty(10);
        productAttributeOptionRole492.SetMainMinOrderQty(2);
        var productAttributeOptionRole493 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption247.Id
        };
        productAttributeOptionRole493.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole493.SetMainMaxOrderQty(10);
        productAttributeOptionRole493.SetMainMinOrderQty(2);
        var productAttributeOptionRole494 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption247.Id
        };
        productAttributeOptionRole494.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole494.SetMainMaxOrderQty(10);
        productAttributeOptionRole494.SetMainMinOrderQty(2);
        var productAttributeOptionRole495 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption248.Id
        };
        productAttributeOptionRole495.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole495.SetMainMaxOrderQty(10);
        productAttributeOptionRole495.SetMainMinOrderQty(2);
        var productAttributeOptionRole496 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption248.Id
        };
        productAttributeOptionRole496.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole496.SetMainMaxOrderQty(10);
        productAttributeOptionRole496.SetMainMinOrderQty(2);
        var productAttributeOptionRole497 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption249.Id
        };
        productAttributeOptionRole497.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole497.SetMainMaxOrderQty(10);
        productAttributeOptionRole497.SetMainMinOrderQty(2);
        var productAttributeOptionRole498 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption249.Id
        };
        productAttributeOptionRole498.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole498.SetMainMaxOrderQty(10);
        productAttributeOptionRole498.SetMainMinOrderQty(2);
        var productAttributeOptionRole499 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption250.Id
        };
        productAttributeOptionRole499.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole499.SetMainMaxOrderQty(10);
        productAttributeOptionRole499.SetMainMinOrderQty(2);
        var productAttributeOptionRole500 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption250.Id
        };
        productAttributeOptionRole500.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole500.SetMainMaxOrderQty(10);
        productAttributeOptionRole500.SetMainMinOrderQty(2);
        var productAttributeOptionRole501 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption251.Id
        };
        productAttributeOptionRole501.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole501.SetMainMaxOrderQty(10);
        productAttributeOptionRole501.SetMainMinOrderQty(2);
        var productAttributeOptionRole502 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption251.Id
        };
        productAttributeOptionRole502.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole502.SetMainMaxOrderQty(10);
        productAttributeOptionRole502.SetMainMinOrderQty(2);
        var productAttributeOptionRole503 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption252.Id
        };
        productAttributeOptionRole503.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole503.SetMainMaxOrderQty(10);
        productAttributeOptionRole503.SetMainMinOrderQty(2);
        var productAttributeOptionRole504 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption252.Id
        };
        productAttributeOptionRole504.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole504.SetMainMaxOrderQty(10);
        productAttributeOptionRole504.SetMainMinOrderQty(2);
        var productAttributeOptionRole505 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption253.Id
        };
        productAttributeOptionRole505.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole505.SetMainMaxOrderQty(10);
        productAttributeOptionRole505.SetMainMinOrderQty(2);
        var productAttributeOptionRole506 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption253.Id
        };
        productAttributeOptionRole506.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole506.SetMainMaxOrderQty(10);
        productAttributeOptionRole506.SetMainMinOrderQty(2);
        var productAttributeOptionRole507 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption254.Id
        };
        productAttributeOptionRole507.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole507.SetMainMaxOrderQty(10);
        productAttributeOptionRole507.SetMainMinOrderQty(2);
        var productAttributeOptionRole508 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption254.Id
        };
        productAttributeOptionRole508.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole508.SetMainMaxOrderQty(10);
        productAttributeOptionRole508.SetMainMinOrderQty(2);
        var productAttributeOptionRole509 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption255.Id
        };
        productAttributeOptionRole509.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole509.SetMainMaxOrderQty(10);
        productAttributeOptionRole509.SetMainMinOrderQty(2);
        var productAttributeOptionRole510 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption255.Id
        };
        productAttributeOptionRole510.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole510.SetMainMaxOrderQty(10);
        productAttributeOptionRole510.SetMainMinOrderQty(2);
        var productAttributeOptionRole511 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Personal,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption256.Id
        };
        productAttributeOptionRole511.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole511.SetMainMaxOrderQty(10);
        productAttributeOptionRole511.SetMainMinOrderQty(2);
        var productAttributeOptionRole512 = new ProductAttributeOptionRole()
        {
            CustomerTypeEnum = CustomerTypeEnum.Store,
            DiscountPercent = 0,
            ProductAttributeOptionId = productAttributeOption256.Id
        };
        productAttributeOptionRole512.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(3);
        productAttributeOptionRole512.SetMainMaxOrderQty(10);
        productAttributeOptionRole512.SetMainMinOrderQty(2);
        if (!_context.ProductAttributeOptionRoles.Any())
        {
            _context.ProductAttributeOptionRoles.AddRange(new List<ProductAttributeOptionRole>()
            {
                productAttributeOptionRole1,productAttributeOptionRole2,productAttributeOptionRole3,productAttributeOptionRole4,productAttributeOptionRole5,productAttributeOptionRole6,productAttributeOptionRole7,productAttributeOptionRole8,productAttributeOptionRole9,productAttributeOptionRole10,productAttributeOptionRole11,productAttributeOptionRole12,productAttributeOptionRole13,productAttributeOptionRole14,productAttributeOptionRole15,productAttributeOptionRole16,
                productAttributeOptionRole17,productAttributeOptionRole18,productAttributeOptionRole19,productAttributeOptionRole20,productAttributeOptionRole21,productAttributeOptionRole22,productAttributeOptionRole23,productAttributeOptionRole24,productAttributeOptionRole25,productAttributeOptionRole26,productAttributeOptionRole27,productAttributeOptionRole28,productAttributeOptionRole29,productAttributeOptionRole30,productAttributeOptionRole31,productAttributeOptionRole32,
                productAttributeOptionRole33,productAttributeOptionRole34,productAttributeOptionRole35,productAttributeOptionRole36,productAttributeOptionRole37,productAttributeOptionRole38,productAttributeOptionRole39,productAttributeOptionRole40,productAttributeOptionRole41,productAttributeOptionRole42,productAttributeOptionRole43,productAttributeOptionRole44,productAttributeOptionRole45,productAttributeOptionRole46,productAttributeOptionRole47,productAttributeOptionRole48,
                productAttributeOptionRole49,productAttributeOptionRole50,productAttributeOptionRole51,productAttributeOptionRole52,productAttributeOptionRole53,productAttributeOptionRole54,productAttributeOptionRole55,productAttributeOptionRole56,productAttributeOptionRole57,productAttributeOptionRole58,productAttributeOptionRole59,productAttributeOptionRole60,productAttributeOptionRole61,productAttributeOptionRole62,productAttributeOptionRole63,productAttributeOptionRole64,
                productAttributeOptionRole65,productAttributeOptionRole66,productAttributeOptionRole67,productAttributeOptionRole68,productAttributeOptionRole69,productAttributeOptionRole70,productAttributeOptionRole71,productAttributeOptionRole72,productAttributeOptionRole73,productAttributeOptionRole74,productAttributeOptionRole75,productAttributeOptionRole76,productAttributeOptionRole77,productAttributeOptionRole78,productAttributeOptionRole79,productAttributeOptionRole80,
                productAttributeOptionRole81,productAttributeOptionRole82,productAttributeOptionRole83,productAttributeOptionRole84,productAttributeOptionRole85,productAttributeOptionRole86,productAttributeOptionRole87,productAttributeOptionRole88,productAttributeOptionRole89,productAttributeOptionRole90,productAttributeOptionRole91,productAttributeOptionRole92,productAttributeOptionRole93,productAttributeOptionRole94,productAttributeOptionRole95,productAttributeOptionRole96,
                productAttributeOptionRole97,productAttributeOptionRole98,productAttributeOptionRole99,productAttributeOptionRole100,productAttributeOptionRole101,productAttributeOptionRole102,productAttributeOptionRole103,productAttributeOptionRole104,productAttributeOptionRole105,productAttributeOptionRole106,productAttributeOptionRole107,productAttributeOptionRole108,productAttributeOptionRole109,productAttributeOptionRole110,productAttributeOptionRole111,productAttributeOptionRole112,
                productAttributeOptionRole113,productAttributeOptionRole114,productAttributeOptionRole115,productAttributeOptionRole116,productAttributeOptionRole117,productAttributeOptionRole118,productAttributeOptionRole119,productAttributeOptionRole120,productAttributeOptionRole121,productAttributeOptionRole122,productAttributeOptionRole123,productAttributeOptionRole124,productAttributeOptionRole125,productAttributeOptionRole126,productAttributeOptionRole127,productAttributeOptionRole128,
                productAttributeOptionRole129,productAttributeOptionRole130,productAttributeOptionRole131,productAttributeOptionRole132,productAttributeOptionRole133,productAttributeOptionRole134,productAttributeOptionRole135,productAttributeOptionRole136,productAttributeOptionRole137,productAttributeOptionRole138,productAttributeOptionRole139,productAttributeOptionRole140,productAttributeOptionRole141,productAttributeOptionRole142,productAttributeOptionRole143,productAttributeOptionRole144,
                productAttributeOptionRole145,productAttributeOptionRole146,productAttributeOptionRole147,productAttributeOptionRole148,productAttributeOptionRole149,productAttributeOptionRole150,productAttributeOptionRole151,productAttributeOptionRole152,productAttributeOptionRole153,productAttributeOptionRole154,productAttributeOptionRole155,productAttributeOptionRole156,productAttributeOptionRole157,productAttributeOptionRole158,productAttributeOptionRole159,productAttributeOptionRole160,
                productAttributeOptionRole161,productAttributeOptionRole162,productAttributeOptionRole163,productAttributeOptionRole164,productAttributeOptionRole165,productAttributeOptionRole166,productAttributeOptionRole167,productAttributeOptionRole168,productAttributeOptionRole169,productAttributeOptionRole170,productAttributeOptionRole171,productAttributeOptionRole172,productAttributeOptionRole173,productAttributeOptionRole174,productAttributeOptionRole175,productAttributeOptionRole176,
                productAttributeOptionRole177,productAttributeOptionRole178,productAttributeOptionRole179,productAttributeOptionRole180,productAttributeOptionRole181,productAttributeOptionRole182,productAttributeOptionRole183,productAttributeOptionRole184,productAttributeOptionRole185,productAttributeOptionRole186,productAttributeOptionRole187,productAttributeOptionRole188,productAttributeOptionRole189,productAttributeOptionRole190,productAttributeOptionRole191,productAttributeOptionRole192,
                productAttributeOptionRole193,productAttributeOptionRole194,productAttributeOptionRole195,productAttributeOptionRole196,productAttributeOptionRole197,productAttributeOptionRole198,productAttributeOptionRole199,productAttributeOptionRole200,productAttributeOptionRole201,productAttributeOptionRole202,productAttributeOptionRole203,productAttributeOptionRole204,productAttributeOptionRole205,productAttributeOptionRole206,productAttributeOptionRole207,productAttributeOptionRole208,
                productAttributeOptionRole209,productAttributeOptionRole210,productAttributeOptionRole211,productAttributeOptionRole212,productAttributeOptionRole213,productAttributeOptionRole214,productAttributeOptionRole215,productAttributeOptionRole216,productAttributeOptionRole217,productAttributeOptionRole218,productAttributeOptionRole219,productAttributeOptionRole220,productAttributeOptionRole221,productAttributeOptionRole222,productAttributeOptionRole223,productAttributeOptionRole224,
                productAttributeOptionRole225,productAttributeOptionRole226,productAttributeOptionRole227,productAttributeOptionRole228,productAttributeOptionRole229,productAttributeOptionRole230,productAttributeOptionRole231,productAttributeOptionRole232,productAttributeOptionRole233,productAttributeOptionRole234,productAttributeOptionRole235,productAttributeOptionRole236,productAttributeOptionRole237,productAttributeOptionRole238,productAttributeOptionRole239,productAttributeOptionRole240,
                productAttributeOptionRole241,productAttributeOptionRole242,productAttributeOptionRole243,productAttributeOptionRole244,productAttributeOptionRole245,productAttributeOptionRole246,productAttributeOptionRole247,productAttributeOptionRole248,productAttributeOptionRole249,productAttributeOptionRole250,productAttributeOptionRole251,productAttributeOptionRole252,productAttributeOptionRole253,productAttributeOptionRole254,productAttributeOptionRole255,productAttributeOptionRole256,
                productAttributeOptionRole256,productAttributeOptionRole257,productAttributeOptionRole258,productAttributeOptionRole259,productAttributeOptionRole260,productAttributeOptionRole261,productAttributeOptionRole262,productAttributeOptionRole263,productAttributeOptionRole264,productAttributeOptionRole264,productAttributeOptionRole265,productAttributeOptionRole266,productAttributeOptionRole267,productAttributeOptionRole268,productAttributeOptionRole269,productAttributeOptionRole270,
                productAttributeOptionRole271,productAttributeOptionRole272,productAttributeOptionRole273,productAttributeOptionRole274,productAttributeOptionRole275,productAttributeOptionRole276,productAttributeOptionRole277,productAttributeOptionRole278,productAttributeOptionRole279,productAttributeOptionRole280,productAttributeOptionRole281,productAttributeOptionRole282,productAttributeOptionRole283,productAttributeOptionRole284,productAttributeOptionRole285,productAttributeOptionRole286,
                productAttributeOptionRole287,productAttributeOptionRole288,productAttributeOptionRole289,productAttributeOptionRole290,productAttributeOptionRole291,productAttributeOptionRole292,productAttributeOptionRole293,productAttributeOptionRole294,productAttributeOptionRole295,productAttributeOptionRole296,productAttributeOptionRole297,productAttributeOptionRole298,productAttributeOptionRole299,productAttributeOptionRole299,productAttributeOptionRole300,productAttributeOptionRole310,
                productAttributeOptionRole311,productAttributeOptionRole312,productAttributeOptionRole313,productAttributeOptionRole314,productAttributeOptionRole315,productAttributeOptionRole316,productAttributeOptionRole317,productAttributeOptionRole318,productAttributeOptionRole319,productAttributeOptionRole320,productAttributeOptionRole321,productAttributeOptionRole322,productAttributeOptionRole323,productAttributeOptionRole324,productAttributeOptionRole325,productAttributeOptionRole326,
                productAttributeOptionRole327,productAttributeOptionRole328,productAttributeOptionRole329,productAttributeOptionRole330,productAttributeOptionRole331,productAttributeOptionRole332,productAttributeOptionRole333,productAttributeOptionRole334,productAttributeOptionRole335,productAttributeOptionRole336,productAttributeOptionRole337,productAttributeOptionRole338,productAttributeOptionRole339,productAttributeOptionRole340,productAttributeOptionRole341,productAttributeOptionRole342,
                productAttributeOptionRole343,productAttributeOptionRole344,productAttributeOptionRole345,productAttributeOptionRole346,productAttributeOptionRole347,productAttributeOptionRole348,productAttributeOptionRole349,productAttributeOptionRole350,productAttributeOptionRole351,productAttributeOptionRole352,productAttributeOptionRole353,productAttributeOptionRole354,productAttributeOptionRole355,productAttributeOptionRole356,productAttributeOptionRole357,productAttributeOptionRole358,
                productAttributeOptionRole359,productAttributeOptionRole360,productAttributeOptionRole361,productAttributeOptionRole362,productAttributeOptionRole363,productAttributeOptionRole364,productAttributeOptionRole365,productAttributeOptionRole366,productAttributeOptionRole367,productAttributeOptionRole368,productAttributeOptionRole369,productAttributeOptionRole370,productAttributeOptionRole371,productAttributeOptionRole372,productAttributeOptionRole373,productAttributeOptionRole374,
                productAttributeOptionRole375,productAttributeOptionRole376,productAttributeOptionRole377,productAttributeOptionRole378,productAttributeOptionRole379,productAttributeOptionRole380,productAttributeOptionRole381,productAttributeOptionRole382,productAttributeOptionRole383,productAttributeOptionRole384,productAttributeOptionRole385,productAttributeOptionRole386,productAttributeOptionRole387,productAttributeOptionRole388,productAttributeOptionRole389,productAttributeOptionRole390,
                productAttributeOptionRole391,productAttributeOptionRole392,productAttributeOptionRole393,productAttributeOptionRole394,productAttributeOptionRole395,productAttributeOptionRole396,productAttributeOptionRole397,productAttributeOptionRole398,productAttributeOptionRole399,productAttributeOptionRole400,productAttributeOptionRole401,productAttributeOptionRole402,productAttributeOptionRole403,productAttributeOptionRole404,productAttributeOptionRole405,productAttributeOptionRole406,
                productAttributeOptionRole407,productAttributeOptionRole408,productAttributeOptionRole409,productAttributeOptionRole410,productAttributeOptionRole411,productAttributeOptionRole412,productAttributeOptionRole413,productAttributeOptionRole414,productAttributeOptionRole415,productAttributeOptionRole416,productAttributeOptionRole417,productAttributeOptionRole418,productAttributeOptionRole419,productAttributeOptionRole420,productAttributeOptionRole421,productAttributeOptionRole422,
                productAttributeOptionRole423,productAttributeOptionRole424,productAttributeOptionRole425,productAttributeOptionRole426,productAttributeOptionRole427,productAttributeOptionRole428,productAttributeOptionRole429,productAttributeOptionRole430,productAttributeOptionRole431,productAttributeOptionRole432,productAttributeOptionRole433,productAttributeOptionRole434,productAttributeOptionRole435,productAttributeOptionRole436,productAttributeOptionRole437,productAttributeOptionRole438,
                productAttributeOptionRole439,productAttributeOptionRole440,productAttributeOptionRole441,productAttributeOptionRole442,productAttributeOptionRole443,productAttributeOptionRole444,productAttributeOptionRole445,productAttributeOptionRole446,productAttributeOptionRole447,productAttributeOptionRole448,productAttributeOptionRole449,productAttributeOptionRole450,productAttributeOptionRole451,productAttributeOptionRole452,productAttributeOptionRole453,productAttributeOptionRole454,
                productAttributeOptionRole455,productAttributeOptionRole456,productAttributeOptionRole457,productAttributeOptionRole458,productAttributeOptionRole459,productAttributeOptionRole460,productAttributeOptionRole461,productAttributeOptionRole462,productAttributeOptionRole463,productAttributeOptionRole464,productAttributeOptionRole465,productAttributeOptionRole466,productAttributeOptionRole467,productAttributeOptionRole468,productAttributeOptionRole469,productAttributeOptionRole470,
                productAttributeOptionRole471,productAttributeOptionRole472,productAttributeOptionRole473,productAttributeOptionRole474,productAttributeOptionRole475,productAttributeOptionRole476,productAttributeOptionRole477,productAttributeOptionRole478,productAttributeOptionRole479,productAttributeOptionRole480,productAttributeOptionRole481,productAttributeOptionRole482,productAttributeOptionRole483,productAttributeOptionRole484,productAttributeOptionRole485,productAttributeOptionRole486,
                productAttributeOptionRole487,productAttributeOptionRole488,productAttributeOptionRole489,productAttributeOptionRole490,productAttributeOptionRole491,productAttributeOptionRole492,productAttributeOptionRole493,productAttributeOptionRole494,productAttributeOptionRole495,productAttributeOptionRole496,productAttributeOptionRole497,productAttributeOptionRole498,productAttributeOptionRole499,productAttributeOptionRole500,productAttributeOptionRole501,productAttributeOptionRole502,
                productAttributeOptionRole503,productAttributeOptionRole504,productAttributeOptionRole505,productAttributeOptionRole506,productAttributeOptionRole507,productAttributeOptionRole508,productAttributeOptionRole509,
                productAttributeOptionRole510,productAttributeOptionRole511,productAttributeOptionRole512
            });
        }

        #endregion


        #region ProductAttributeOptionValue
        var productAttributeOptionValue1 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption1.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue2 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption1.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue3 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption2.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue4 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption2.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue5 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption3.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue6 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption3.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue7 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption4.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue8 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption4.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue9 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption5.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue10 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption5.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue11 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption6.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue12 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption6.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue13 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption7.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue14 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption7.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue15 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption8.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue16 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption8.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue17 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption9.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue18 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption9.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue19 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption10.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue20 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption10.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue21 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption11.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue22 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption11.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue23 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption12.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue24 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption12.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue25 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption13.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue26 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption13.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue27 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption14.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue28 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption14.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue29 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption15.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue30 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption15.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue31 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption16.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue32 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption16.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue33 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption17.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue34 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption17.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue35 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption18.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue36 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption18.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue37 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption19.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue38 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption19.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue39 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption20.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue40 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption20.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue41 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption21.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue42 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption21.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue43 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption22.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue44 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption22.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue45 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption23.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue46 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption23.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue47 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption24.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue48 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption24.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue49 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption25.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue50 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption25.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue51 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption26.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue52 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption26.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue53 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption27.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue54 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption27.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue55 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption28.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue56 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption28.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue57 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption29.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue58 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption29.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue59 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption30.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue60 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption30.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue61 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption31.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue62 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption31.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue63 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption32.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue64 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption32.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue65 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption33.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue66 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption33.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue67 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption34.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue68 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption34.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue69 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption35.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue70 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption35.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue71 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption36.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue72 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption36.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue73 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption37.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue74 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption37.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue75 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption38.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue76 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption38.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue77 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption39.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue78 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption39.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue79 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption40.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue80 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption40.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue81 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption41.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue82 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption41.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue83 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption42.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue84 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption42.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue85 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption43.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue86 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption43.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue87 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption44.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue88 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption44.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue89 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption45.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue90 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption45.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue91 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption46.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue92 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption46.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue93 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption47.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue94 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption47.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue95 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption48.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue96 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption48.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue97 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption49.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue98 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption49.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue99 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption50.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue100 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption50.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue101 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption51.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue102 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption51.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue103 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption52.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue104 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption52.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue105 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption53.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue106 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption53.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue107 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption54.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue108 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption54.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue109 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption55.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue110 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption55.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue111 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption56.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue112 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption56.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue113 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption57.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue114 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption57.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue115 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption58.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue116 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption58.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue117 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption59.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue118 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption59.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue119 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption60.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue120 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption60.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue121 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption61.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue122 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption61.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue123 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption62.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue124 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption62.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue125 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption63.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue126 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption63.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue127 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption64.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue128 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption64.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue129 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption65.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue130 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption65.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue131 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption66.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue132 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption66.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue133 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption67.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue134 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption67.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue135 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption68.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue136 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption68.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue137 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption69.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue138 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption69.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue139 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption70.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue140 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption70.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue141 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption71.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue142 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption71.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue143 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption72.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue144 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption72.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue145 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption73.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue146 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption73.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue147 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption74.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue148 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption74.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue149 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption75.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue150 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption75.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue151 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption76.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue152 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption76.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue153 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption77.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue154 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption77.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue155 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption78.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue156 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption78.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue157 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption79.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue158 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption79.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue159 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption80.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue160 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption80.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue161 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption81.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue162 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption81.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue163 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption82.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue164 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption82.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue165 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption83.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue166 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption83.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue167 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption84.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue168 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption84.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue169 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption85.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue170 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption85.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue171 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption86.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue172 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption86.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue173 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption87.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue174 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption87.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue175 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption88.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue176 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption88.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue177 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption89.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue178 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption89.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue179 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption90.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue180 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption90.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue181 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption91.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue182 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption91.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue183 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption92.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue184 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption92.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue185 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption93.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue186 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption93.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue187 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption94.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue188 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption94.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue189 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption95.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue190 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption95.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue191 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption96.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue192 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption96.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue193 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption97.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue194 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption97.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue195 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption98.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue196 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption98.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue197 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption99.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue198 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption99.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue199 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption100.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue200 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption100.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue201 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption101.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue202 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption101.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue203 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption102.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue204 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption102.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue205 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption103.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue206 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption103.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue207 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption104.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue208 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption104.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue209 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption105.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue210 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption105.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue211 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption106.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue212 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption106.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue213 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption107.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue214 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption107.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue215 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption108.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue216 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption108.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue217 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption109.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue218 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption109.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue219 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption110.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue220 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption110.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue221 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption111.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue222 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption111.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue223 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption112.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue224 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption112.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue225 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption113.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue226 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption113.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue227 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption114.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue228 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption114.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue229 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption115.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue230 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption115.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue231 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption116.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue232 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption116.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue233 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption117.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue234 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption117.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue235 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption118.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue236 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption118.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue237 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption119.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue238 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption119.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue239 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption120.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue240 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption120.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue241 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption121.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue242 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption121.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue243 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption122.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue244 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption122.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue245 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption123.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue246 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption123.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue247 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption124.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue248 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption124.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue249 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption125.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue250 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption125.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue251 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption126.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue252 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption126.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue253 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption127.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue254 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption127.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue255 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption128.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue256 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption128.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue257 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption129.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue258 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption129.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue259 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption130.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue260 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption130.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue261 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption131.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue262 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption131.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue263 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption132.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue264 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption132.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue265 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption133.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue266 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption133.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue267 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption134.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue268 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption134.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue269 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption135.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue270 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption135.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue271 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption136.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue272 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption136.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue273 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption137.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue274 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption137.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue275 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption138.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue276 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption138.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue277 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption139.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue278 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption139.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue279 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption140.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue280 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption140.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue281 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption141.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue282 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption141.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue283 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption142.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue284 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption142.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue285 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption143.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue286 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption143.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue287 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption144.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue288 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption144.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue289 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption145.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue290 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption145.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue291 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption146.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue292 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption146.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue293 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption147.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue294 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption147.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue295 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption148.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue296 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption148.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue297 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption149.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue298 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption149.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue299 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption150.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue300 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption150.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue301 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption151.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue302 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption151.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue303 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption152.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue304 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption152.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue305 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption153.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue306 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption153.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue307 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption154.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue308 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption154.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue309 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption155.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue310 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption155.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue311 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption156.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue312 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption156.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue313 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption157.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue314 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption157.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue315 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption158.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue316 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption158.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue317 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption159.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue318 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption159.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue319 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption160.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue320 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption160.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue321 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption161.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue322 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption161.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue323 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption162.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue324 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption162.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue325 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption163.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue326 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption163.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue327 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption164.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue328 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption164.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue329 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption165.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue330 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption165.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue331 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption166.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue332 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption166.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue333 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption167.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue334 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption167.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue335 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption168.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue336 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption168.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue337 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption169.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue338 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption169.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue339 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption170.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue340 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption170.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue341 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption171.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue342 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption171.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue343 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption172.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue344 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption172.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue345 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption173.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue346 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption173.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue347 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption174.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue348 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption174.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue349 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption175.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue350 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption175.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue351 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption176.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue352 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption176.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue353 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption177.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue354 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption177.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue355 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption178.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue356 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption178.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue357 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption179.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue358 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption179.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue359 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption180.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue360 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption180.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue361 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption181.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue362 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption181.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue363 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption182.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue364 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption182.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue365 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption183.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue366 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption183.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue367 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption184.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue368 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption184.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue369 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption185.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue370 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption185.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue371 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption186.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue372 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption186.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue373 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption187.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue374 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption187.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue375 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption188.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue376 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption188.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue377 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption189.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue378 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption189.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue379 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption190.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue380 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption190.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue381 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption191.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue382 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption191.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue383 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption192.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue384 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption192.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue385 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption193.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue386 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption193.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue387 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption194.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue388 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption194.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue389 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption195.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue390 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption195.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue391 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption196.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue392 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption196.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue393 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption197.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue394 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption197.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue395 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption198.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue396 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption198.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue397 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption199.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue398 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption199.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue399 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption200.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue400 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption200.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue401 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption201.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue402 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption201.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue403 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption202.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue404 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption202.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue405 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption203.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue406 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption203.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue407 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption204.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue408 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption204.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue409 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption205.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue410 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption205.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue411 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption206.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue412 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption206.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue413 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption207.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue414 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption207.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue415 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption208.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue416 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption208.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue417 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption209.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue418 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption209.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue419 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption210.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue420 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption210.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue421 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption211.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue422 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption211.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue423 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption212.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue424 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption212.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue425 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption213.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue426 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption213.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue427 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption214.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue428 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption214.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue429 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption215.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue430 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption215.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue431 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption216.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue432 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption216.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue433 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption217.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue434 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption217.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue435 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption218.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue436 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption218.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue437 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption219.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue438 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption219.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue439 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption220.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue440 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption220.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue441 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption221.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue442 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption221.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue443 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption222.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue444 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption222.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue445 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption223.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue446 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption223.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue447 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption224.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue448 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption224.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue449 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption225.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue450 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption225.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue451 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption226.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue452 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption226.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue453 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption227.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue454 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption227.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue455 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption228.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue456 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption228.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue457 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption229.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue458 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption229.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue459 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption230.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue460 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption230.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue461 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption231.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue462 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption231.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue463 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption232.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue464 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption232.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue465 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption233.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue466 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption233.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue467 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption234.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue468 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption234.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue469 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption235.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue470 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption235.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue471 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption236.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue472 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption236.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue473 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption237.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue474 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption237.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue475 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption238.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue476 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption238.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue477 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption239.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue478 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption239.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue479 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption240.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue480 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption240.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue481 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption241.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue482 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption241.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue483 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption242.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue484 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption242.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue485 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption243.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue486 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption243.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue487 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption244.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue488 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption244.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue489 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption245.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue490 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption245.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue491 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption246.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue492 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption246.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue493 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption247.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue494 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption247.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue495 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption248.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue496 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption248.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue497 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption249.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue498 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption249.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue499 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption250.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue500 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption250.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue501 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption251.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue502 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption251.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue503 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption252.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue504 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption252.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue505 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption253.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue506 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption253.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue507 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption254.Id, Name = productOptionColor1.Name, Value = productOptionValueColor1.Name };
        var productAttributeOptionValue508 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption254.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };
        var productAttributeOptionValue509 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption255.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue510 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption255.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial1.Name };
        var productAttributeOptionValue511 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption256.Id, Name = productOptionColor1.Name, Value = productOptionValueColor2.Name };
        var productAttributeOptionValue512 = new ProductAttributeOptionValue() { ProductAttributeOptionId = productAttributeOption256.Id, Name = productOptionMaterial1.Name, Value = productOptionValueMaterial2.Name };

        if (!_context.ProductAttributeOptionValues.Any())
        {
            _context.ProductAttributeOptionValues.AddRange(new List<ProductAttributeOptionValue>()
          {
                productAttributeOptionValue1,productAttributeOptionValue2,productAttributeOptionValue3,productAttributeOptionValue4,productAttributeOptionValue5,productAttributeOptionValue6,productAttributeOptionValue7,productAttributeOptionValue8,productAttributeOptionValue9,productAttributeOptionValue10,productAttributeOptionValue11,productAttributeOptionValue12,productAttributeOptionValue13,productAttributeOptionValue14,productAttributeOptionValue15,productAttributeOptionValue16,
                productAttributeOptionValue17,productAttributeOptionValue18,productAttributeOptionValue19,productAttributeOptionValue20,productAttributeOptionValue21,productAttributeOptionValue22,productAttributeOptionValue23,productAttributeOptionValue24,productAttributeOptionValue25,productAttributeOptionValue26,productAttributeOptionValue27,productAttributeOptionValue28,productAttributeOptionValue29,productAttributeOptionValue30,productAttributeOptionValue31,productAttributeOptionValue32,
                productAttributeOptionValue33,productAttributeOptionValue34,productAttributeOptionValue35,productAttributeOptionValue36,productAttributeOptionValue37,productAttributeOptionValue38,productAttributeOptionValue39,productAttributeOptionValue40,productAttributeOptionValue41,productAttributeOptionValue42,productAttributeOptionValue43,productAttributeOptionValue44,productAttributeOptionValue45,productAttributeOptionValue46,productAttributeOptionValue47,productAttributeOptionValue48,
                productAttributeOptionValue49,productAttributeOptionValue50,productAttributeOptionValue51,productAttributeOptionValue52,productAttributeOptionValue53,productAttributeOptionValue54,productAttributeOptionValue55,productAttributeOptionValue56,productAttributeOptionValue57,productAttributeOptionValue58,productAttributeOptionValue59,productAttributeOptionValue60,productAttributeOptionValue61,productAttributeOptionValue62,productAttributeOptionValue63,productAttributeOptionValue64,
                productAttributeOptionValue65,productAttributeOptionValue66,productAttributeOptionValue67,productAttributeOptionValue68,productAttributeOptionValue69,productAttributeOptionValue70,productAttributeOptionValue71,productAttributeOptionValue72,productAttributeOptionValue73,productAttributeOptionValue74,productAttributeOptionValue75,productAttributeOptionValue76,productAttributeOptionValue77,productAttributeOptionValue78,productAttributeOptionValue79,productAttributeOptionValue80,
                productAttributeOptionValue81,productAttributeOptionValue82,productAttributeOptionValue83,productAttributeOptionValue84,productAttributeOptionValue85,productAttributeOptionValue86,productAttributeOptionValue87,productAttributeOptionValue88,productAttributeOptionValue89,productAttributeOptionValue90,productAttributeOptionValue91,productAttributeOptionValue92,productAttributeOptionValue93,productAttributeOptionValue94,productAttributeOptionValue95,productAttributeOptionValue96,
                productAttributeOptionValue97,productAttributeOptionValue98,productAttributeOptionValue99,productAttributeOptionValue100,productAttributeOptionValue101,productAttributeOptionValue102,productAttributeOptionValue103,productAttributeOptionValue104,productAttributeOptionValue105,productAttributeOptionValue106,productAttributeOptionValue107,productAttributeOptionValue108,productAttributeOptionValue109,productAttributeOptionValue110,productAttributeOptionValue111,productAttributeOptionValue112,
                productAttributeOptionValue113,productAttributeOptionValue114,productAttributeOptionValue115,productAttributeOptionValue116,productAttributeOptionValue117,productAttributeOptionValue118,productAttributeOptionValue119,productAttributeOptionValue120,productAttributeOptionValue121,productAttributeOptionValue122,productAttributeOptionValue123,productAttributeOptionValue124,productAttributeOptionValue125,productAttributeOptionValue126,productAttributeOptionValue127,productAttributeOptionValue128,
                productAttributeOptionValue129,productAttributeOptionValue130,productAttributeOptionValue131,productAttributeOptionValue132,productAttributeOptionValue133,productAttributeOptionValue134,productAttributeOptionValue135,productAttributeOptionValue136,productAttributeOptionValue137,productAttributeOptionValue138,productAttributeOptionValue139,productAttributeOptionValue140,productAttributeOptionValue141,productAttributeOptionValue142,productAttributeOptionValue143,productAttributeOptionValue144,
                productAttributeOptionValue145,productAttributeOptionValue146,productAttributeOptionValue147,productAttributeOptionValue148,productAttributeOptionValue149,productAttributeOptionValue150,productAttributeOptionValue151,productAttributeOptionValue152,productAttributeOptionValue153,productAttributeOptionValue154,productAttributeOptionValue155,productAttributeOptionValue156,productAttributeOptionValue157,productAttributeOptionValue158,productAttributeOptionValue159,productAttributeOptionValue160,
                productAttributeOptionValue161,productAttributeOptionValue162,productAttributeOptionValue163,productAttributeOptionValue164,productAttributeOptionValue165,productAttributeOptionValue166,productAttributeOptionValue167,productAttributeOptionValue168,productAttributeOptionValue169,productAttributeOptionValue170,productAttributeOptionValue171,productAttributeOptionValue172,productAttributeOptionValue173,productAttributeOptionValue174,productAttributeOptionValue175,productAttributeOptionValue176,
                productAttributeOptionValue177,productAttributeOptionValue178,productAttributeOptionValue179,productAttributeOptionValue180,productAttributeOptionValue181,productAttributeOptionValue182,productAttributeOptionValue183,productAttributeOptionValue184,productAttributeOptionValue185,productAttributeOptionValue186,productAttributeOptionValue187,productAttributeOptionValue188,productAttributeOptionValue189,productAttributeOptionValue190,productAttributeOptionValue191,productAttributeOptionValue192,
                productAttributeOptionValue193,productAttributeOptionValue194,productAttributeOptionValue195,productAttributeOptionValue196,productAttributeOptionValue197,productAttributeOptionValue198,productAttributeOptionValue199,productAttributeOptionValue200,productAttributeOptionValue201,productAttributeOptionValue202,productAttributeOptionValue203,productAttributeOptionValue204,productAttributeOptionValue205,productAttributeOptionValue206,productAttributeOptionValue207,productAttributeOptionValue208,
                productAttributeOptionValue209,productAttributeOptionValue210,productAttributeOptionValue211,productAttributeOptionValue212,productAttributeOptionValue213,productAttributeOptionValue214,productAttributeOptionValue215,productAttributeOptionValue216,productAttributeOptionValue217,productAttributeOptionValue218,productAttributeOptionValue219,productAttributeOptionValue220,productAttributeOptionValue221,productAttributeOptionValue222,productAttributeOptionValue223,productAttributeOptionValue224,
                productAttributeOptionValue225,productAttributeOptionValue226,productAttributeOptionValue227,productAttributeOptionValue228,productAttributeOptionValue229,productAttributeOptionValue230,productAttributeOptionValue231,productAttributeOptionValue232,productAttributeOptionValue233,productAttributeOptionValue234,productAttributeOptionValue235,productAttributeOptionValue236,productAttributeOptionValue237,productAttributeOptionValue238,productAttributeOptionValue239,productAttributeOptionValue240,
                productAttributeOptionValue241,productAttributeOptionValue242,productAttributeOptionValue243,productAttributeOptionValue244,productAttributeOptionValue245,productAttributeOptionValue246,productAttributeOptionValue247,productAttributeOptionValue248,productAttributeOptionValue249,productAttributeOptionValue250,productAttributeOptionValue251,productAttributeOptionValue252,productAttributeOptionValue253,productAttributeOptionValue254,productAttributeOptionValue255,productAttributeOptionValue256,
                productAttributeOptionValue257,productAttributeOptionValue258,productAttributeOptionValue259,productAttributeOptionValue260,productAttributeOptionValue261,productAttributeOptionValue262,productAttributeOptionValue263,productAttributeOptionValue264,productAttributeOptionValue265,productAttributeOptionValue266,productAttributeOptionValue267,productAttributeOptionValue268,productAttributeOptionValue269,productAttributeOptionValue270,productAttributeOptionValue271,productAttributeOptionValue272,
                productAttributeOptionValue273,productAttributeOptionValue274,productAttributeOptionValue275,productAttributeOptionValue276,productAttributeOptionValue277,productAttributeOptionValue278,productAttributeOptionValue279,productAttributeOptionValue280,productAttributeOptionValue281,productAttributeOptionValue282,productAttributeOptionValue283,productAttributeOptionValue284,productAttributeOptionValue285,productAttributeOptionValue286,productAttributeOptionValue287,productAttributeOptionValue288,
                productAttributeOptionValue289,productAttributeOptionValue290,productAttributeOptionValue291,productAttributeOptionValue292,productAttributeOptionValue293,productAttributeOptionValue294,productAttributeOptionValue295,productAttributeOptionValue296,productAttributeOptionValue297,productAttributeOptionValue298,productAttributeOptionValue299,productAttributeOptionValue300,productAttributeOptionValue301,productAttributeOptionValue302,productAttributeOptionValue303,productAttributeOptionValue304,
                productAttributeOptionValue305,productAttributeOptionValue306,productAttributeOptionValue307,productAttributeOptionValue308,productAttributeOptionValue309,productAttributeOptionValue310,productAttributeOptionValue311,productAttributeOptionValue312,productAttributeOptionValue313,productAttributeOptionValue314,productAttributeOptionValue315,productAttributeOptionValue316,productAttributeOptionValue317,productAttributeOptionValue318,productAttributeOptionValue319,productAttributeOptionValue320,
                productAttributeOptionValue321,productAttributeOptionValue322,productAttributeOptionValue323,productAttributeOptionValue324,productAttributeOptionValue325,productAttributeOptionValue326,productAttributeOptionValue327,productAttributeOptionValue328,productAttributeOptionValue329,productAttributeOptionValue330,productAttributeOptionValue331,productAttributeOptionValue332,productAttributeOptionValue333,productAttributeOptionValue334,productAttributeOptionValue335,productAttributeOptionValue336,
                productAttributeOptionValue337,productAttributeOptionValue338,productAttributeOptionValue339,productAttributeOptionValue340,productAttributeOptionValue341,productAttributeOptionValue342,productAttributeOptionValue343,productAttributeOptionValue344,productAttributeOptionValue345,productAttributeOptionValue346,productAttributeOptionValue347,productAttributeOptionValue348,productAttributeOptionValue349,productAttributeOptionValue350,productAttributeOptionValue351,productAttributeOptionValue352,
                productAttributeOptionValue353,productAttributeOptionValue354,productAttributeOptionValue355,productAttributeOptionValue356,productAttributeOptionValue357,productAttributeOptionValue358,productAttributeOptionValue359,productAttributeOptionValue360,productAttributeOptionValue361,productAttributeOptionValue362,productAttributeOptionValue363,productAttributeOptionValue364,productAttributeOptionValue365,productAttributeOptionValue366,productAttributeOptionValue367,productAttributeOptionValue368,
                productAttributeOptionValue369,productAttributeOptionValue370,productAttributeOptionValue371,productAttributeOptionValue372,productAttributeOptionValue373,productAttributeOptionValue374,productAttributeOptionValue375,productAttributeOptionValue376,productAttributeOptionValue377,productAttributeOptionValue378,productAttributeOptionValue379,productAttributeOptionValue380,productAttributeOptionValue381,productAttributeOptionValue382,productAttributeOptionValue383,productAttributeOptionValue384,
                productAttributeOptionValue385,productAttributeOptionValue386,productAttributeOptionValue387,productAttributeOptionValue388,productAttributeOptionValue389,productAttributeOptionValue390,productAttributeOptionValue391,productAttributeOptionValue392,productAttributeOptionValue393,productAttributeOptionValue394,productAttributeOptionValue395,productAttributeOptionValue396,productAttributeOptionValue397,productAttributeOptionValue398,productAttributeOptionValue399,productAttributeOptionValue400,
                productAttributeOptionValue401,productAttributeOptionValue402,productAttributeOptionValue403,productAttributeOptionValue404,productAttributeOptionValue405,productAttributeOptionValue406,productAttributeOptionValue407,productAttributeOptionValue408,productAttributeOptionValue409,productAttributeOptionValue410,productAttributeOptionValue411,productAttributeOptionValue412,productAttributeOptionValue413,productAttributeOptionValue414,productAttributeOptionValue415,productAttributeOptionValue416,
                productAttributeOptionValue417,productAttributeOptionValue418,productAttributeOptionValue419,productAttributeOptionValue420,productAttributeOptionValue421,productAttributeOptionValue422,
                productAttributeOptionValue423,productAttributeOptionValue424,productAttributeOptionValue425,productAttributeOptionValue426,productAttributeOptionValue427,productAttributeOptionValue428,productAttributeOptionValue429,productAttributeOptionValue430,productAttributeOptionValue431,productAttributeOptionValue432,productAttributeOptionValue433,productAttributeOptionValue434,productAttributeOptionValue435,productAttributeOptionValue436,productAttributeOptionValue437,productAttributeOptionValue438,
                productAttributeOptionValue439,productAttributeOptionValue440,productAttributeOptionValue441,productAttributeOptionValue442,productAttributeOptionValue443,productAttributeOptionValue444,productAttributeOptionValue445,productAttributeOptionValue446,productAttributeOptionValue447,productAttributeOptionValue448,productAttributeOptionValue449,productAttributeOptionValue450,productAttributeOptionValue451,productAttributeOptionValue452,productAttributeOptionValue453,productAttributeOptionValue454,
                productAttributeOptionValue455,productAttributeOptionValue456,productAttributeOptionValue457,productAttributeOptionValue458,productAttributeOptionValue459,productAttributeOptionValue460,productAttributeOptionValue461,productAttributeOptionValue462,productAttributeOptionValue463,productAttributeOptionValue464,productAttributeOptionValue465,productAttributeOptionValue466,productAttributeOptionValue467,productAttributeOptionValue468,productAttributeOptionValue469,productAttributeOptionValue470,
                productAttributeOptionValue471,productAttributeOptionValue472,productAttributeOptionValue473,productAttributeOptionValue474,productAttributeOptionValue475,productAttributeOptionValue476,productAttributeOptionValue477,productAttributeOptionValue478,productAttributeOptionValue479,productAttributeOptionValue480,productAttributeOptionValue481,productAttributeOptionValue482,productAttributeOptionValue483,productAttributeOptionValue484,productAttributeOptionValue485,productAttributeOptionValue486,
                productAttributeOptionValue487,productAttributeOptionValue488,productAttributeOptionValue489,productAttributeOptionValue490,productAttributeOptionValue491,productAttributeOptionValue492,productAttributeOptionValue493,productAttributeOptionValue494,productAttributeOptionValue495,productAttributeOptionValue496,productAttributeOptionValue497,productAttributeOptionValue498,productAttributeOptionValue499,productAttributeOptionValue500,productAttributeOptionValue501,productAttributeOptionValue502,
                productAttributeOptionValue503,productAttributeOptionValue504,productAttributeOptionValue505,productAttributeOptionValue506,productAttributeOptionValue507,productAttributeOptionValue508,productAttributeOptionValue509,productAttributeOptionValue510,productAttributeOptionValue511,productAttributeOptionValue512
          });
        }
        #endregion

        #region Price


        var price1 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption1.Id
        };
        var price2 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption1.Id
        };
        var price3 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption2.Id
        };
        var price4 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption2.Id
        };
        var price5 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption3.Id
        };
        var price6 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption3.Id
        };
        var price7 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption4.Id
        };
        var price8 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption4.Id
        };
        var price9 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption5.Id
        };
        var price10 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption5.Id
        };
        var price11 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption6.Id
        };
        var price12 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption6.Id
        };
        var price13 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 16000,
            ProductAttributeOptionId = productAttributeOption7.Id
        };
        var price14 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption7.Id
        };
        var price15 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 17000,
            ProductAttributeOptionId = productAttributeOption8.Id
        };
        var price16 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption8.Id
        };
        var price17 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 18000,
            ProductAttributeOptionId = productAttributeOption9.Id
        };
        var price18 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption9.Id
        };
        var price19 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 19000,
            ProductAttributeOptionId = productAttributeOption10.Id
        };
        var price20 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption10.Id
        };
        var price21 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 20000,
            ProductAttributeOptionId = productAttributeOption11.Id
        };
        var price22 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption11.Id
        };
        var price23 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption12.Id
        };
        var price24 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption12.Id
        };
        var price25 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption13.Id
        };
        var price26 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption13.Id
        };
        var price27 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption14.Id
        };
        var price28 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption14.Id
        };
        var price29 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption15.Id
        };
        var price30 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption15.Id
        };
        var price31 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption16.Id
        };
        var price32 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption16.Id
        };
        var price33 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption17.Id
        };
        var price34 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption17.Id
        };
        var price35 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption18.Id
        };
        var price36 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption18.Id
        };
        var price37 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption19.Id
        };
        var price38 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption19.Id
        };
        var price39 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption20.Id
        };
        var price40 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption20.Id
        };
        var price41 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption21.Id
        };
        var price42 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption21.Id
        };
        var price43 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption22.Id
        };
        var price44 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption22.Id
        };
        var price45 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption23.Id
        };
        var price46 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption23.Id
        };
        var price47 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption24.Id
        };
        var price48 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption24.Id
        };
        var price49 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption25.Id
        };
        var price50 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption25.Id
        };
        var price51 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption26.Id
        };
        var price52 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption26.Id
        };
        var price53 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption27.Id
        };
        var price54 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption27.Id
        };
        var price55 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption28.Id
        };
        var price56 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption28.Id
        };
        var price57 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption29.Id
        };
        var price58 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption29.Id
        };
        var price59 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption30.Id
        };
        var price60 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption30.Id
        };
        var price61 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption31.Id
        };
        var price62 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption31.Id
        };
        var price63 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption32.Id
        };
        var price64 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption32.Id
        };
        var price65 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption33.Id
        };
        var price66 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption33.Id
        };
        var price67 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption34.Id
        };
        var price68 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption34.Id
        };
        var price69 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption35.Id
        };
        var price70 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption35.Id
        };
        var price71 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption36.Id
        };
        var price72 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption36.Id
        };
        var price73 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption37.Id
        };
        var price74 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption37.Id
        };
        var price75 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption38.Id
        };
        var price76 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption38.Id
        };
        var price77 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption39.Id
        };
        var price78 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption39.Id
        };
        var price79 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption40.Id
        };
        var price80 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption40.Id
        };
        var price81 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption41.Id
        };
        var price82 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption41.Id
        };
        var price83 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption42.Id
        };
        var price84 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption42.Id
        };
        var price85 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption43.Id
        };
        var price86 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption43.Id
        };
        var price87 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption44.Id
        };
        var price88 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption44.Id
        };
        var price89 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption45.Id
        };
        var price90 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption45.Id
        };
        var price91 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption46.Id
        };
        var price92 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption46.Id
        };
        var price93 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption47.Id
        };
        var price94 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption47.Id
        };
        var price95 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption48.Id
        };
        var price96 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption48.Id
        };
        var price97 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption49.Id
        };
        var price98 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption49.Id
        };
        var price99 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption50.Id
        };
        var price100 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption50.Id
        };
        var price101 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption51.Id
        };
        var price102 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption51.Id
        };
        var price103 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption52.Id
        };
        var price104 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption52.Id
        };
        var price105 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption53.Id
        };
        var price106 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption53.Id
        };
        var price107 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption54.Id
        };
        var price108 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption54.Id
        };
        var price109 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption55.Id
        };
        var price110 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption55.Id
        };
        var price111 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption56.Id
        };
        var price112 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption56.Id
        };
        var price113 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption57.Id
        };
        var price114 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption57.Id
        };
        var price115 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption58.Id
        };
        var price116 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption58.Id
        };
        var price117 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption59.Id
        };
        var price118 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption59.Id
        };
        var price119 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption60.Id
        };
        var price120 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption60.Id
        };
        var price121 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption61.Id
        };
        var price122 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption61.Id
        };
        var price123 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption62.Id
        };
        var price124 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption62.Id
        };
        var price125 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption63.Id
        };
        var price126 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption63.Id
        };
        var price127 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption64.Id
        };
        var price128 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption64.Id
        };
        var price129 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 1000,
            ProductAttributeOptionId = productAttributeOption65.Id
        };
        var price130 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption65.Id
        };
        var price131 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption66.Id
        };
        var price132 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption66.Id
        };
        var price133 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption67.Id
        };
        var price134 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption67.Id
        };
        var price135 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption68.Id
        };
        var price136 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption68.Id
        };
        var price137 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption69.Id
        };
        var price138 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption69.Id
        };
        var price139 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption70.Id
        };
        var price140 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption70.Id
        };
        var price141 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption71.Id
        };
        var price142 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption71.Id
        };
        var price143 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption72.Id
        };
        var price144 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption72.Id
        };
        var price145 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption73.Id
        };
        var price146 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption73.Id
        };
        var price147 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption74.Id
        };
        var price148 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption74.Id
        };
        var price149 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption75.Id
        };
        var price150 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption75.Id
        };
        var price151 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption176.Id
        };
        var price152 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption76.Id
        };
        var price153 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption77.Id
        };
        var price154 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption77.Id
        };
        var price155 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption78.Id
        };
        var price156 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption78.Id
        };
        var price157 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption79.Id
        };
        var price158 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption79.Id
        };
        var price159 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption80.Id
        };
        var price160 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption80.Id
        };
        var price161 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption81.Id
        };
        var price162 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption81.Id
        };
        var price163 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption82.Id
        };
        var price164 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption82.Id
        };
        var price165 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption83.Id
        };
        var price166 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption83.Id
        };
        var price167 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption84.Id
        };
        var price168 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption84.Id
        };
        var price169 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption85.Id
        };
        var price170 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption85.Id
        };
        var price171 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption86.Id
        };
        var price172 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption86.Id
        };
        var price173 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption87.Id
        };
        var price174 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption87.Id
        };
        var price175 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption88.Id
        };
        var price176 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption88.Id
        };
        var price177 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption89.Id
        };
        var price178 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption89.Id
        };
        var price179 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption90.Id
        };
        var price180 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption90.Id
        };
        var price181 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption91.Id
        };
        var price182 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption91.Id
        };
        var price183 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption92.Id
        };
        var price184 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption92.Id
        };
        var price185 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption93.Id
        };
        var price186 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption93.Id
        };
        var price187 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption94.Id
        };
        var price188 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption94.Id
        };
        var price189 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption95.Id
        };
        var price190 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption95.Id
        };
        var price191 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption96.Id
        };
        var price192 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption96.Id
        };
        var price193 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption97.Id
        };
        var price194 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption97.Id
        };
        var price195 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption98.Id
        };
        var price196 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption98.Id
        };
        var price197 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption99.Id
        };
        var price198 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption99.Id
        };
        var price199 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption100.Id
        };
        var price200 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption100.Id
        };
        var price201 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption101.Id
        };
        var price202 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption101.Id
        };
        var price203 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption102.Id
        };
        var price204 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption102.Id
        };
        var price205 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption103.Id
        };
        var price206 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption103.Id
        };
        var price207 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption104.Id
        };
        var price208 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption104.Id
        };
        var price209 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption105.Id
        };
        var price210 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption105.Id
        };
        var price211 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption106.Id
        };
        var price212 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption106.Id
        };
        var price213 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption107.Id
        };
        var price214 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption107.Id
        };
        var price215 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption108.Id
        };
        var price216 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption108.Id
        };
        var price217 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption109.Id
        };
        var price218 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption109.Id
        };
        var price219 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption110.Id
        };
        var price220 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption110.Id
        };
        var price221 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption111.Id
        };
        var price222 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption111.Id
        };
        var price223 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption112.Id
        };
        var price224 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption112.Id
        };
        var price225 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption113.Id
        };
        var price226 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption113.Id
        };
        var price227 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption114.Id
        };
        var price228 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption114.Id
        };
        var price229 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption115.Id
        };
        var price230 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption115.Id
        };
        var price231 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption116.Id
        };
        var price232 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption116.Id
        };
        var price233 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption117.Id
        };
        var price234 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption117.Id
        };
        var price235 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption118.Id
        };
        var price236 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption118.Id
        };
        var price237 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption119.Id
        };
        var price238 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption119.Id
        };
        var price239 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption120.Id
        };
        var price240 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption120.Id
        };
        var price241 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption121.Id
        };
        var price242 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption121.Id
        };
        var price243 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption122.Id
        };
        var price244 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption122.Id
        };
        var price245 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption123.Id
        };
        var price246 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption123.Id
        };
        var price247 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption124.Id
        };
        var price248 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption124.Id
        };
        var price249 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption125.Id
        };
        var price250 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption125.Id
        };
        var price251 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption126.Id
        };
        var price252 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption126.Id
        };
        var price253 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption127.Id
        };
        var price254 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption127.Id
        };
        var price255 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption128.Id
        };
        var price256 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption128.Id
        };
        var price257 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption129.Id
        };
        var price258 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption129.Id
        };
        var price259 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption130.Id
        };
        var price260 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption130.Id
        };
        var price261 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption131.Id
        };
        var price262 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption131.Id
        };
        var price263 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption132.Id
        };
        var price264 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption132.Id
        };
        var price265 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption133.Id
        };
        var price266 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption133.Id
        };
        var price267 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption134.Id
        };
        var price268 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption134.Id
        };
        var price269 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption135.Id
        };
        var price270 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption135.Id
        };
        var price271 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption136.Id
        };
        var price272 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption136.Id
        };
        var price273 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption137.Id
        };
        var price274 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption137.Id
        };
        var price275 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption138.Id
        };
        var price276 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption138.Id
        };
        var price277 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption139.Id
        };
        var price278 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption139.Id
        };
        var price279 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption140.Id
        };
        var price280 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption140.Id
        };
        var price281 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption141.Id
        };
        var price282 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption141.Id
        };
        var price283 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption142.Id
        };
        var price284 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption142.Id
        };
        var price285 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption143.Id
        };
        var price286 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption143.Id
        };
        var price287 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption144.Id
        };
        var price288 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption144.Id
        };
        var price289 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption145.Id
        };
        var price290 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption145.Id
        };
        var price291 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption146.Id
        };
        var price292 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption146.Id
        };
        var price293 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption147.Id
        };
        var price294 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption147.Id
        };
        var price295 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption148.Id
        };
        var price296 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption148.Id
        };
        var price297 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption149.Id
        };
        var price298 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption149.Id
        };
        var price299 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption150.Id
        };
        var price300 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption150.Id
        };
        var price301 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption151.Id
        };
        var price302 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption151.Id
        };
        var price303 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption152.Id
        };
        var price304 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption152.Id
        };
        var price305 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption153.Id
        };
        var price306 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption153.Id
        };
        var price307 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption154.Id
        };
        var price308 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption154.Id
        };
        var price309 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption155.Id
        };
        var price310 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption155.Id
        };
        var price311 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption156.Id
        };
        var price312 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption156.Id
        };
        var price313 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption157.Id
        };
        var price314 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption157.Id
        };
        var price315 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption158.Id
        };
        var price316 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption158.Id
        };
        var price317 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption159.Id
        };
        var price318 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption159.Id
        };
        var price319 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption160.Id
        };
        var price320 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption160.Id
        };
        var price321 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption161.Id
        };
        var price322 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption161.Id
        };
        var price323 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption162.Id
        };
        var price324 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption162.Id
        };
        var price325 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption163.Id
        };
        var price326 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption163.Id
        };
        var price327 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption164.Id
        };
        var price328 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption164.Id
        };
        var price329 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption165.Id
        };
        var price330 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption165.Id
        };
        var price331 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption166.Id
        };
        var price332 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption166.Id
        };
        var price333 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption167.Id
        };
        var price334 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption167.Id
        };
        var price335 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption168.Id
        };
        var price336 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption168.Id
        };
        var price337 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption169.Id
        };
        var price338 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption169.Id
        };
        var price339 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption170.Id
        };
        var price340 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption170.Id
        };
        var price341 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption171.Id
        };
        var price342 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption171.Id
        };
        var price343 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption172.Id
        };
        var price344 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption172.Id
        };
        var price345 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption173.Id
        };
        var price346 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption173.Id
        };
        var price347 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption174.Id
        };
        var price348 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption174.Id
        };
        var price349 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption175.Id
        };
        var price350 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption175.Id
        };
        var price351 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption176.Id
        };
        var price352 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption176.Id
        };
        var price353 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption177.Id
        };
        var price354 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption177.Id
        };
        var price355 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption178.Id
        };
        var price356 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption178.Id
        };
        var price357 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption179.Id
        };
        var price358 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption179.Id
        };
        var price359 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption180.Id
        };
        var price360 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption180.Id
        };
        var price361 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption181.Id
        };
        var price362 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption181.Id
        };
        var price363 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption182.Id
        };
        var price364 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption182.Id
        };
        var price365 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption183.Id
        };
        var price366 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption183.Id
        };
        var price367 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption184.Id
        };
        var price368 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption184.Id
        };
        var price369 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption185.Id
        };
        var price370 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption185.Id
        };
        var price371 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption186.Id
        };
        var price372 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption186.Id
        };
        var price373 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption187.Id
        };
        var price374 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption187.Id
        };
        var price375 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption188.Id
        };
        var price376 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption188.Id
        };
        var price377 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption189.Id
        };
        var price378 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption189.Id
        };
        var price379 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption190.Id
        };
        var price380 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption190.Id
        };
        var price381 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption191.Id
        };
        var price382 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption191.Id
        };
        var price383 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption192.Id
        };
        var price384 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption192.Id
        };
        var price385 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption193.Id
        };
        var price386 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption193.Id
        };
        var price387 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption194.Id
        };
        var price388 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption194.Id
        };
        var price389 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption195.Id
        };
        var price390 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption195.Id
        };
        var price391 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption196.Id
        };
        var price392 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption196.Id
        };
        var price393 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption197.Id
        };
        var price394 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption197.Id
        };
        var price395 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption198.Id
        };
        var price396 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption198.Id
        };
        var price397 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption199.Id
        };
        var price398 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption199.Id
        };
        var price399 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption200.Id
        };
        var price400 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption200.Id
        };
        var price401 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption201.Id
        };
        var price402 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption201.Id
        };
        var price403 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption202.Id
        };
        var price404 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption202.Id
        };
        var price405 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption203.Id
        };
        var price406 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption203.Id
        };
        var price407 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption204.Id
        };
        var price408 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption204.Id
        };
        var price409 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption205.Id
        };
        var price410 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption205.Id
        };
        var price411 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption206.Id
        };
        var price412 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption206.Id
        };
        var price413 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption207.Id
        };
        var price414 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption207.Id
        };
        var price415 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption208.Id
        };
        var price416 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption208.Id
        };
        var price417 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption209.Id
        };
        var price418 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption209.Id
        };
        var price419 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption210.Id
        };
        var price420 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption210.Id
        };
        var price421 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption211.Id
        };
        var price422 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption211.Id
        };
        var price423 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption212.Id
        };
        var price424 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption212.Id
        };
        var price425 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption213.Id
        };
        var price426 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption213.Id
        };
        var price427 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption214.Id
        };
        var price428 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption214.Id
        };
        var price429 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption215.Id
        };
        var price430 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption215.Id
        };
        var price431 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption216.Id
        };
        var price432 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption216.Id
        };
        var price433 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption217.Id
        };
        var price434 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption217.Id
        };
        var price435 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption218.Id
        };
        var price436 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption218.Id
        };
        var price437 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption219.Id
        };
        var price438 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption219.Id
        };
        var price439 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption220.Id
        };
        var price440 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption220.Id
        };
        var price441 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption221.Id
        };
        var price442 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption221.Id
        };
        var price443 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption222.Id
        };
        var price444 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption222.Id
        };
        var price445 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption223.Id
        };
        var price446 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption223.Id
        };
        var price447 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption224.Id
        };
        var price448 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption224.Id
        };
        var price449 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption225.Id
        };
        var price450 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption225.Id
        };
        var price451 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption226.Id
        };
        var price452 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption226.Id
        };
        var price453 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption227.Id
        };
        var price454 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption227.Id
        };
        var price455 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption228.Id
        };
        var price456 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption228.Id
        };
        var price457 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption229.Id
        };
        var price458 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption229.Id
        };
        var price459 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption230.Id
        };
        var price460 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption230.Id
        };
        var price461 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption231.Id
        };
        var price462 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption231.Id
        };
        var price463 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption232.Id
        };
        var price464 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption232.Id
        };
        var price465 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption233.Id
        };
        var price466 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption233.Id
        };
        var price467 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption234.Id
        };
        var price468 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption234.Id
        };
        var price469 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption235.Id
        };
        var price470 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption235.Id
        };
        var price471 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption236.Id
        };
        var price472 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption236.Id
        };
        var price473 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption237.Id
        };
        var price474 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption237.Id
        };
        var price475 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption238.Id
        };
        var price476 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption238.Id
        };
        var price477 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption239.Id
        };
        var price478 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption239.Id
        };
        var price479 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption240.Id
        };
        var price480 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption240.Id
        };
        var price481 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption241.Id
        };
        var price482 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption241.Id
        };
        var price483 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption242.Id
        };
        var price484 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption242.Id
        };
        var price485 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption243.Id
        };
        var price486 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption243.Id
        };
        var price487 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption244.Id
        };
        var price488 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption244.Id
        };
        var price489 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption245.Id
        };
        var price490 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption245.Id
        };
        var price491 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption246.Id
        };
        var price492 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption246.Id
        };
        var price493 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption247.Id
        };
        var price494 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption247.Id
        };
        var price495 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption248.Id
        };
        var price496 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption248.Id
        };
        var price497 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption249.Id
        };
        var price498 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption249.Id
        };
        var price499 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption250.Id
        };
        var price500 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption250.Id
        };
        var price501 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption251.Id
        };
        var price502 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption251.Id
        };
        var price503 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 11000,
            ProductAttributeOptionId = productAttributeOption252.Id
        };
        var price504 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption252.Id
        };
        var price505 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 12000,
            ProductAttributeOptionId = productAttributeOption253.Id
        };
        var price506 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption253.Id
        };
        var price507 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 13000,
            ProductAttributeOptionId = productAttributeOption254.Id
        };
        var price508 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption254.Id
        };
        var price509 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 14000,
            ProductAttributeOptionId = productAttributeOption255.Id
        };
        var price510 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 1500,
            ProductAttributeOptionId = productAttributeOption255.Id
        };
        var price511 = new Price()
        {
            Date = DateTime.Now,
            MainPrice = 10000,
            ProductAttributeOptionId = productAttributeOption256.Id
        };
        var price512 = new Price()
        {
            Date = DateTime.Now + TimeSpan.FromDays(100),
            MainPrice = 15000,
            ProductAttributeOptionId = productAttributeOption256.Id
        };
        if (!_context.Prices.Any())
        {
            _context.Prices.AddRange(new List<Price>()
            {
                price1,price2,price3,price4,price5,price6,price7,price8,price9,price10,price11,price12,price13,price14,price15,price16,
                price17,price18,price19,price20,price21,price22,price23,price24,price25,price26,price27,price28,price29,price30,price31,price32,
                price33,price34,price35,price36,price37,price38,price39,price40,price41,price42,price43,price44,price45,price46,price47,price48,
                price49,price50,price51,price52,price53,price54,price55,price56,price57,price58,price59,price60,price61,price62,price63,price64,
                price65,price66,price67,price68,price69,price70,price71,price72,price73,price74,price75,price76,price77,price78,price79,price80,
                price81,price82,price83,price84,price85,price86,price87,price88,price89,price90,price91,price92,price93,price94,price95,price96,
                price97,price98,price99,price100,price101,price102,price103,price104,price105,price106,price107,price108,price109,price110,price111,price112,
                price113,price114,price115,price116,price117,price118,price119,price120,price121,price122,price123,price124,price125,price126,price127,price128,
                price129,price130,price131,price132,price133,price134,price135,price136,price137,price138,price139,price140,price141,price142,price143,price144,
                price145,price146,price147,price148,price149,price150,price151,price152,price153,price154,price155,price156,price157,price158,price159,price160,
                price161,price162,price163,price164,price165,price166,price167,price168,price169,price170,price171,price172,price173,price174,price175,price176,
                price177,price178,price179,price180,price181,price182,price183,price184,price185,price186,price187,price188,price189,price190,price191,price192,
                price193,price194,price195,price196,price197,price198,price199,price200,price201,price202,price203,price204,price205,price206,price207,price208,
                price209,price210,price211,price212,price213,price214,price215,price216,price217,price218,price219,price220,price221,price222,price223,price224,
                price225,price226,price227,price228,price229,price230,price231,price232,price233,price234,price235,price236,price237,price238,price239,price240,
                price241,price242,price243,price244,price245,price246,price247,price248,price249,price250,price251,price252,price253,price254,price255,price256,
                price256,price257,price258,price259,price260,price261,price262,price263,price264,price264,price265,price266,price267,price268,price269,price270,
                price271,price272,price273,price274,price275,price276,price277,price278,price279,price280,price281,price282,price283,price284,price285,price286,
                price287,price288,price289,price290,price291,price292,price293,price294,price295,price296,price297,price298,price299,price299,price300,price310,
                price311,price312,price313,price314,price315,price316,price317,price318,price319,price320,price321,price322,price323,price324,price325,price326,
                price327,price328,price329,price330,price331,price332,price333,price334,price335,price336,price337,price338,price339,price340,price341,price342,
                price343,price344,price345,price346,price347,price348,price349,price350,price351,price352,price353,price354,price355,price356,price357,price358,
                price359,price360,price361,price362,price363,price364,price365,price366,price367,price368,price369,price370,price371,price372,price373,price374,
                price375,price376,price377,price378,price379,price380,price381,price382,price383,price384,price385,price386,price387,price388,price389,price390,
                price391,price392,price393,price394,price395,price396,price397,price398,price399,price400,price401,price402,price403,price404,price405,price406,
                price407,price408,price409,price410,price411,price412,price413,price414,price415,price416,price417,price418,price419,price420,price421,price422,
                price423,price424,price425,price426,price427,price428,price429,price430,price431,price432,price433,price434,price435,price436,price437,price438,
                price439,price440,price441,price442,price443,price444,price445,price446,price447,price448,price449,price450,price451,price452,price453,price454,
                price455,price456,price457,price458,price459,price460,price461,price462,price463,price464,price465,price466,price467,price468,price469,price470,
                price471,price472,price473,price474,price475,price476,price477,price478,price479,price480,price481,price482,price483,price484,price485,price486,
                price487,price488,price489,price490,price491,price492,price493,price494,price495,price496,price497,price498,price499,price500,price501,price502,
                price503,price504,price505,price506,price507,price508,price509,
                price510,price511,price512
            });
        }

        #endregion
        await _context.SaveChangesAsync();
        #endregion

        #endregion
    }
}
