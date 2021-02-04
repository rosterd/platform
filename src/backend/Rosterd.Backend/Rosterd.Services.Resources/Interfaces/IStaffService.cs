using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;

namespace Rosterd.Services.Resources.Interfaces
{
    public interface IStaffService
    {
        Task<PagedList<StaffModel>> GetStaff(PagingQueryStringParameters pagingParameters);
    }
}