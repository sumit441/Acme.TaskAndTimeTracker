using Acme.TaskAndTimeTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker;

public interface IProjectAppService
{
    Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input);
    Task DeleteAsync(Guid id);
    Task<ProjectDto> GetAsync(Guid id);
    Task<PagedResultDto<ProjectDto>> GetListAsync(ProjectFilterDto input);
    Task<ProjectDto> UpdateAsync(Guid id, CreateUpdateProjectDto input);
}
