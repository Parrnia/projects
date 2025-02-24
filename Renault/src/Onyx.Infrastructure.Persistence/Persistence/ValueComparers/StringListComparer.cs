using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Onyx.Infrastructure.Persistence.ValueComparers;
public class StringListComparer : ValueComparer<List<string>>
{
    private static readonly Expression<Func<List<string>?, List<string>?, bool>> CustomEqualsExpression =
        (c1, c2) => c1 == c2 || (c1 != null && c2 != null && c1.Order().SequenceEqual(c2.Order()));

    private static readonly Expression<Func<List<string>, int>> CustomHasCodeExpression =
        (c) => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()));

    private static readonly Expression<Func<List<string>, List<string>>> CustomSnapshotExpression =
        (c) => c.ToList();


    private StringListComparer() : base(CustomEqualsExpression, CustomHasCodeExpression, CustomSnapshotExpression)
    {
    }

    public static StringListComparer Instance { get; } = new();
}
