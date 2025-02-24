using System.Reflection;
using Onyx.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Infrastructure.Common;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.CustomerSupportCluster;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Entities.FilesCluster;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;

namespace Onyx.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        ChangeTracker.LazyLoadingEnabled = false;
    }

    #region BlogsCluster

    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<WidgetComment> WidgetComments => Set<WidgetComment>();

    #endregion

    #region BrandsCluster

    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
    public DbSet<VehicleBrand> VehicleBrands => Set<VehicleBrand>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<Kind> Kinds => Set<Kind>();
    public DbSet<Model> Models => Set<Model>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    #endregion

    #region CategoriesCluster

    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogCategoryCustomField> BlogCategoryCustomFields => Set<BlogCategoryCustomField>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductCategoryCustomField> ProductCategoryCustomFields => Set<ProductCategoryCustomField>();

    #endregion

    #region CustomerSupportCluster

    public DbSet<CustomerTicket> CustomerTickets => Set<CustomerTicket>();

    #endregion

    #region FilesCluster

    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();

    #endregion

    #region InfoCluster

    public DbSet<CorporationInfo> CorporationInfos => Set<CorporationInfo>();
    public DbSet<CostType> CostTypes => Set<CostType>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<AboutUs> AboutUsEnumerable => Set<AboutUs>();

    #endregion

    #region LayoutsCluster

    public DbSet<FooterLink> FooterLinks => Set<FooterLink>();
    public DbSet<FooterLinkContainer> FooterLinkContainers => Set<FooterLinkContainer>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<Link> Links => Set<Link>();
    public DbSet<BlockBanner> BlockBanners => Set<BlockBanner>();
    public DbSet<Carousel> Carousels => Set<Carousel>();
    public DbSet<Theme> Themes => Set<Theme>();


    #endregion

    #region OrdersCluster

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderItemOption> OrderItemOptions => Set<OrderItemOption>();
    public DbSet<OrderTotal> OrderTotals => Set<OrderTotal>();
    public DbSet<OrderStateBase> OrderStateBases => Set<OrderStateBase>();
    public DbSet<OrderPayment> OrderPayments => Set<OrderPayment>();

    #endregion

    #region ProductsCluster

    #region ProductAttributesCluster

    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
    public DbSet<ProductAttributeCustomField> ProductAttributeCustomFields => Set<ProductAttributeCustomField>();
    public DbSet<ProductAttributeType> ProductAttributeTypes => Set<ProductAttributeType>();
    public DbSet<ProductAttributeValueCustomField> ProductAttributeValueCustomFields => Set<ProductAttributeValueCustomField>();
    public DbSet<ProductTypeAttributeGroup> ProductTypeAttributeGroups => Set<ProductTypeAttributeGroup>();
    public DbSet<ProductTypeAttributeGroupAttribute> ProductTypeAttributeGroupAttributes => Set<ProductTypeAttributeGroupAttribute>();
    public DbSet<ProductTypeAttributeGroupCustomField> ProductTypeAttributeGroupCustomFields => Set<ProductTypeAttributeGroupCustomField>();

    #endregion

    #region ProductOptionsCluster

    #region Structure

    #region Color
    public DbSet<ProductOptionColor> ProductOptionColors => Set<ProductOptionColor>();
    public DbSet<ProductOptionValueColor> ProductOptionValueColors => Set<ProductOptionValueColor>();
    public DbSet<ProductOptionColorCustomField> ProductOptionColorCustomFields => Set<ProductOptionColorCustomField>();
    #endregion
    #region Material
    public DbSet<ProductOptionMaterial> ProductOptionMaterials => Set<ProductOptionMaterial>();
    public DbSet<ProductOptionValueMaterial> ProductOptionValueMaterials => Set<ProductOptionValueMaterial>();
    public DbSet<ProductOptionMaterialCustomField> ProductOptionMaterialCustomFields => Set<ProductOptionMaterialCustomField>();
    #endregion

    #endregion

    #region Value
    public DbSet<ProductAttributeOption> ProductAttributeOptions => Set<ProductAttributeOption>();
    public DbSet<ProductAttributeOptionValue> ProductAttributeOptionValues => Set<ProductAttributeOptionValue>();
    public DbSet<ProductAttributeOptionRole> ProductAttributeOptionRoles => Set<ProductAttributeOptionRole>();

    #endregion

    #endregion

    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<CountingUnit> CountingUnits => Set<CountingUnit>();
    public DbSet<CountingUnitType> CountingUnitTypes => Set<CountingUnitType>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCustomField> ProductCustomFields => Set<ProductCustomField>();
    public DbSet<ProductDisplayVariant> ProductDisplayVariants => Set<ProductDisplayVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductKind> ProductKinds => Set<ProductKind>();
    public DbSet<ProductStatus> ProductStatuses => Set<ProductStatus>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Tag> Tags => Set<Tag>();

    #endregion

    #region RequestsCluster

    public DbSet<RequestLog> RequestLogs => Set<RequestLog>();

    #endregion

    #region ReturnOrdersCluster

    public DbSet<ReturnOrder> ReturnOrders => Set<ReturnOrder>();
    public DbSet<ReturnOrderItem> ReturnOrderItems => Set<ReturnOrderItem>();
    public DbSet<ReturnOrderItemDocument> ReturnOrderItemDocuments => Set<ReturnOrderItemDocument>();
    public DbSet<ReturnOrderItemGroup> ReturnOrderItemGroups => Set<ReturnOrderItemGroup>();
    public DbSet<ReturnOrderItemGroupProductAttributeOptionValue> ReturnOrderItemGroupProductAttributeOptionValues => Set<ReturnOrderItemGroupProductAttributeOptionValue>();
    public DbSet<ReturnOrderReason> ReturnOrderReasons => Set<ReturnOrderReason>();
    public DbSet<ReturnOrderStateBase> ReturnOrderStateBases => Set<ReturnOrderStateBase>();
    public DbSet<ReturnOrderTotal> ReturnOrderTotals => Set<ReturnOrderTotal>();
    public DbSet<ReturnOrderTotalDocument> ReturnOrderTotalDocuments => Set<ReturnOrderTotalDocument>();

    #endregion

    #region UserProfilesCluster

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Credit> Credits => Set<Credit>();
    public DbSet<MaxCredit> MaxCredits => Set<MaxCredit>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CustomerType> CustomerTypes => Set<CustomerType>();

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        //optionsBuilder.EnableSensitiveDataLogging();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
