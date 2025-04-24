using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.TimeEntries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp;
using Microsoft.AspNetCore.Authorization;
using Acme.TaskAndTimeTracker.Permissions;
using Volo.Abp.Identity;
using Acme.TaskAndTimeTracker.Tasks;

namespace Acme.TaskAndTimeTracker
{
    [Authorize]
    public class TimeEntryAppService : ApplicationService
    {
        private readonly IRepository<TimeEntry, Guid> _timeEntryRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ProjectTask, Guid> _taskRepository;
        private readonly IRepository<IdentityUser, Guid> _abpUsers;
        public TimeEntryAppService(IRepository<TimeEntry, Guid> timeEntryRepository, IGuidGenerator guidGenerator, IRepository<IdentityUser, Guid> abpUsers, IRepository<ProjectTask, Guid> taskRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _guidGenerator = guidGenerator;
            _abpUsers = abpUsers;
            _taskRepository = taskRepository;
        }

        [Authorize(TaskAndTimeTrackerPermissions.TimeEntries.Create)]
        [HttpPost("api/time-entries")]
        public async Task<TimeEntryDto> CreateAsync(CreateUpdateTimeEntryDto input)
        {
            try
            {
                var timeEntry = new TimeEntry(GuidGenerator.Create(), input.TaskId, input.UserId, input.LogDate, input.LoggedHours, input.Notes);
                await _timeEntryRepository.InsertAsync(timeEntry);

                return new TimeEntryDto
                {
                    TaskId = timeEntry.TaskId,
                    UserId = timeEntry.UserId,
                    LogDate = timeEntry.LogDate,
                    LoggedHours = timeEntry.LoggedHours,
                    Notes = timeEntry.Notes
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while creating the time entry.");
            }
        }

        [Authorize(TaskAndTimeTrackerPermissions.TimeEntries.Delete)]
        [HttpDelete("api/time-entries/{id}")]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var timeEntry = await _timeEntryRepository.GetAsync(id);
                if (timeEntry == null)
                {
                    throw new UserFriendlyException("Time entry not found.");
                }
                await _timeEntryRepository.DeleteAsync(timeEntry);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while deleting the time entry.");
            }
        }

        [HttpGet("api/time-entries/{id}")]
        public async Task<TimeEntryDto> GetAsync(Guid id)
        {
            try
            {
                var timeEntry = await _timeEntryRepository.GetAsync(id);
                if (timeEntry == null)
                {
                    throw new UserFriendlyException("Time entry not found.");
                }

                return new TimeEntryDto
                {
                    TaskId = timeEntry.TaskId,
                    UserId = timeEntry.UserId,
                    LogDate = timeEntry.LogDate,
                    LoggedHours = timeEntry.LoggedHours,
                    Notes = timeEntry.Notes
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while retrieving the time entry.");
            }
        }

        [HttpGet("api/time-entries")]
        public async Task<PagedResultDto<TimeEntryDto>> GetListAsync(TimeEntryFilterDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
                input.Sorting = "LogDate";

            var entries = await _timeEntryRepository.GetQueryableAsync();
            var users = await _abpUsers.GetQueryableAsync();
            var tasks = await _taskRepository.GetQueryableAsync();

            var query = (from e in entries
                         join u in users on e.UserId equals u.Id into userGroup
                         from user in userGroup.DefaultIfEmpty()
                         join t in tasks on e.TaskId equals t.Id into taskGroup
                         from task in taskGroup.DefaultIfEmpty()
                         select new TimeEntryDto
                         {
                             TaskId = e.TaskId,
                             TaskTitle = task != null ? task.Title : "",
                             UserId = e.UserId,
                             UserName = user != null ? user.UserName : "",
                             LoggedHours = e.LoggedHours,
                             LogDate = e.LogDate,
                             Notes = e.Notes,
                             CreationTime = e.CreationTime
                         })
                        .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                            e => e.TaskTitle.Contains(input.Filter) ||
                                 e.UserName.Contains(input.Filter) ||
                                 e.Notes.Contains(input.Filter));

            var totalCount = await AsyncExecuter.CountAsync(query);

            var items = await AsyncExecuter.ToListAsync(
                query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
            );

            return new PagedResultDto<TimeEntryDto>(totalCount, items);
        }

        [Authorize(TaskAndTimeTrackerPermissions.TimeEntries.Update)]
        [HttpPut("api/time-entries/{id}")]
        public async Task<TimeEntryDto> UpdateAsync(Guid id, CreateUpdateTimeEntryDto input)
        {
            try
            {
                var timeEntry = await _timeEntryRepository.GetAsync(id);
                if (timeEntry == null)
                {
                    throw new UserFriendlyException("Time entry not found.");
                }
                timeEntry.TaskId = input.TaskId;
                timeEntry.UserId = input.UserId;
                timeEntry.LogDate = input.LogDate;
                timeEntry.LoggedHours = input.LoggedHours;
                timeEntry.Notes = input.Notes;
                await _timeEntryRepository.UpdateAsync(timeEntry);

                return new TimeEntryDto
                {
                    TaskId = timeEntry.TaskId,
                    UserId = timeEntry.UserId,
                    LogDate = timeEntry.LogDate,
                    LoggedHours = timeEntry.LoggedHours,
                    Notes = timeEntry.Notes
                };
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while updating the time entry.");
            }
        }
    }
}
