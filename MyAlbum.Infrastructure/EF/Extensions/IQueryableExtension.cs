using System;
namespace MyAlbum.Infrastructure.EF.Extensions
{
	public static class IQueryableExtension
	{
		public static IQueryable<TEntity> Pagination<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize)
		{
			if (pageIndex < 1)
			{
				pageIndex = 1;
            }
			if (pageSize < 10)
			{
				pageSize = 10;
			}
			var skipCount = pageIndex == 1 ? 0 : (pageIndex - 1) * pageSize;
			query = query.Skip(skipCount);
			query = query.Take(pageSize);
			return query;
        }
	}
}

