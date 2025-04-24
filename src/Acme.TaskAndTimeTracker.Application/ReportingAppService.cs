using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Projects;
using Acme.TaskAndTimeTracker.Tasks;
using Acme.TaskAndTimeTracker.TimeEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Acme.TaskAndTimeTracker.Reports
{
    [Route("api/reports")]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<ProjectTask, Guid> _taskRepository;
        private readonly IRepository<TimeEntry, Guid> _timeEntryRepository;
        private readonly IRepository<IdentityUser, Guid> _abpUsers;

        public ReportAppService(
            IRepository<Project, Guid> projectRepository,
            IRepository<ProjectTask, Guid> taskRepository,
            IRepository<TimeEntry, Guid> timeEntryRepository,
            IRepository<IdentityUser, Guid> abpUsers)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _timeEntryRepository = timeEntryRepository;
            _abpUsers = abpUsers;
        }

        [HttpGet("projectReport")]
        public async Task<List<ProjectReportDto>> GetProjectReportAsync()
        {
            var projects = await _projectRepository.GetListAsync();
            var tasks = await _taskRepository.GetListAsync();
            var timeEntries = await _timeEntryRepository.GetListAsync();

            var report = projects.Select(project =>
            {
                var projectTasks = tasks.Where(t => t.ProjectId == project.Id).ToList();
                var totalLoggedHours = timeEntries
                    .Where(te => projectTasks.Any(pt => pt.Id == te.TaskId))
                    .Sum(te => te.LoggedHours);

                return new ProjectReportDto
                {
                    ProjectName = project.Name,
                    TotalTasks = projectTasks.Count,
                    TotalLoggedHours = (double)totalLoggedHours,
                    Tasks = projectTasks.Select(t => new TaskReportDto
                    {
                        TaskId = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        Priority = t.Priority,
                        TotalLoggedHours = (double)timeEntries
                            .Where(te => te.TaskId == t.Id)
                            .Sum(te => te.LoggedHours),
                        TimeEntries = timeEntries
                            .Where(te => te.TaskId == t.Id)
                            .Select(te => new TimeEntryReportDto
                            {
                                TimeEntryId = te.Id,
                                LogDate = te.LogDate,
                                LoggedHours = te.LoggedHours,
                                Notes = te.Notes,
                                ProjectName = project.Name
                            }).ToList()
                    }).ToList()
                };
            }).ToList();

            return report;
        }

        [HttpGet("userReport")]
        public async Task<List<UserReportDto>> GetUserReportAsync()
        {
            var users = await _abpUsers.GetListAsync();
            var timeEntries = await _timeEntryRepository.GetListAsync();

            var report = users.Select(user =>
            {
                var userEntries = timeEntries.Where(te => te.UserId == user.Id).ToList();

                return new UserReportDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    TotalLoggedHours = (double)userEntries.Sum(te => te.LoggedHours),
                    TimeEntries = userEntries.Select(te => new TimeEntryReportDto
                    {
                        TimeEntryId = te.Id,
                        LogDate = te.LogDate,
                        LoggedHours = te.LoggedHours,
                        Notes = te.Notes,
                    }).ToList()
                };
            }).ToList();

            return report;
        }
    }
}