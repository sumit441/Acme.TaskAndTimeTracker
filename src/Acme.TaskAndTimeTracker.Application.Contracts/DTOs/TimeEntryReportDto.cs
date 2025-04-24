using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class TimeEntryReportDto
    {
        public string ProjectName { get; set; }
        public string TaskTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationInHours { get; set; }
        public Guid TimeEntryId { get; set; }
        public DateTime LogDate { get; set; }
        public decimal LoggedHours { get; set; }
        public string Notes { get; set; }
    }
}
