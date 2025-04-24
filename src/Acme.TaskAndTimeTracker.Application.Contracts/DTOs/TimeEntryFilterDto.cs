using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class TimeEntryFilterDto: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
