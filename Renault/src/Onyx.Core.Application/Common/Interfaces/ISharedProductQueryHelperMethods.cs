using System.Linq.Expressions;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Common.Interfaces;

public interface ISharedProductQueryHelperMethods
{
    Task<List<Product>> GetProducts(CancellationToken cancellationToken);
    IQueryable<ProductModelHelper> GetProducts(Expression<Func<ProductDisplayVariant, bool>> displayVariantSelector);
    IQueryable<ProductModelHelper> GetBestSellerProducts();
    IQueryable<ProductModelHelper> GetFeaturedProducts();
    IQueryable<ProductModelHelper> GetLatestProducts();
    IQueryable<ProductModelHelper> GetNewProducts();
    IQueryable<ProductModelHelper> GetPopularProducts();
    IQueryable<ProductModelHelper> GetSaleProducts();
    IQueryable<ProductModelHelper> GetSpecialOfferProducts();
    IQueryable<ProductModelHelper> GetTopRatedProducts();
    List<Product> MapNames(List<ProductModelHelper> list);

    List<TProduct> MapNames<T, TProduct>(
        List<T> list,
        Func<T, string> getDisplayVariantName,
        Func<T, TProduct> getProduct,
        Action<TProduct, string> setLocalizedName);

    List<TProduct> FilterItemsForRole<TProduct, TAttributeOption, TOptionRole>(
        List<TProduct> products,
        Func<TProduct, IEnumerable<TAttributeOption>> attributeSelector,
        Func<TAttributeOption, IEnumerable<TOptionRole>> roleSelector,
        Func<TOptionRole, bool> rolePredicate,
        Action<TAttributeOption, IEnumerable<TOptionRole>> updateRoles);

    List<TProduct> FindAndSetSelectedAttributeOption<TProduct, TAttributeOption, TOptionRole>(
        List<TProduct> products,
        Func<TProduct, IEnumerable<TAttributeOption>> attributeSelector,
        Func<TAttributeOption, IEnumerable<TOptionRole>> roleSelector,
        Func<TOptionRole, bool> rolePredicate,
        Func<TOptionRole, bool> availabilityPredicate,
        Func<TAttributeOption, bool> isDefaultPredicate,
        Action<TProduct, TAttributeOption?> setSelectedOption);
}
