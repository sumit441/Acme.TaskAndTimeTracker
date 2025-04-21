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

namespace Acme.TaskAndTimeTracker
{
    [Authorize]
    public class TimeEntryAppService : ApplicationService
    {
        private readonly IRepository<TimeEntry, Guid> _timeEntryRepository;
        private readonly IGuidGenerator _guidGenerator;
        public TimeEntryAppService(IRepository<TimeEntry, Guid> timeEntryRepository, IGuidGenerator guidGenerator)
        {
            _timeEntryRepository = timeEntryRepository;
            _guidGenerator = guidGenerator;
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
                    Id = timeEntry.Id,
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
                    Id = timeEntry.Id,
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
        public async Task<List<TimeEntryDto>> GetListAsync()
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetListAsync();

                var timeEntryDtos = new List<TimeEntryDto>();
                foreach (var timeEntry in timeEntries)
                {
                    timeEntryDtos.Add(new TimeEntryDto
                    {
                        Id = timeEntry.Id,
                        TaskId = timeEntry.TaskId,
                        UserId = timeEntry.UserId,
                        LogDate = timeEntry.LogDate,
                        LoggedHours = timeEntry.LoggedHours,
                        Notes = timeEntry.Notes
                    });
                }

                return timeEntryDtos;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw new UserFriendlyException("An error occurred while retrieving the time entry list.");
            }
        }

        [Authorize(TaskAndTimeTrackerPermissions.TimeEntries.Update)]
        [HttpPut("api/time-entries/{id}")]
        public async Task UpdateAsync(Guid id, CreateUpdateTimeEntryDto input)
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
