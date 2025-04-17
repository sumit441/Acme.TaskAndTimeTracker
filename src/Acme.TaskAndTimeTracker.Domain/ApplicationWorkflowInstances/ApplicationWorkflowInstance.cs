using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.TaskAndTimeTracker.ApplicationWorkflowInstances
{
    public class ApplicationWorkflowInstance : FullAuditedEntity<Guid>
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

        // Constructor
        public ApplicationWorkflowInstance(Guid id) : base(id)
        {
        }
    }
}
