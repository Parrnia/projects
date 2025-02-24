namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice;
public class FilteredRequestLogDto
{
    public List<RequestLogDto> RequestLogs { get; set; } = new List<RequestLogDto>();
    public int Count { get; set; }
}
