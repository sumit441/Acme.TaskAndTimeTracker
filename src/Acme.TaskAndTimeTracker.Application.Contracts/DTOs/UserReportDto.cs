using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class UserReportDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public double TotalLoggedHours { get; set; }
        public List<TimeEntryReportDto> TimeEntries { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
