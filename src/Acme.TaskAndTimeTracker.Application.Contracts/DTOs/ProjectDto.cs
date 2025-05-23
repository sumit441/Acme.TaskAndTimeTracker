﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
