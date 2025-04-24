using System.Collections.Generic;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ProjectReportDto
    {
        public string Name { get; set; }
        public int TotalTasks { get; set; }
        public double TotalLoggedHours { get; set; }
        public List<TaskReportDto> Tasks { get; set; }
        public string ProjectName { get; set; }
    }
}
