using Rosterd.Domain.Models.Resources;

namespace Rosterd.Admin.Api.Requests.Job
{
    public class AddUpdateJobRequest
    {
        
       public JobModel JobToAddOrUpdate { get; set; }
        
    }
}

