using System.Net;
using Onyx.Domain.Interfaces;

namespace Onyx.Domain.Entities.RequestsCluster;
public class RequestLog : IChangeDateAware
{
    public int Id { get; set; }
    /// <summary>
    /// آدرس api
    /// </summary>
    public string ApiAddress { get; set; } = string.Empty;
    /// <summary>
    /// بدنه درخواست
    /// </summary>
    public string? RequestBody { get; set; } = string.Empty;
    /// <summary>
    /// پیام خطا
    /// </summary>
    public string? ErrorMessage { get; set; } = string.Empty;
    /// <summary>
    /// نتیجه درخواست
    /// </summary>
    public HttpStatusCode ResponseStatus { get; set; }
    /// <summary>
    /// زمان زدن درخواست
    /// </summary>
    public DateTime Created { get; set; }
    /// <summary>
    /// نوع درخواست
    /// </summary>
    public RequestType RequestType { get; set; }
    /// <summary>
    /// نوع api
    /// </summary>
    public ApiType ApiType { get; set; }
    public DateTime? LastModified { get; set; }

}