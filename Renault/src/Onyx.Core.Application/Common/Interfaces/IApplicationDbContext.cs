using Microsoft.EntityFrameworkCore;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.CustomerSupportCluster;
using Onyx.Domain.Entities.FilesCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.LayoutsCluster;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Common.Interfaces;

public interface IApplicationDbContext
{

    #region BlogsCluster

    DbSet<Comment> Comments { get; }
    DbSet<Post> Posts { get; }
    DbSet<WidgetComment> WidgetComments { get; }

    #endregion

    #region BrandsCluster

    DbSet<ProductBrand> ProductBrands { get; }
    DbSet<VehicleBrand> VehicleBrands { get; }
    DbSet<Family> Families { get; }
    DbSet<Kind> Kinds { get; }
    DbSet<Model> Models { get; }
    DbSet<Vehicle> Vehicles { get; }

    #endregion

    #region CategoriesCluster

    DbSet<BlogCategory> BlogCategories { get; }
    DbSet<BlogCategoryCustomField> BlogCategoryCustomFields { get; }
    DbSet<ProductCategory> ProductCategories { get; }
    DbSet<ProductCategoryCustomField> ProductCategoryCustomFields { get; }

    #endregion

    #region CustomerSupportCluster

    DbSet<CustomerTicket> CustomerTickets { get; }

    #endregion

    #region FilesCluster

    DbSet<StoredFile> StoredFiles { get; }

    #endregion

    #region InfoCluster

    DbSet<CorporationInfo> CorporationInfos { get; }
    DbSet<CostType> CostTypes { get; }
    DbSet<Country> Countries { get; }
    DbSet<Question> Questions { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<Testimonial> Testimonials { get; }
    DbSet<AboutUs> AboutUsEnumerable { get; }

    #endregion

    #region LayoutsCluster

    DbSet<FooterLink> FooterLinks { get; }
    DbSet<FooterLinkContainer> FooterLinkContainers { get; }
    DbSet<SocialLink> SocialLinks { get; }
    DbSet<Link> Links { get; }
    DbSet<BlockBanner> BlockBanners { get; }
    DbSet<Carousel> Carousels { get; }
    DbSet<Theme> Themes { get; }

    #endregion

    #region OrdersCluster

    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<OrderItemOption> OrderItemOptions { get; }
    DbSet<OrderTotal> OrderTotals { get; }
    DbSet<OrderStateBase> OrderStateBases { get; }
    DbSet<OrderPayment> OrderPayments { get; }

    #endregion

    #region ProductsCluster

    #region ProductAttributesCluster

    DbSet<ProductAttribute> ProductAttributes { get; }
    DbSet<ProductAttributeCustomField> ProductAttributeCustomFields { get; }
    DbSet<ProductAttributeType> ProductAttributeTypes { get; }
    DbSet<ProductAttributeValueCustomField> ProductAttributeValueCustomFields { get; }
    DbSet<ProductTypeAttributeGroup> ProductTypeAttributeGroups { get; }
    DbSet<ProductTypeAttributeGroupAttribute> ProductTypeAttributeGroupAttributes { get; }
    DbSet<ProductTypeAttributeGroupCustomField> ProductTypeAttributeGroupCustomFields { get; }

    #endregion

    #region ProductOptionsCluster

    #region Structure

    #region Color
    DbSet<ProductOptionColor> ProductOptionColors { get; }
    DbSet<ProductOptionValueColor> ProductOptionValueColors { get; }
    DbSet<ProductOptionColorCustomField> ProductOptionColorCustomFields { get; }
    #endregion
    #region Material
    DbSet<ProductOptionMaterial> ProductOptionMaterials { get; }
    DbSet<ProductOptionValueMaterial> ProductOptionValueMaterials { get; }
    DbSet<ProductOptionMaterialCustomField> ProductOptionMaterialCustomFields { get; }
    #endregion

    #endregion

    #region Value
    public DbSet<ProductAttributeOption> ProductAttributeOptions { get; }
    public DbSet<ProductAttributeOptionValue> ProductAttributeOptionValues { get; }
    public DbSet<ProductAttributeOptionRole> ProductAttributeOptionRoles { get; }
    #endregion

    #endregion

    DbSet<Badge> Badges { get; }
    DbSet<CountingUnit> CountingUnits { get; }
    DbSet<CountingUnitType> CountingUnitTypes { get; }
    DbSet<Price> Prices { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductCustomField> ProductCustomFields { get; }
    DbSet<ProductDisplayVariant> ProductDisplayVariants { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<ProductKind> ProductKinds { get; }
    DbSet<ProductStatus> ProductStatuses { get; }
    DbSet<ProductType> ProductTypes { get; }
    DbSet<Provider> Providers { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Tag> Tags { get; }

    #endregion

    #region RequestsCluster

    DbSet<RequestLog> RequestLogs { get; }

    #endregion

    #region ReturnOrdersCluster

    DbSet<ReturnOrder> ReturnOrders { get; }
    DbSet<ReturnOrderItem> ReturnOrderItems { get; }
    DbSet<ReturnOrderItemDocument> ReturnOrderItemDocuments { get; }
    DbSet<ReturnOrderItemGroup> ReturnOrderItemGroups { get; }
    DbSet<ReturnOrderItemGroupProductAttributeOptionValue> ReturnOrderItemGroupProductAttributeOptionValues { get; }
    DbSet<ReturnOrderReason> ReturnOrderReasons { get; }
    DbSet<ReturnOrderStateBase> ReturnOrderStateBases { get; }
    DbSet<ReturnOrderTotal> ReturnOrderTotals { get; }
    DbSet<ReturnOrderTotalDocument> ReturnOrderTotalDocuments { get; }


    #endregion

    #region UserProfilesCluster

    DbSet<Address> Addresses { get; }
    DbSet<Credit> Credits { get; }
    DbSet<MaxCredit> MaxCredits { get; }
    DbSet<Customer> Customers { get; }
    DbSet<User> Users { get; }
    DbSet<CustomerType> CustomerTypes { get; }

    #endregion
   

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
