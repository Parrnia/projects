using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;

public class SharedProductQueryHelperMethods : ISharedProductQueryHelperMethods
{
    private readonly IApplicationDbContext _context;

    public SharedProductQueryHelperMethods(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
    {
        var query = await _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants,
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            )
            .ToListAsync(cancellationToken);


        return MapNames(query);
    }

    public IQueryable<ProductModelHelper> GetProducts(Expression<Func<ProductDisplayVariant, bool>> displayVariantSelector)
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants.Where(c => c.IsActive).Where(displayVariantSelector),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .Where(result => result.ProductDisplayGroup.Any())
            .SelectMany(
                result => result.ProductDisplayGroup,
                (result, displayVariant) => new ProductModelHelper()
                {
                    Product = result.Product,
                    DisplayVariantName = displayVariant.Name
                }
            );

        return query;

    }

    public IQueryable<ProductModelHelper> GetBestSellerProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsBestSeller && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetFeaturedProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsFeatured && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetLatestProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsLatest && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetNewProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsNew && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetPopularProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsPopular && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetSaleProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsSale && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetSpecialOfferProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsSpecialOffer && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public IQueryable<ProductModelHelper> GetTopRatedProducts()
    {
        var query = _context.Products
            .Where(c => c.IsActive)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name)
            .GroupJoin(
                _context.ProductDisplayVariants
                    .Where(c => c.IsTopRated && c.IsActive),
                product => product.Id,
                displayVariant => displayVariant.ProductId,
                (product, productDisplayGroup) => new { Product = product, ProductDisplayGroup = productDisplayGroup }
            )
            .SelectMany(
                result => result.ProductDisplayGroup.DefaultIfEmpty(),
                (result, displayVariant) => new ProductModelHelper() { Product = result.Product, DisplayVariantName = displayVariant != null ? displayVariant.Name : null }
            );


        return query;
    }

    public List<Product> MapNames(List<ProductModelHelper> list)
    {
        foreach (var item in list)
        {
            if (!string.IsNullOrEmpty(item.DisplayVariantName))
            {
                item.Product.LocalizedName = item.DisplayVariantName;
            }
        }

        return list.Select(c => c.Product).ToList();
    }

    public List<TProduct> MapNames<T, TProduct>(
        List<T> list,
        Func<T, string> getDisplayVariantName,
        Func<T, TProduct> getProduct,
        Action<TProduct, string> setLocalizedName)
    {
        foreach (var item in list)
        {
            var displayVariantName = getDisplayVariantName(item);
            if (!string.IsNullOrEmpty(displayVariantName))
            {
                var product = getProduct(item);
                setLocalizedName(product, displayVariantName);
            }
        }

        return list.Select(getProduct).ToList();
    }

    public List<TProduct> FilterItemsForRole<TProduct, TAttributeOption, TOptionRole>(
        List<TProduct> products,
        Func<TProduct, IEnumerable<TAttributeOption>> attributeSelector,
        Func<TAttributeOption, IEnumerable<TOptionRole>> roleSelector,
        Func<TOptionRole, bool> rolePredicate,
        Action<TAttributeOption, IEnumerable<TOptionRole>> updateRoles)
    {
        foreach (var attributeOption in products.SelectMany(attributeSelector))
        {
            var filteredRoles = roleSelector(attributeOption).Where(rolePredicate).ToList();
            updateRoles(attributeOption, filteredRoles);
        }

        return products;
    }

    public List<TProduct> FindAndSetSelectedAttributeOption<TProduct, TAttributeOption, TOptionRole>(
        List<TProduct> products,
        Func<TProduct, IEnumerable<TAttributeOption>> attributeSelector,
        Func<TAttributeOption, IEnumerable<TOptionRole>> roleSelector,
        Func<TOptionRole, bool> rolePredicate,
        Func<TOptionRole, bool> availabilityPredicate,
        Func<TAttributeOption, bool> isDefaultPredicate,
        Action<TProduct, TAttributeOption?> setSelectedOption)
    {
        foreach (var product in products)
        {
            TAttributeOption? selectedOption = default;

            foreach (var option in attributeSelector(product))
            {
                var optionForAvailability = roleSelector(option).SingleOrDefault(rolePredicate);
                if (optionForAvailability != null && isDefaultPredicate(option) && availabilityPredicate(optionForAvailability))
                {
                    selectedOption = option;
                    break;
                }
            }


            foreach (var option in attributeSelector(product))
            {
                var optionForAvailability = roleSelector(option).SingleOrDefault(rolePredicate);
                if (optionForAvailability != null && availabilityPredicate(optionForAvailability))
                {
                    selectedOption = option;
                    break;
                }
            }


            if (selectedOption == null && attributeSelector(product).Any())
            {
                selectedOption = attributeSelector(product).First();
            }

            setSelectedOption(product, selectedOption);
        }
        return products;
    }
}