using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ReportSummaryDto
    {
        public Guid ProjectId { get; set; }
        public decimal TotalHours { get; set; }
    }
}
