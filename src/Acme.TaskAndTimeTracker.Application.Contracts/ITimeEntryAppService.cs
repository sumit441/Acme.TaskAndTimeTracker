using Acme.TaskAndTimeTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.TaskAndTimeTracker
{
    public interface ITimeEntryAppService : IApplicationService
    {
        Task<TimeEntryDto> CreateAsync(CreateUpdateTimeEntryDto input);
        Task<TimeEntryDto> UpdateAsync(Guid id, CreateUpdateTimeEntryDto input);
        Task DeleteAsync(Guid id);
        Task<TimeEntryDto> GetAsync(Guid id);
        Task<PagedResultDto<TimeEntryDto>> GetListAsync(TimeEntryFilterDto input);
    }
}
