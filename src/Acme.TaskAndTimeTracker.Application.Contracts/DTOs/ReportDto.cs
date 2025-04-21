using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ReportDto
    {
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? TaskId { get; set; }
        public decimal TotalHours { get; set; }
    }
}
