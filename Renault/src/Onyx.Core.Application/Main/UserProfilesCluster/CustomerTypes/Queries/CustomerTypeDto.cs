using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries;
public class CustomerTypeDto : IMapFrom<CustomerType>
{ 
    public int Id { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public string CustomerTypeEnumName => EnumHelper<CustomerTypeEnum>.GetDisplayValue(CustomerTypeEnum);
    public double DiscountPercent { get; set; }
}
