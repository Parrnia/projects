using System.Net;
using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice;
public class RequestLogDto : IMapFrom<RequestLog>
{
    public int Id { get; set; }
    public string ApiAddress { get; set; } = string.Empty;
    public string? RequestBody { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; } = string.Empty;
    public HttpStatusCode ResponseStatus { get; set; }
    public string HttpStatusCodeName => ResponseStatus.ToString();
    public string Created { get; set; } = string.Empty;
    public RequestType RequestType { get; set; }
    public string RequestTypeName => EnumHelper<RequestType>.GetDisplayValue(RequestType);
    public ApiType ApiType { get; set; }
    public string ApiTypeName => EnumHelper<ApiType>.GetDisplayValue(ApiType);

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RequestLog, RequestLogDto>()
            .ForMember(d => d.Created,
            opt =>
                opt.MapFrom(s => s.Created.ToPersianDate()));
    }
}