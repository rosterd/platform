using Rosterd.Domain.Models.JobModels;

namespace Rosterd.Admin.Api.Requests.Job
{
    public class AddUpdateJobRequest
    {
        public JobModel JobToAddOrUpdate { get; set; }
    }
}

