using Acme.TaskAndTimeTracker.Enum;
using Acme.TaskAndTimeTracker.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace Acme.TaskAndTimeTracker.Tasks
{
    public class ProjectTask : FullAuditedEntity<Guid>
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } // Pending, In Progress, Completed
        public TaskPriority Priority { get; set; } // Low, Medium, High
        public Guid ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public virtual Project Project { get; set; }
        public virtual IdentityUser AssignedUser { get; set; } // Link to the ABP User entity

        // Parameterless constructor for EF Core
        public ProjectTask() { }

        public ProjectTask(Guid id, string title, string description, DateTime dueDate, TaskStatus status, TaskPriority priority, Guid projectId, Guid assignedUserId)
            : base(id)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            Priority = priority;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
        }
    }
}
