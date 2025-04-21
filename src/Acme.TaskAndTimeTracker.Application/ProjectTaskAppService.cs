using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.TaskAndTimeTracker;

[Authorize]
public class ProjectTaskAppService : ApplicationService
{
    private readonly IRepository<ProjectTask, Guid> _taskRepository;

    public ProjectTaskAppService(IRepository<ProjectTask, Guid> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Create)]
    [HttpPost("api/tasks")]
    public async Task<ProjectTaskDto> CreateAsync(CreateUpdateTaskDto input)
    {
        try
        {
            var task = new ProjectTask(
                     GuidGenerator.Create(),
                     input.Title,
                     input.Description,
                     input.DueDate,
                     input.Status,
                     input.Priority,
                     input.ProjectId,
                     input.AssignedUserId ?? Guid.Empty
            );

            await _taskRepository.InsertAsync(task);

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while creating the task.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Delete)]
    [HttpDelete("api/tasks/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            await _taskRepository.DeleteAsync(task);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while deleting the task.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Update)]
    [HttpPut("api/tasks/{id}")]
    public async Task UpdateAsync(Guid id, CreateUpdateTaskDto input)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            task.Title = input.Title;
            task.Description = input.Description;
            task.DueDate = input.DueDate;
            task.Status = input.Status;
            task.Priority = input.Priority;
            task.ProjectId = input.ProjectId;
            task.AssignedUserId = input.AssignedUserId;
            await _taskRepository.UpdateAsync(task);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while updating the task.");
        }
    }

    [HttpGet("api/tasks/{id}")]
    public async Task<ProjectTaskDto> GetAsync(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the task.");
        }
    }

    [HttpGet("api/tasks")]
    public async Task<PagedResultDto<ProjectTaskDto>> GetListAsync(int skipCount = 0, int maxResultCount = 10)
    {
        try
        {
            // Provide a default sorting parameter
            var sorting = nameof(ProjectTask.DueDate); // Sort by DueDate as an example

            // Fetch paginated tasks with sorting
            var tasks = await _taskRepository.GetPagedListAsync(skipCount, maxResultCount, sorting);

            var totalCount = (int)await _taskRepository.GetCountAsync(); // Explicitly cast 'long' to 'int'

            // Map tasks to DTOs
            var taskDtos = tasks.Select(task => new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            }).ToList();

            // Return paginated result
            return new PagedResultDto<ProjectTaskDto>(taskDtos, totalCount);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the task list.");
        }
    }
}