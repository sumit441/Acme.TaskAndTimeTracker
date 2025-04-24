using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class CreateUpdateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
