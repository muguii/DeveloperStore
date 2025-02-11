using Sales.Domain.Abstractions;

namespace Sales.Application.Shared;

internal static class QueryUtils
{
    internal static IEnumerable<string> ConvertToSqlLikePattern(List<string> filters)
    {
        return filters.Select(ConvertToSqlLikePattern);
    }

    internal static string RemoveWrongOrderByProperties<TEntity>(string? requestedOrderBy) where TEntity : BaseEntity
    {
        if (string.IsNullOrWhiteSpace(requestedOrderBy))
            return nameof(BaseEntity.Id);

        var entityProperties = typeof(TEntity).GetProperties()
                                              .Select(p => p.Name)
                                              .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var validOrderBy = requestedOrderBy.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(orderByParam => orderByParam.Trim())
                                           .Where(orderByParam => entityProperties.Contains(orderByParam.Split(' ')[0]))
                                           .ToList();

        return validOrderBy.Count > 0 ? string.Join(",", validOrderBy) : nameof(BaseEntity.Id);
    }

    private static string ConvertToSqlLikePattern(string filter)
    {
        if (filter.StartsWith("*") && filter.EndsWith("*"))
            return $"%{filter.Trim('*')}%";

        if (filter.StartsWith("*"))
            return $"%{filter.TrimStart('*')}";

        if (filter.EndsWith("*"))
            return $"{filter.TrimEnd('*')}%";

        return filter;
    }
}