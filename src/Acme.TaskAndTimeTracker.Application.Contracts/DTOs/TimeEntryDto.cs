using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class TimeEntryDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public decimal LoggedHours { get; set; }
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }
    }

    public class CreateUpdateTimeEntryDto
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public decimal LoggedHours { get; set; }
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }
    }
}
