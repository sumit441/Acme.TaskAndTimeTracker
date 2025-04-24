using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Projects;
using Acme.TaskAndTimeTracker.Tasks;
using Acme.TaskAndTimeTracker.TimeEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

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

        [HttpGet("api/reports/by-project")]
        public async Task<List<ReportResultDto>> GetTotalHoursByProjectAsync(ReportFilterDto input)
        {
            var query = await _timeEntryRepository.GetQueryableAsync();

            if (input.UserId.HasValue)
                query = query.Where(e => e.UserId == input.UserId.Value);

            if (input.StartDate.HasValue)
                query = query.Where(e => e.LogDate >= input.StartDate.Value);

            if (input.EndDate.HasValue)
                query = query.Where(e => e.LogDate <= input.EndDate.Value);

            if (input.ProjectId.HasValue)
            {
                var taskQueryable = await _taskRepository.GetQueryableAsync();
                var taskIds = taskQueryable
                    .Where(t => t.ProjectId == input.ProjectId.Value)
                    .Select(t => t.Id)
                    .ToList();

                query = query.Where(e => taskIds.Contains(e.TaskId));
            }

            var taskQuery = await _taskRepository.GetQueryableAsync();
            var projectQuery = await _projectRepository.GetQueryableAsync();

            var result = await query
                .Join(taskQuery,
                      entry => entry.TaskId,
                      task => task.Id,
                      (entry, task) => new { entry, task })
                .Join(projectQuery,
                      x => x.task.ProjectId,
                      project => project.Id,
                      (x, project) => new { x.entry, ProjectName = project.Name, ProjectId = project.Id })
                .GroupBy(x => new { x.ProjectId, x.ProjectName })
                .Select(g => new ReportResultDto
                {
                    Title = g.Key.ProjectName,
                    TotalHours = (float)g.Sum(e => e.entry.LoggedHours)
                })
                .ToListAsync();

            return result;
        }

        [HttpGet("user")]
        public async Task<List<ReportResultDto>> GetTotalHoursByUserAsync(ReportFilterDto input)
        {
            var query = await _timeEntryRepository.GetQueryableAsync();

            if (input.ProjectId.HasValue)
            {
                var taskQueryable = await _taskRepository.GetQueryableAsync();
                var taskIds = taskQueryable
                    .Where(t => t.ProjectId == input.ProjectId.Value)
                    .Select(t => t.Id)
                    .ToList();

                query = query.Where(e => taskIds.Contains(e.TaskId));
            }

            if (input.StartDate.HasValue)
                query = query.Where(e => e.LogDate >= input.StartDate.Value);

            if (input.EndDate.HasValue)
                query = query.Where(e => e.LogDate <= input.EndDate.Value);

            var userQuery = await _abpUsers.GetQueryableAsync();

            var result = await query
                .Join(userQuery,
                      entry => entry.UserId,
                      user => user.Id,
                      (entry, user) => new { entry, UserName = user.UserName, UserId = user.Id })
                .GroupBy(x => new { x.UserId, x.UserName })
                .Select(g => new ReportResultDto
                {
                    Title = g.Key.UserName,
                    TotalHours = (float)g.Sum(e => e.entry.LoggedHours)
                })
                .ToListAsync();

            return result;
        }

        [HttpGet("task")]
        public async Task<List<ReportResultDto>> GetTotalHoursByTaskAsync(ReportFilterDto input)
        {
            var query = await _timeEntryRepository.GetQueryableAsync();

            if (input.UserId.HasValue)
                query = query.Where(e => e.UserId == input.UserId.Value);

            if (input.StartDate.HasValue)
                query = query.Where(e => e.LogDate >= input.StartDate.Value);

            if (input.EndDate.HasValue)
                query = query.Where(e => e.LogDate <= input.EndDate.Value);

            if (input.ProjectId.HasValue)
            {
                var taskQueryable = await _taskRepository.GetQueryableAsync();
                var taskIds = taskQueryable
                    .Where(t => t.ProjectId == input.ProjectId.Value)
                    .Select(t => t.Id)
                    .ToList();

                query = query.Where(e => taskIds.Contains(e.TaskId));
            }

            var taskQuery = await _taskRepository.GetQueryableAsync();

            var result = await query
                .Join(taskQuery,
                      entry => entry.TaskId,
                      task => task.Id,
                      (entry, task) => new { entry, TaskName = task.Title, TaskId = task.Id })
                .GroupBy(x => new { x.TaskId, x.TaskName })
                .Select(g => new ReportResultDto
                {
                    Title = g.Key.TaskName,
                    TotalHours = (float)g.Sum(e => e.entry.LoggedHours)
                })
                .ToListAsync();

            return result;
        }
    }
}

