using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public static class PaginationQueryHelper
{
    public static IQueryable<Product> ApplySearch(this IQueryable<Product> query, string searchTerm)
    {
        query = query.Where(o => o.Code.ToString().Contains(searchTerm)
                                   || (o.ProductNo != null && o.ProductNo.Contains(searchTerm))
                                   || (o.OldProductNo != null && o.OldProductNo.Contains(searchTerm))
                                   || o.LocalizedName.Contains(searchTerm)
                                   || o.Name.Contains(searchTerm)
                                   || (o.ProductCatalog != null && o.ProductCatalog.Contains(searchTerm))
                                   || o.OrderRate.ToString().Contains(searchTerm)
                                   || (o.Mileage.ToString() != null && o.Mileage.ToString()!.Contains(searchTerm))
                                   || (o.Duration.ToString() != null && o.Duration.ToString()!.Contains(searchTerm))
                                   || o.Excerpt.Contains(searchTerm)
                                   || o.Description.Contains(searchTerm)
                                   || o.Slug.Contains(searchTerm)
                                   || (o.Sku != null && o.Sku.Contains(searchTerm))
                                   || (o.Provider != null && o.Provider.LocalizedName.Contains(searchTerm))
                                   || (o.Country != null && o.Country.LocalizedName.Contains(searchTerm))
                                   || (o.ProductType != null && o.ProductType.LocalizedName.Contains(searchTerm))
                                   || (o.ProductStatus != null && o.ProductStatus.LocalizedName.Contains(searchTerm))
                                   || (o.MainCountingUnit != null && o.MainCountingUnit.LocalizedName.Contains(searchTerm))
                                   || (o.CommonCountingUnit != null && o.CommonCountingUnit.LocalizedName.Contains(searchTerm))
                                   || o.ProductBrand.LocalizedName.Contains(searchTerm)
                                   || o.ProductCategory.LocalizedName.Contains(searchTerm)
                                   || o.ProductAttributeType.Name.Contains(searchTerm)
        );

        return query;
    }
    public static IQueryable<Product> ApplySorting(this IQueryable<Product> query, string sortColumn, string sortDirection)
    {
        switch (sortColumn)
        {
            case "localizedName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.LocalizedName) : query.OrderByDescending(o => o.LocalizedName);
                break;
            case "name":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                break;
            case "code":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Code) : query.OrderByDescending(o => o.Code);
                break;
            case "productNo":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductNo) : query.OrderByDescending(o => o.ProductNo);
                break;
            case "oldProductNo":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.OldProductNo) : query.OrderByDescending(o => o.OldProductNo);
                break;
            case "productCatalog":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductCatalog) : query.OrderByDescending(o => o.ProductCatalog);
                break;
            case "orderRate":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.OrderRate) : query.OrderByDescending(o => o.OrderRate);
                break;
            case "mileage":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Mileage) : query.OrderByDescending(o => o.Mileage);
                break;
            case "duration":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Duration) : query.OrderByDescending(o => o.Duration);
                break;
            case "excerpt":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Excerpt) : query.OrderByDescending(o => o.Excerpt);
                break;
            case "description":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Description) : query.OrderByDescending(o => o.Description);
                break;
            case "slug":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Slug) : query.OrderByDescending(o => o.Slug);
                break;
            case "sku":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Sku) : query.OrderByDescending(o => o.Sku);
                break;
            case "providerName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Provider != null ? o.Provider.LocalizedName : "") : query.OrderByDescending(o => o.Provider != null ? o.Provider.LocalizedName : "");
                break;
            case "countryName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.Country != null ? o.Country.LocalizedName : "") : query.OrderByDescending(o => o.Country != null ? o.Country.LocalizedName : "");
                break;
            case "productTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductType != null ? o.ProductType.LocalizedName : "") : query.OrderByDescending(o => o.ProductType != null ? o.ProductType.LocalizedName : "");
                break;
            case "productStatusName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductStatus != null ? o.ProductStatus.LocalizedName : "") : query.OrderByDescending(o => o.ProductStatus != null ? o.ProductStatus.LocalizedName : "");
                break;
            case "mainCountingUnitName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.MainCountingUnit != null ? o.MainCountingUnit.LocalizedName : "") : query.OrderByDescending(o => o.MainCountingUnit != null ? o.MainCountingUnit.LocalizedName : "");
                break;
            case "commonCountingUnitName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.CommonCountingUnit != null ? o.CommonCountingUnit.LocalizedName : "") : query.OrderByDescending(o => o.CommonCountingUnit != null ? o.CommonCountingUnit.LocalizedName : "");
                break;
            case "brandName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductBrand.LocalizedName) : query.OrderByDescending(o => o.ProductBrand.LocalizedName);
                break;
            case "productCategoryName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductCategory.LocalizedName) : query.OrderByDescending(o => o.ProductCategory.LocalizedName);
                break;
            case "productAttributeTypeName":
                query = sortDirection.ToLower() == "asc" ? query.OrderBy(o => o.ProductAttributeType.Name) : query.OrderByDescending(o => o.ProductAttributeType.Name);
                break;
            default:
                query = query.OrderBy(o => o.Created);
                break;
        }

        return query;
    }
}
