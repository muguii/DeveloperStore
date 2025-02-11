using Microsoft.EntityFrameworkCore;

namespace Sales.Application.Shared;

internal static class PagingExtensions
{
    internal static async Task<Paging<T>> ToPagedListAsync<T>(this IQueryable<T> query, PagingFilter pagingFilter, CancellationToken cancellationToken) where T : class
        => new Paging<T>(await query.AsNoTracking()
                                    .Skip(pagingFilter.Skip)
                                    .Take(pagingFilter.Size)
                                    .ToListAsync(cancellationToken).ConfigureAwait(false),
                         pagingFilter.Page,
                         pagingFilter.Size,
                         await query.CountAsync(cancellationToken).ConfigureAwait(false));
}