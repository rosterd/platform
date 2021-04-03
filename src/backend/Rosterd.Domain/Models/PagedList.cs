using System.Collections.Generic;

namespace Rosterd.Domain.Models
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int totalCount, int currentPage, int pageSize, int totalPages)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            Items = items;
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Items { get; }
    }
}
