using Acme.BookStore.Workflows;
using Acme.TaskAndTimeTracker.ApplicationWorkflowInstances;
using Acme.TaskAndTimeTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.TaskAndTimeTracker
{
    public class ApplicationWorkflowInstanceAppService :
        CrudAppService<
            ApplicationWorkflowInstance, // The entity
            ApplicationWorkflowInstanceDto, // Used to show data
            Guid, // Primary key
            PagedAndSortedResultRequestDto, // For paging/sorting
            CreateUpdateApplicationWorkflowInstanceDto>, // For create/update
        IApplicationWorkflowInstanceAppService
    {
        public ApplicationWorkflowInstanceAppService(IRepository<ApplicationWorkflowInstance, Guid> repository)
            : base(repository)
        {x
        }
    }
}
