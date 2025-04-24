using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ReportResultDto
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public float TotalHours { get; set; }
        public string Title { get; set; }
    }
}
