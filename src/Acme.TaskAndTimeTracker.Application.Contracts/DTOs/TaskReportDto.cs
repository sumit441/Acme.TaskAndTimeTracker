using Acme.TaskAndTimeTracker.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Acme.TaskAndTimeTracker.Enum.TaskStatus;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class TaskReportDto
    {
        public string Title { get; set; }
        public TaskStatus Status { get; set; } // From Enum.TaskStatus, convert to string
        public TaskPriority Priority { get; set; } // From Enum.TaskPriority, convert to string
        public string AssignedUser { get; set; }
        public double TotalLoggedHours { get; set; }
        public List<TimeEntryReportDto> TimeEntries { get; set; }
        public Guid TaskId { get; set; }
    }
}
