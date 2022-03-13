using System.Collections.Generic;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.Client.Api.IntegrationTests.Utitlities
{
    public class JobPagedList
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public bool hasPrevious { get; set; }
        public bool hasNext { get; set; }
        public List<JobModel> items { get; set; }
    }
}
