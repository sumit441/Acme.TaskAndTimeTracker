using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class TimeEntryDto
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal LoggedHours { get; set; }
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
