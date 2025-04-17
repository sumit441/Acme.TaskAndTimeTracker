using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Workflows
{
    public interface IApplicationWorkflowInstanceAppService :
        ICrudAppService<
            ApplicationWorkflowInstanceDto, // Used to show data
            Guid, // Primary key
            PagedAndSortedResultRequestDto, // For paging/sorting
            CreateUpdateApplicationWorkflowInstanceDto> // For create/update
    {
    }
}