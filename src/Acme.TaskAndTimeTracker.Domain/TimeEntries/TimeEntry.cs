using Acme.TaskAndTimeTracker.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.TaskAndTimeTracker.TimeEntries
{
    public class TimeEntry : FullAuditedEntity<Guid>
    {
        [Required]
        public Guid TaskId { get; set; }
        public ProjectTask Task { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Logged hours must be a positive value.")]
        public decimal LoggedHours { get; set; }
        public DateTime LogDate { get; set; }

        [StringLength(500, ErrorMessage = "Notes should not exceed 500 characters.")]
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
