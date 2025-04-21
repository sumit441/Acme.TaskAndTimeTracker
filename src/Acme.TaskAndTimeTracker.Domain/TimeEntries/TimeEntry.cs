using Acme.TaskAndTimeTracker.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.TaskAndTimeTracker.TimeEntries
{
    public class TimeEntry : FullAuditedEntity<Guid>
    {
        public Guid TaskId { get; set; }
        public ProjectTask Task { get; set; }
        public Guid UserId { get; set; }
        public decimal LoggedHours { get; set; }
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }

        public TimeEntry(Guid id, Guid taskId, Guid userId, DateTime logDate, decimal loggedHours, string notes) : base(id)
        {
            TaskId = taskId;
            UserId = userId;
            LoggedHours = loggedHours;
            LogDate = logDate;
            Notes = notes;
        }
    }
}
