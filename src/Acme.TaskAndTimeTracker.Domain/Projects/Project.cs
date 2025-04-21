using Acme.TaskAndTimeTracker.Tasks;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.TaskAndTimeTracker.Projects
{
    public class Project : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public Project()
        {
            ProjectTasks = new List<ProjectTask>();
        }

        public Project(Guid id, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
            ProjectTasks = new List<ProjectTask>();
        }
    }
}
