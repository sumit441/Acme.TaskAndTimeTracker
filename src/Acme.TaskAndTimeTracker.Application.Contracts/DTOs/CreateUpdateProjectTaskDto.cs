using Acme.TaskAndTimeTracker.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Acme.TaskAndTimeTracker.Enum.TaskStatus;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class CreateUpdateProjectTaskDto
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
        public Guid AssignedUserId { get; set; }
    }
}
