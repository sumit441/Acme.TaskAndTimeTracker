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
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }

        public Guid? AssignedUserId { get; set; }
    }
    public class CreateUpdateTaskDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid? AssignedUserId { get; set; }
    }
}
