using System.Linq.Expressions;
using Onyx.Domain.Interfaces;

namespace Onyx.Application.Common.Interfaces;
public interface IExportServices
{
    public byte[] ExportExcel<TSource>(List<TSource> source,
        List<Expression<Func<TSource, object>>?> selectedProperties) where TSource : class, IChangeDateAware;

    public Task<List<TSource>> Export<TSource>(
        IQueryable<TSource> source,
        List<Expression<Func<TSource, object>>?> selectedProperties,
        string? searchText,
        int? pageNumber,
        int? pageSize,
        DateTime? startCreationDate,
        DateTime? endCreationDate,
        DateTime? startChangeDate,
        DateTime? endChangeDate,
        CancellationToken cancellationToken) where TSource : class, IChangeDateAware;
}
