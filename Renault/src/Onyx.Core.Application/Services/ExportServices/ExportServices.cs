using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;
using Onyx.Domain.Interfaces;

namespace Onyx.Application.Services.ExportServices;
public class ExportServices : IExportServices
{
    public byte[] ExportExcel<TSource>(List<TSource> source,
        List<Expression<Func<TSource, object>>?> selectedProperties) where TSource : class, IChangeDateAware
    {
        var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
        var xmlFilePath = Path.Combine(assemblyPath, "Onyx.Domain.xml");
        var xmlDoc = XDocument.Load(xmlFilePath);

        var columnInfos = new List<ExcelColumnInfo>();

        foreach (var property in selectedProperties)
        {
            var memberExpression = GetMemberExpression(property.Body);

            if (memberExpression != null)
            {
                var propertyType = ((PropertyInfo)memberExpression.Member).PropertyType;

                var xmlPath = $"P:{memberExpression.Member.DeclaringType?.FullName}.{memberExpression.Member.Name}";
                var summary = GetXmlSummary(xmlDoc, xmlPath);
                var columnInfo = new ExcelColumnInfo
                {
                    Name = summary,
                    PropertyName = memberExpression.Member.Name,
                    ColumnWidth = 10,
                    Formatter = d => Format(d, propertyType)
                };

                columnInfos.Add(columnInfo);
            }
        }


        var exporter = new ExcelExporter();
        var data = exporter.Create(source, columnInfos, "sheet1");

        return data;
    }

    public async Task<List<TSource>> Export<TSource>(
    IQueryable<TSource> source,
    List<Expression<Func<TSource, object>>?> selectedProperties,
    string? searchText,
    int? pageNumber,
    int? pageSize,
    DateTime? startCreationDate,
    DateTime? endCreationDate,
    DateTime? startChangeDate,
    DateTime? endChangeDate,
    CancellationToken cancellationToken) where TSource : class, IChangeDateAware
    {
        // Include all navigation properties in the query
        foreach (var property in selectedProperties)
        {
            if (property != null)
            {
                var memberExpression = GetMemberExpression(property.Body);
                if (memberExpression != null && IsNavigationProperty(typeof(TSource), memberExpression))
                {
                    // Include navigation property in the query
                    source = source.Include(GetIncludePath(memberExpression));
                }
            }
        }

        // Search text filtering
        if (!string.IsNullOrEmpty(searchText))
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression searchExpression = Expression.Constant(false);

            foreach (var property in selectedProperties)
            {
                if (property != null)
                {
                    var memberExpression = GetMemberExpression(property.Body);

                    if (memberExpression != null)
                    {
                        // Handle navigation properties (deep property access)
                        Expression propertyExpression = BuildNestedPropertyExpression(parameter, memberExpression);

                        // Generate the search string condition
                        var toStringCall = Expression.Call(propertyExpression, "ToString", null);
                        var containsCall = Expression.Call(toStringCall, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(searchText));

                        searchExpression = Expression.OrElse(searchExpression, containsCall);
                    }
                }
            }

            var lambda = Expression.Lambda<Func<TSource, bool>>(searchExpression, parameter);
            source = source.Where(lambda);
        }

        if (startCreationDate != null)
        {
            source = source.Where(i => i.Created >= startCreationDate);
        }
        if (endCreationDate != null)
        {
            source = source.Where(i => i.Created <= endCreationDate);
        }
        if (startChangeDate != null)
        {
            source = source.Where(i => i.LastModified >= startChangeDate);
        }
        if (endChangeDate != null)
        {
            source = source.Where(i => i.LastModified <= endChangeDate);
        }

