using System.Collections.Generic;

namespace Rosterd.Domain.Models
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int totalCount, int currentPage, int pageSize, int totalPages)
        {
            TotalTotalCount = totalCount;
            PageSize = pageSize;
            CurrentCurrentPage = currentPage;
            TotalPages = totalPages;
            Items = items;
        }

        public int CurrentCurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalTotalCount { get; }

        public bool HasPrevious => CurrentCurrentPage > 1;
        public bool HasNext => CurrentCurrentPage < TotalPages;

        public List<T> Items { get; }
    }
}
