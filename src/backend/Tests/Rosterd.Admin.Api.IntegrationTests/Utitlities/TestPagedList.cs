using System.Collections.Generic;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.Admin.Api.IntegrationTests.Utitlities
{
    public class TestPagedList<T>
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public bool hasPrevious { get; set; }
        public bool hasNext { get; set; }
        public List<T> items { get; set; }
    }
}
