using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries;
public class OrderPaymentDto : IMapFrom<OrderPayment>
{
    public int Id { get; set; }
    public PaymentType PaymentType { get; set; }
    public string PaymentTypeName => EnumHelper<PaymentType>.GetDisplayValue(PaymentType);

    public long? Amount { get; set; }


    //For online
    public PaymentServiceType? PaymentServiceType { get; set; }
    public string? PaymentServiceTypeName => EnumHelper<PaymentServiceType>.GetDisplayValue(PaymentServiceType);
    public OnlinePaymentStatus? Status { get; set; }
    public string? StatusName => EnumHelper<OnlinePaymentStatus>.GetDisplayValue(Status);
    public string? Authority { get; set; }
    public string? CardNumber { get; set; }
    public string? Rrn { get; set; }
    public string? RefId { get; set; }
    public string? PayGateTranId { get; set; }
    public long? SalesOrderId { get; set; }
    public int? ServiceTypeId { get; set; }
    public string? Error { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderPayment, OrderPaymentDto>()
            .Include<OnlinePayment, OrderPaymentDto>();

        profile.CreateMap<OnlinePayment, OrderPaymentDto>()
            .ForMember(dest => dest.PaymentServiceType, opt => opt.MapFrom(src => src.PaymentServiceType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Authority, opt => opt.MapFrom(src => src.Authority))
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
            .ForMember(dest => dest.Rrn, opt => opt.MapFrom(src => src.Rrn))
            .ForMember(dest => dest.RefId, opt => opt.MapFrom(src => src.RefId))
            .ForMember(dest => dest.PayGateTranId, opt => opt.MapFrom(src => src.PayGateTranId))
            .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId))
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => src.ServiceTypeId))
            .ForMember(dest => dest.Error, opt => opt.MapFrom(src => src.Error));
    }
    
}
