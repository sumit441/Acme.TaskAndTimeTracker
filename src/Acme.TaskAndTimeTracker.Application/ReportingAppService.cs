using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.TimeEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.TaskAndTimeTracker
{
    [Authorize]
    public class ReportingAppService : ApplicationService
    {
        private readonly IRepository<TimeEntry, Guid> _timeEntryRepository;

        public ReportingAppService(IRepository<TimeEntry, Guid> timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        [Authorize(TaskAndTimeTrackerPermissions.Reports.View)]
        [HttpGet("api/reports/project/{projectId}")]
        public async Task<ReportDto> GetProjectReportAsync(Guid projectId)
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetListAsync(x => x.Task.ProjectId == projectId);
                var totalHours = timeEntries.Sum(x => x.LoggedHours);

                return new ReportDto
                {
                    ProjectId = projectId,
                    TotalHours = totalHours
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while generating the project report.");
            }
        }

        [HttpGet("api/reports/user/{userId}")]
        public async Task<ReportDto> GetUserReportAsync(Guid userId)
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetListAsync(x => x.UserId == userId);
                var totalHours = timeEntries.Sum(x => x.LoggedHours);

                return new ReportDto
                {
                    UserId = userId,
                    TotalHours = totalHours
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while generating the user report.");
            }
        }

        [HttpGet("api/reports/task/{taskId}")]
        public async Task<ReportDto> GetTaskReportAsync(Guid taskId)
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetListAsync(x => x.TaskId == taskId);
                var totalHours = timeEntries.Sum(x => x.LoggedHours);

                return new ReportDto
                {
                    TaskId = taskId,
                    TotalHours = totalHours
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while generating the task report.");
            }
        }

        [HttpGet("api/reports/summary")]
        public async Task<List<ReportSummaryDto>> GetSummaryReportAsync()
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetListAsync();

                var summary = timeEntries
                    .GroupBy(x => x.Task.ProjectId)
                    .Select(group => new ReportSummaryDto
                    {
                        ProjectId = group.Key,
                        TotalHours = group.Sum(x => x.LoggedHours)
                    })
                    .ToList();

                return summary;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while generating the summary report.");
            }
        }
    }
}