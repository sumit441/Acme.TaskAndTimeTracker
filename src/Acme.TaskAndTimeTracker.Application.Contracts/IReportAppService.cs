using Acme.TaskAndTimeTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.TaskAndTimeTracker
{
    public interface IReportAppService : IApplicationService
    {
        Task<List<ProjectReportDto>> GetProjectReportAsync();
        Task<List<UserReportDto>> GetUserReportAsync();
    }
}
