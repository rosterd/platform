using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rosterd.Data.SqlServer.Helpers
{
    public class PagingList<T> : List<T>
    {
        public PagingList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;


        public static async Task<PagingList<T>> ToPagingList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //TODO: If performance becomes a concern then we can get rid of this count here 
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingList<T>(items, count, pageNumber, pageSize);
        }
    }
}
