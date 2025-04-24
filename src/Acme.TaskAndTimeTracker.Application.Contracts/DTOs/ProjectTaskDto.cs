using Acme.TaskAndTimeTracker.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ProjectTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string ProjectName { get; set; }
        public string AssignedUserName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