        // Pagination
        if (pageNumber != null && pageSize != null)
        {
            source = source.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize);
        }

        var finalSource = await source.ToListAsync(cancellationToken);

        return finalSource;
    }

    // Helper method to build a nested property expression (for navigation properties)
    private Expression BuildNestedPropertyExpression(ParameterExpression parameter, MemberExpression memberExpression)
    {
        // If it's a simple property, return it
        if (memberExpression.Expression is ParameterExpression)
            return Expression.Property(parameter, memberExpression.Member.Name);

        // If it's a nested property, recursively build the expression
        var innerExpression = memberExpression.Expression as MemberExpression;
        var innerProperty = BuildNestedPropertyExpression(parameter, innerExpression);

        return Expression.Property(innerProperty, memberExpression.Member.Name);
    }

    // Helper method to get the MemberExpression, handling conversions/unary expressions
    private MemberExpression? GetMemberExpression(Expression expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            return memberExpression;
        }
        if (expression is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression)
        {
            return (MemberExpression)unaryExpression.Operand;
        }
        return null;
    }

    // Helper method to check if a member is a navigation property
    private bool IsNavigationProperty(Type type, MemberExpression memberExpression)
    {
        var propertyInfo = type.GetProperty(memberExpression.Member.Name);
        return propertyInfo != null && !propertyInfo.PropertyType.IsPrimitive && !propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType != typeof(string);
    }

    // Helper method to build the Include path for navigation properties
    private string GetIncludePath(MemberExpression memberExpression)
    {
        var memberNames = new Stack<string>();
        var currentExpression = memberExpression;

        // Collect member names up the chain
        while (currentExpression != null)
        {
            memberNames.Push(currentExpression.Member.Name);
            currentExpression = currentExpression.Expression as MemberExpression;
        }

        return string.Join(".", memberNames);
    }




    private static string GetXmlSummary(XDocument xmlDoc, string xmlPath)
    {
        XElement? member = xmlDoc.Descendants("member")
            .FirstOrDefault(m => m.Attribute("name")?.Value == xmlPath);

        return member?.Element("summary")?.Value.Trim() ?? "بدون نام";
    }

    private static string? Format(Object? o, Type type)
    {
        if (o == null) return null;

        if (type == typeof(int))
        {
            return ((int)o).ToString("N0");
        }

        if (type == typeof(string))
        {
            return o.ToString();
        }

        if (type == typeof(DateTime))
        {
            return ((DateTime)o).ToPersianDate();
        }

        if (type == typeof(Address))
        {
            return o is Address adr ? $"{adr.Title}, {adr.City}, {adr.AddressDetails1}, {adr.AddressDetails1}" : "-";
        }

        if (type == typeof(Country))
        {
            return o is Country adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductBrand))
        {
            return o is ProductBrand adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(VehicleBrand))
        {
            return o is VehicleBrand adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(Model))
        {
            return o is Model adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(Family))
        {
            return o is Family adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(Kind))
        {
            return o is Kind adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductCategory))
        {
            return o is ProductCategory adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(List<string>))
        {
            return o is List<string> adr ? $"{String.Join(" ,", adr)}" : "-";
        }

        if (type == typeof(AboutUs))
        {
            return o is AboutUs adr ? $"{adr.Title}" : "-";
        }

        if (type.IsEnum)
        {
            return o is Enum adr ? GetEnumDisplayValue(adr) : "-";
        }

        if (type == typeof(FooterLinkContainer))
        {
            return o is FooterLinkContainer adr ? $"{adr.Header}" : "-";
        }

        if (type == typeof(OrderItem))
        {
            return o is OrderItem adr ? $"{adr.ProductAttributeOptionId}" : "-";
        }

        if (type == typeof(List<OrderStateBase>))
        {
            return o is List<OrderStateBase> adr ? GetEnumDisplayValue(adr.OrderBy(e => e.Created).Last().OrderStatus) : "-";
        }

        if (type == typeof(List<OrderItem>))
        {
            return o is List<OrderItem> adr ? $"{String.Join(" ,", adr.Select(c => c.ProductLocalizedName))}" : "-";
        }

        if (type == typeof(List<OrderTotal>))
        {
            return o is List<OrderTotal> adr ? $"{String.Join(" ,", adr.Select(c => c.Title))}" : "-";
        }

        if (type == typeof(CountingUnitType))
        {
            return o is CountingUnitType adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(Product))
        {
            return o is Product adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductTypeAttributeGroup))
        {
            return o is ProductTypeAttributeGroup adr ? $"{adr.Name}" : "-";
        }

        if (type == typeof(ProductOptionColor))
        {
            return o is ProductOptionColor adr ? $"{adr.Name}" : "-";
        }

        if (type == typeof(ProductOptionMaterial))
        {
            return o is ProductOptionMaterial adr ? $"{adr.Name}" : "-";
        }

        if (type == typeof(List<Price>))
        {
            return o is List<Price> adr ? $"{String.Join(" ,", adr.Select(c => c.MainPrice))}" : "-";
        }

        if (type == typeof(List<Badge>))
        {
            return o is List<Badge> adr ? $"{String.Join(" ,", adr.Select(c => c.Value))}" : "-";
        }

        if (type == typeof(List<ProductAttributeOptionValue>))
        {
            return o is List<ProductAttributeOptionValue> adr ? $"{String.Join(" ,", adr.Select(c => c.Value))}" : "-";
        }

        if (type == typeof(Provider))
        {
            return o is Provider adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(Country))
        {
            return o is Country adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductStatus))
        {
            return o is ProductStatus adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductType))
        {
            return o is ProductType adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(CountingUnit))
        {
            return o is CountingUnit adr ? $"{adr.LocalizedName}" : "-";
        }

        if (type == typeof(ProductAttributeType))
        {
            return o is ProductAttributeType adr ? $"{adr.Name}" : "-";
        }

        if (type == typeof(Tag))
        {
            return o is Tag adr ? $"{adr.FaTitle}" : "-";
        }

        if (type == typeof(CostRefundType))
        {
            return o is CostRefundType adr ? GetEnumDisplayValue(adr): "-";
        }

        if (type == typeof(List<ReturnOrderStateBase>))
        {
            return o is List<ReturnOrderStateBase> adr ? GetEnumDisplayValue(adr.OrderBy(e => e.Created).Last().ReturnOrderStatus) : "-";
        }

        if (type == typeof(ReturnOrderTransportationType))
        {
            return o is ReturnOrderTransportationType adr ? GetEnumDisplayValue(adr) : "-";
        }

        if (type == typeof(List<ReturnOrderTotal>))
        {
            return o is List<ReturnOrderTotal> adr ? $"{String.Join(" ,", adr.Select(c => c.Title))}" : "-";
        }

        if (type == typeof(Order))
        {
            return o is Order adr ? $"{adr.Number}" : "-";
        }

        if (type == typeof(ReturnOrderItemGroup))
        {
            return o is ReturnOrderItemGroup adr ? $"{adr.ProductLocalizedName}" : "-";
        }

        if (type == typeof(ReturnOrderReason))
        {
            return o is ReturnOrderReason adr ? adr.Details + "," + GetEnumDisplayValue(adr.ReturnOrderReasonType) + "," + (adr.OrganizationType != null ? GetEnumDisplayValue(adr.OrganizationType) : "") + (adr.CustomerType != null ? GetEnumDisplayValue(adr.CustomerType) : "") : "-";
        }

        if (type == typeof(ReturnOrder))
        {
            return o is ReturnOrder adr ? $"{adr.Number}" : "-";
        }

        if (type == typeof(ProductAttributeOption))
        {
            return o is ProductAttributeOption adr ? $"{adr.Id}" : "-";
        }

        if (type == typeof(ReturnOrderTotalType))
        {
            return o is ReturnOrderTotalType adr ? GetEnumDisplayValue(adr) : "-";
        }

        if (type == typeof(ReturnOrderItem))
        {
            return o is ReturnOrderItem adr ? $"{adr.Id}" : "-";
        }

        if (type == typeof(OrderPaymentType))
        {
            return o is OrderPaymentType adr ? GetEnumDisplayValue(adr) : "-";
        }

        return o.ToString();
    }

    private static string GetEnumDisplayValue(Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.GetName() ?? enumValue.ToString();
    }
}
