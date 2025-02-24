using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice;
public class ReturnOrderItemGroupDto : IMapFrom<ReturnOrderItemGroup>
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public double TotalDiscountPercent { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductNo { get; set; }
    public int ProductAttributeOptionId { get; set; }
    public int ReturnOrderId { get; set; }
    public double TotalQuantity { get; set; }

    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<ReturnOrderItemGroup, ReturnOrderItemGroupDto>()
    //        .ForMember(d => d.TotalQuantity,
    //            opt =>
    //                opt.MapFrom(s => s.ReturnOrderItems.Sum(c => c.Quantity)));
    //}
}
