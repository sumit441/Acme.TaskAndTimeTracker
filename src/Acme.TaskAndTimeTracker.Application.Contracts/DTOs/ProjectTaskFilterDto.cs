using Acme.TaskAndTimeTracker.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using TaskStatus = Acme.TaskAndTimeTracker.Enum.TaskStatus;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ProjectTaskFilterDto: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // for general text search (Title, Description, ProjectName, etc.)
    }
}
