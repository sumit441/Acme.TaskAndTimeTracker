using Acme.TaskAndTimeTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.TaskAndTimeTracker
{
    public interface IProjectTaskAppService : IApplicationService
    {
        Task<ProjectTaskDto> CreateAsync(CreateUpdateProjectTaskDto input);
        Task<ProjectTaskDto> UpdateAsync(Guid id, CreateUpdateProjectTaskDto input);
        Task DeleteAsync(Guid id);
        Task<ProjectTaskDto> GetAsync(Guid id);
        Task<PagedResultDto<ProjectTaskDto>> GetListAsync(ProjectTaskFilterDto input);
    }
}
