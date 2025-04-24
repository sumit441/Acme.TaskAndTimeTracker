using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class CreateUpdateTimeEntryDto
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public decimal LoggedHours { get; set; }
        [Required]
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }
    }
}
