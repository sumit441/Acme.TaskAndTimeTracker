using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ApplicationWorkflowInstanceDto : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }
        public Guid OperationWorkflowInstanceId { get; set; }
        public Guid OperationWorkflowConfigurationId { get; set; }
        public string? InstanceId { get; set; }
        public string InitialData { get; set; }
        public string? IntermediateData { get; set; }
        public Guid WorkflowStageId { get; set; }
        public Guid? WorkflowSubStageId { get; set; }
        public string? Remarks { get; set; }
        public DateTime WorkflowStageDate { get; set; }
        public string? RequestedBy { get; set; }
    }

    public class CreateUpdateApplicationWorkflowInstanceDto
    {
        public Guid? TenantId { get; set; }
        public Guid OperationWorkflowInstanceId { get; set; }
        public Guid OperationWorkflowConfigurationId { get; set; }
        public string? InstanceId { get; set; }
        public string InitialData { get; set; }
        public string? IntermediateData { get; set; }
        public Guid WorkflowStageId { get; set; }
        public Guid? WorkflowSubStageId { get; set; }
        public string? Remarks { get; set; }
        public DateTime WorkflowStageDate { get; set; }
        public string? RequestedBy { get; set; }
    }
}
